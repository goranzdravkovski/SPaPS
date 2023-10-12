using System;
using System.Collections.Generic;

namespace SPaPS.Models
{
    public partial class Request
    {
        public long RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public long ServiceId { get; set; }
        public long ActivityId { get; set; }
        public long ClientId { get; set; }
        public int? BuildingTypeId { get; set; }
        public int? BuildingSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Color { get; set; }
        public int? NoOfWindows { get; set; }
        public int? NoOfDoors { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual Service? Service { get; set; } = null!;
        public virtual Activity? Activity { get; set; } = null!;
        public virtual Client? Client { get; set; } = null!;
    }
}
