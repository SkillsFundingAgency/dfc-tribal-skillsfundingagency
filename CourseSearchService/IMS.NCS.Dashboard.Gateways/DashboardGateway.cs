using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using IMS.NCS.Dashboard.Common;
using IMS.NCS.Dashboard.Entities;
using System.Data.Common;

namespace IMS.NCS.Dashboard.Gateways
{
    /// <summary>
    /// Implementation of Dashboard functions to
    /// access the SQL Server NCSImportControl db.
    /// </summary>
    public class DashboardGateway : IDashboardGateway
    {
        /// <summary>
        /// Gets a list of Jobs for the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>A DashboardJob of data.</returns>
        DashboardJob IDashboardGateway.GetJobList(JobSearchCriteria searchCriteria)
        {
            DashboardJob dashboardJob = new DashboardJob();

            using (SqlConnection sqlConnection = new SqlConnection(Utilities.GetDatabaseConnection()))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = Constants.Database.UspGetJobsByCriteria;

                    if (searchCriteria.StartDate != DateTime.MinValue)
                    {
                        AddParameter(command, SqlDbType.DateTime, ParameterDirection.Input, Constants.UspGetJobsByCriteriaParameters.StartDate, searchCriteria.StartDate);
                    }
                    else
                    {
                        AddParameter(command, SqlDbType.DateTime, ParameterDirection.Input, Constants.UspGetJobsByCriteriaParameters.StartDate, DBNull.Value);
                    }
                    if (searchCriteria.EndDate != DateTime.MinValue)
                    {
                        AddParameter(command, SqlDbType.DateTime, ParameterDirection.Input, Constants.UspGetJobsByCriteriaParameters.EndDate, searchCriteria.EndDate);
                    }
                    else
                    {
                        AddParameter(command, SqlDbType.DateTime, ParameterDirection.Input, Constants.UspGetJobsByCriteriaParameters.EndDate, DBNull.Value);
                    }

                    
                    AddBoolParameter(command, SqlDbType.Bit, ParameterDirection.Input, Constants.UspGetJobsByCriteriaParameters.InProgressJobs, searchCriteria.InProgressJobs);
                    AddBoolParameter(command, SqlDbType.Bit, ParameterDirection.Input, Constants.UspGetJobsByCriteriaParameters.CompletedJobs, searchCriteria.CompletedJobs);
                    AddBoolParameter(command, SqlDbType.Bit, ParameterDirection.Input, Constants.UspGetJobsByCriteriaParameters.FailedJobs, searchCriteria.FailedJobs);
                    AddParameter(command, SqlDbType.VarChar, ParameterDirection.Input, Constants.UspGetJobsByCriteriaParameters.SortBy, searchCriteria.SortBy);
                    AddParameter(command, SqlDbType.Int, ParameterDirection.Input, Constants.UspGetJobsByCriteriaParameters.NoOfRecords, searchCriteria.RecordsPerPage);
                    AddParameter(command, SqlDbType.Int, ParameterDirection.Input, Constants.UspGetJobsByCriteriaParameters.PageNo, searchCriteria.NextPage);
                    AddParameter(command, SqlDbType.Int, ParameterDirection.Output, Constants.UspGetJobsByCriteriaParameters.TotalRows);

                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    List<Job> jobsList = new List<Job>();
                    while (reader.Read())
                    {
                        Job job = new Job();
                        job.CurrentStep = (string)reader[Constants.UspGetJobsByCriteriaColumns.CurrentStep];
                        object tempObj = reader[Constants.UspGetJobsByCriteriaColumns.ElapsedTime];
                        if (tempObj != DBNull.Value)
                        {
                            job.ElapsedTime = (long)tempObj;
                        }

                        tempObj = reader[Constants.UspGetJobsByCriteriaColumns.JobId];
                        if (tempObj != DBNull.Value)
                        {
                            job.JobId = (int)reader[Constants.UspGetJobsByCriteriaColumns.JobId];
                        }

                        tempObj = reader[Constants.UspGetJobsByCriteriaColumns.JobName];
                        if (tempObj != DBNull.Value)
                        {
                            job.JobName = (string)reader[Constants.UspGetJobsByCriteriaColumns.JobName];
                        }

                        tempObj = reader[Constants.UspGetJobsByCriteriaColumns.ProcessEnd];
                        if (tempObj != DBNull.Value)
                        {
                            job.ProcessEnd = (DateTime)reader[Constants.UspGetJobsByCriteriaColumns.ProcessEnd];
                        }

                        tempObj = reader[Constants.UspGetJobsByCriteriaColumns.ProcessStart];
                        if (tempObj != DBNull.Value)
                        {
                            job.ProcessStart = (DateTime)reader[Constants.UspGetJobsByCriteriaColumns.ProcessStart];
                        }
                        job.Status = (string)reader[Constants.UspGetJobsByCriteriaColumns.Status];

                        jobsList.Add(job);
                    }

                    // need to close the reader to get hold of the output params
                    // and create our paging values
                    reader.Close();
                    dashboardJob.CurrentPageNo = searchCriteria.NextPage;
                    dashboardJob.TotalRecords = Int32.Parse(GetOutputParameter(command, Constants.UspGetJobsByCriteriaParameters.TotalRows));
                    var result = Decimal.Divide((decimal)dashboardJob.TotalRecords, (decimal)searchCriteria.RecordsPerPage);
                    var totalPages = Math.Ceiling((decimal)result);
                    dashboardJob.TotalPages = (int)totalPages;

                    command.Connection.Close();

                    dashboardJob.Jobs = jobsList;
                }
            }

            return dashboardJob;
        }

        /// <summary>
        /// Gets the Job and Step details for a Job.
        /// </summary>
        /// <param name="jobId">The id of the Job.</param>
        /// <returns>A Job and list of Steps for that Job.</returns>
        DashboardDetailJob IDashboardGateway.GetJobDetails(int jobId)
        {
            bool result;
            Int64 elapsedTime;
            DashboardDetailJob detailsJob = null;
            DateTime dateTimeResult;

            using (SqlConnection sqlConnection = new SqlConnection(Utilities.GetDatabaseConnection()))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = Constants.Database.UspGetJobByJobId;

                    AddParameter(command, SqlDbType.Int, ParameterDirection.Input, Constants.UspGetJobByJobIdParameters.JobId, jobId);
                    AddParameter(command, SqlDbType.VarChar, ParameterDirection.Output, Constants.UspGetJobByJobIdParameters.JobName);
                    AddParameter(command, SqlDbType.DateTime, ParameterDirection.Output, Constants.UspGetJobByJobIdParameters.JobProcessStart);
                    AddParameter(command, SqlDbType.DateTime, ParameterDirection.Output, Constants.UspGetJobByJobIdParameters.JobProcessEnd);
                    AddParameter(command, SqlDbType.BigInt, ParameterDirection.Output, Constants.UspGetJobByJobIdParameters.JobElapsedTime);

                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Job job = new Job();
                    List<JobStep> steps = new List<JobStep>();
                    
                    // and get our Step data from the data set sent back
                    while (reader.Read())
                    {
                        JobStep step = new JobStep();
                        result = Int64.TryParse(reader[Constants.UspGetJobByJobIdColumns.StepElapsedTime].ToString(), out elapsedTime);
                        if (result)
                        {
                            step.ElapsedTime = elapsedTime;
                        }

                        result = DateTime.TryParse(reader[Constants.UspGetJobByJobIdColumns.StepProcessEnd].ToString(), out dateTimeResult);
                        if (result)
                        {
                            step.ProcessEnd = dateTimeResult;
                        }

                        result = DateTime.TryParse(reader[Constants.UspGetJobByJobIdColumns.StepProcessStart].ToString(), out dateTimeResult);
                        if (result)
                        {
                            step.ProcessStart = dateTimeResult;
                        }

                        step.Status = (string)reader[Constants.UspGetJobByJobIdColumns.Status];
                        step.StepName = (string)reader[Constants.UspGetJobByJobIdColumns.StepName];

                        steps.Add(step);
                    }

                    // need to close the reader to get hold of the output params
                    // and create our Job details
                    reader.Close();
                    job.JobId = jobId;
                    job.JobName = GetOutputParameter(command, Constants.UspGetJobByJobIdParameters.JobName);

                    result = DateTime.TryParse(GetOutputParameter(command, Constants.UspGetJobByJobIdParameters.JobProcessStart), out dateTimeResult);
                    if (result)
                    {
                        job.ProcessStart = dateTimeResult;
                    }
                    result = DateTime.TryParse(GetOutputParameter(command, Constants.UspGetJobByJobIdParameters.JobProcessEnd), out dateTimeResult);
                    if (result)
                    {
                        job.ProcessEnd = dateTimeResult;
                    }

                    result = Int64.TryParse(GetOutputParameter(command, Constants.UspGetJobByJobIdParameters.JobElapsedTime), out elapsedTime);
                    if (result)
                    {
                        job.ElapsedTime = elapsedTime;
                    }
                    
                    command.Connection.Close();

                    detailsJob = new DashboardDetailJob();
                    detailsJob.DetailJob = job;
                    detailsJob.Steps = steps;
                }
            }

            return detailsJob;
        }

        #region Private methods

        /// <summary>
        /// Creates a SqlParameter and adds it to the Command.Parameters collection.
        /// </summary>
        /// <param name="commmand">The command to add the parameter to.</param>
        /// <param name="sqlDbType">The data type of the parameter.</param>
        /// <param name="direction">Identifies the in / out direction of the parameter.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="parameterValue">The value to pass into the parameter.</param>
        private static void AddParameter(SqlCommand commmand, SqlDbType sqlDbType, ParameterDirection direction, string parameterName, object parameterValue)
        {
            SqlParameter parameter = new SqlParameter();

            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.Direction = direction;
            parameter.SqlDbType = sqlDbType;

            commmand.Parameters.Add(parameter);
        }


        /// <summary>
        /// Creates a SqlParameter with no value ( can be used for Output params ) and adds it to the Command.Parameters collection.
        /// </summary>
        /// <param name="commmand">The command to add the parameter to.</param>
        /// <param name="sqlDbType">The data type of the parameter.</param>
        /// <param name="direction">Identifies the in / out direction of the parameter.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        private static void AddParameter(SqlCommand commmand, SqlDbType sqlDbType, ParameterDirection direction, string parameterName)
        {
            SqlParameter parameter = new SqlParameter();

            parameter.ParameterName = parameterName;
            parameter.Direction = direction;
            parameter.SqlDbType = sqlDbType;
            parameter.Size = -1;

            commmand.Parameters.Add(parameter);
        }


        /// <summary>
        /// Creates a Bit SqlParameter from the boolean value passed in and adds it to the Command.Parameters collection.
        /// </summary>
        /// <param name="commmand">The command to add the parameter to.</param>
        /// <param name="sqlDbType">The data type of the parameter.</param>
        /// <param name="direction">Identifies the in / out direction of the parameter.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="parameterValue">The value to pass into the parameter.</param>
        private static void AddBoolParameter(SqlCommand commmand, SqlDbType sqlDbType, ParameterDirection direction, string parameterName, bool parameterValue)
        {
            SqlParameter parameter = new SqlParameter();

            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue == true ? 1 : 0;
            parameter.Direction = direction;
            parameter.SqlDbType = sqlDbType;

            commmand.Parameters.Add(parameter);
        }


        /// <summary>
        /// Gets the output paramter from the command object.
        /// </summary>
        /// <param name="command">The commmand containing the output parameter.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A string representing the parameter, if the parameter is not found then an empty string is returned.</returns>
        private static string GetOutputParameter(SqlCommand command, string parameterName)
        {
            string paramValue = string.Empty;

            if (command.Parameters[parameterName].Value != null)
            {
                paramValue = command.Parameters[parameterName].Value.ToString();
            }

            return paramValue;
        }


        /// <summary>
        /// Gets a DateTime from a string value passed in.
        /// </summary>
        /// <param name="columnValue">The string to parse.</param>
        /// <returns>A valid DateTime object</returns>
        private static DateTime GetDateTime(string columnValue)
        {
            if (columnValue.Length > 0)
            {
                DateTime dateTimeResult;
                bool result = DateTime.TryParse(columnValue, out dateTimeResult);
                if (result)
                {
                    return dateTimeResult;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        
        #endregion
    }
}
