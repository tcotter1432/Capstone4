using System;
using System.Collections.Generic;

namespace Capstone4Take22343187.Models
{
    public partial class Tasks
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime DueDate { get; set; }
        public string OwnerId { get; set; }

        public virtual AspNetUsers Owner { get; set; }
    }
}
