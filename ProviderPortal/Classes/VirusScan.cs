using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tribal.SkillsFundingAgency.ProviderPortal
{
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// The virus scanner engine type to use
    /// </summary>
    public enum VirusScanEngineType
    {
        /// <summary>
        /// Disables the virus scanner, no scanning will take place and all files are passed without exception
        /// </summary>
        None,
        /// <summary>
        /// Uses the command line interface exe for ClamWin virus scanner available from http://www.clamwin.com/.  ClamWin should be set up to have it's definition files moved to it's own program directory, rather than the default AppData folder that will not be accessible to the application pool
        /// </summary>
        ClamWin,
        /// <summary>
        /// Use the installed Sophos file scanner using it's command line interface
        /// </summary>
        Sophos,
        /// <summary>
        /// Use the installed virus scanner on access real-time scanning
        /// </summary>
        InstalledOnAccessVirusScanner
    }

    /// <summary>
    /// A Virus scanner which will scan files using ClamWin or Sophos using their built in command line scanner, or will use any virus scanner that is installed which is
    /// set to scan in real-time
    /// </summary>
    public class VirusScan
    {
        string _scanExePathFileName;
        VirusScanEngineType _virusScanEngineType;
        const string CommandFormatClamWin = "--database=\"{0}\" {1}";
        const string CommandFormatSophosCli = "-sc -s -extensive -zip -gzip -arj -cmz -tar -archive -cab -loopback -mime -oe -tnef -pua \"{0}\"";

        /// <summary>
        /// Virus scanner which will scan files using ClamWin or Sophos using their built in command line scanner, or will use any virus scanner that is installed which is
        /// set to scan in real-time
        /// </summary>
        /// <param name="scanExePathFileName">The path to the virus scanner command line EXE, for ClamWin this by default is C:\Program Files\ClamWin\bin\clamscan.exe </param>
        /// <param name="virusScanEngineType">The virus scanning engine type</param>
        public VirusScan(string scanExePathFileName, VirusScanEngineType virusScanEngineType)
        {
            _scanExePathFileName = scanExePathFileName;
            _virusScanEngineType = virusScanEngineType;
            this.ClamWinVirusDefinationPath = null;
            this.WaitForOnAccessScanner = 200;
        }

        /// <summary>
        /// Virus scanner which will scan files using ClamWin
        /// </summary>
        /// <param name="scanExePathFileName"></param>
        /// <param name="clamWinVirusDefinitionPath"></param>
        public VirusScan(string scanExePathFileName, string clamWinVirusDefinitionPath)
            : this(scanExePathFileName, VirusScanEngineType.ClamWin)
        {
            this.ClamWinVirusDefinationPath = clamWinVirusDefinitionPath;
        }

        /// <summary>
        /// The full path to the ClamWin virus definition file, not require for Sophos
        /// </summary>
        public string ClamWinVirusDefinationPath { get; set; }

        /// <summary>
        /// Sets or gets the time in milliseconds to wait when the virus engine is InstalledOnAccessVirusScanner for the scanner to intercept the file, by default this is 200ms. 
        /// This setting has no effect when using ClamWin of Sophos scanning
        /// </summary>
        public int WaitForOnAccessScanner { get; set; }

        /// <summary>
        /// Scans a byte array of the file.
        /// </summary>
        /// <param name="data">The file data</param>
        /// <param name="fileExtension">The file extension</param>
        /// <returns>Returns true if the scan was successful and no virus found, returns false if a virus found</returns>
        /// <exception cref="ArgumentException">Thrown if the scan engine is ClamWin and no virus definition path has been set</exception>
        /// <exception cref="FileNotFoundException">Thrown when the command line scanner EXE can't be found</exception>
        /// <exception cref="TimeoutException">Thrown when the command line scanner doesn't respond in a timely manner</exception>
        public bool Scan(byte[] data, string fileExtension="")
        {
            if (_virusScanEngineType == VirusScanEngineType.None) return true;

            if (_virusScanEngineType == VirusScanEngineType.ClamWin
                && (string.IsNullOrEmpty(this.ClamWinVirusDefinationPath) || !Directory.Exists(this.ClamWinVirusDefinationPath)))
            {
                throw new ArgumentException("The virus database for ClamWin AV scanner has not been specified or the path does not exist");
            }

            // Check the clam exe exists
            if (_virusScanEngineType != VirusScanEngineType.InstalledOnAccessVirusScanner && !File.Exists(_scanExePathFileName))
            {
                throw new FileNotFoundException(string.Format("The scan engine exe set as {0} was not found, check the path and that the virus scanner is installed at that location", _scanExePathFileName));
            }

            // Create temporary directory to save the file, the directory is in the temp location
            string tempPath = Path.GetTempPath();
            tempPath = Path.Combine(tempPath, System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempPath);

            string extension;
            if (_virusScanEngineType == VirusScanEngineType.InstalledOnAccessVirusScanner)
            {
                // Save with the files proper extension to allow the installed on access virus scanner to deal with the file based on it's list of file types to scan
                extension = fileExtension;
            }
            else
            {
                // Use a temporary extension so any installed on access scanner ignores the file to allow the chosen scanner to scan it
                extension = ".temp_virus_scan";
            }

            // Create temp file name and path
            string tempFileName = string.Concat(System.Guid.NewGuid().ToString("N"), extension);
            string tempFullPathName = Path.Combine(tempPath, tempFileName);

            // Save the file to the temporary location
            System.IO.File.WriteAllBytes(tempFullPathName, data);

            int exitCode;
            if (_virusScanEngineType != VirusScanEngineType.InstalledOnAccessVirusScanner)
            {
                // Create the process that will scan the file
                using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = _scanExePathFileName;
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.CreateNoWindow = true;
                    switch (_virusScanEngineType)
                    {
                        case VirusScanEngineType.ClamWin:
                            process.StartInfo.Arguments = string.Format(CommandFormatClamWin, this.ClamWinVirusDefinationPath, tempPath);
                            break;
                        case VirusScanEngineType.Sophos:
                            process.StartInfo.Arguments = string.Format(CommandFormatSophosCli, tempFullPathName);
                            break;
                    }

                    process.Start();
                    // MB - increased time-out time, since scanning a tiny 
                    // csv file locally was taking over a minute with Sophos.
                    if (process.WaitForExit((int)TimeSpan.FromSeconds(180).TotalMilliseconds))
                    {
                        exitCode = process.ExitCode;
                    }
                    else
                    {
                        throw new TimeoutException("The Virus scan engine didn't respond in a timely manner and the process has timed out");
                    }
                }
            }
            else
            {
                // Waiting for built in virus scanner so pause for a short time to let file get written and scanned
                System.Threading.Thread.Sleep(this.WaitForOnAccessScanner);
                exitCode = 0;
            }

            if (exitCode == 0)
            {
                // Re-open the temporary file and ensure it is the same, this caters for on access scanning detecting a virus (the file will be cleaned or deleted) 
                // or where the file was cleaned before ClamWin/Sophos scanned the file giving a false clean result
                try
                {
                    // Get the temp file
                    byte[] fileFromTemp = File.ReadAllBytes(tempFullPathName);
                    // Create a hash code to compare to the original
                    MD5 md5 = System.Security.Cryptography.MD5.Create();
                    string tempFileHash = Convert.ToBase64String(md5.ComputeHash(fileFromTemp));
                    string originalFileHash = Convert.ToBase64String(md5.ComputeHash(data));
                    if (!tempFileHash.Equals(originalFileHash))
                    {
                        // Hashes do not match, file has been modified
                        exitCode = -2;
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    // File been removed by on access scanner so must have a virus
                    exitCode = -4;
                }
                catch (System.UnauthorizedAccessException)
                {
                    // File locked in use, possibly by a virus scanner that has quarantined it
                    exitCode = -3;
                }
            }

            // Clean up
            try
            {
                File.Delete(tempFullPathName);
                Directory.Delete(tempPath);
            }
            catch
            {
                // Directory or file may be locked if detected by another virus scanner
                exitCode = -5;
            }

            // Check exit code and return true if no problems found
            return exitCode == 0;
        }
    }
}