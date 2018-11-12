using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMS.NCS.Dashboard.Entities
{
    /// <summary>
    /// JobStep entity for display on the Data Import Dashboard
    /// </summary>
    public class JobStep
    {
        /// <summary>
        /// The name of the Step.
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// The status of the Step.
        /// </summary>
        public string Status { get; set; }

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
        public Int64 ElapsedTime { get; set; }
    }
}
