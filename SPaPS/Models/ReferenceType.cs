using System;
using System.Collections.Generic;

namespace SPaPS.Models
{
    public partial class ReferenceType
    {
        public ReferenceType()
        {
            References = new HashSet<Reference>();
        }

        public long ReferenceTypeId { get; set; }
        public string Description { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Reference> References { get; set; }
    }
}
