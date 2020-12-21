using System;
using System.Collections.Generic;
using System.Text;

namespace Alef.RoomMonitoring.Configuration.Model
{
    public class JobSetting
    {
        /// <summary>
        /// Job name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// job trigger time in case of on time triggered job 
        /// </summary>
        public TriggerTime TriggerTimeValue { get; set; }

        /// <summary>
        /// Period time (in second) in case of  simple triggered job
        /// </summary>
        public int? SimpleTriggerTime { get; set; }

        /// <summary>
        /// Flag with job activity 
        /// </summary>
        public bool Enabled { get; set; }

    }
}
