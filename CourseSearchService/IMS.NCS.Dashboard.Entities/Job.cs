using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.Dashboard.Entities
{
    /// <summary>
    /// Job entity for display on the Data Import Dashboard
    /// </summary>
    public class Job
    {
        /// <summary>
        /// The Id of the Job.
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// The name of the Job ( from Job_Metadata ).
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// The ProcessStart date time.
        /// </summary>
        public DateTime ProcessStart { get; set; }

        /// <summary>
        /// The ProcessEnd date time.
        /// </summary>
        public DateTime ProcessEnd { get; set; }

        /// <summary>
        /// The elapsed time in milliseconds.
        /// </summary>
        public long ElapsedTime { get; set; }

        /// <summary>
        /// The name of the current Step.
        /// </summary>
        public string CurrentStep { get; set; }

        /// <summary>
        /// The status of the current Step.
        /// </summary>
        public string Status { get; set; }
    }
}
