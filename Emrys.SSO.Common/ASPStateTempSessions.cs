namespace Emrys.SSO.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ASPStateTempSessions
    {
        [Key]
        [StringLength(88)]
        public string SessionId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Expires { get; set; }

        public DateTime LockDate { get; set; }

        public int LockId { get; set; }

        public int Timeout { get; set; }

        public bool Locked { get; set; }

        public string SessionItem { get; set; }

        public int Flags { get; set; }

        public bool IsPassport { get; set; }

        public string Token { get; set; }
    }
}
