using System;
using System.Collections.Generic;

namespace SPaPS.Models
{
    public partial class ClientService
    {
        public long ClientServiceId { get; set; }
        public long ClientId { get; set; }
        public long ServiceId { get; set; }

        public virtual Service Service { get; set; } = null!;
        public virtual Client Client { get; set; } = null!;
    }
}
