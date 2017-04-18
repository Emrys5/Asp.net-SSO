namespace Emrys.SSO.Common
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SessionsDB : DbContext
    {
        public SessionsDB()
            : base("name=SessionsDB")
        {
        }

        public virtual DbSet<ASPStateTempSessions> ASPStateTempSessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
