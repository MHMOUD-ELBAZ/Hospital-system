using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class HospitalDbContext : IdentityDbContext <AppUser>
    {
        public HospitalDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public HospitalDbContext(): base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("server = .; Database = hospital; Trusted_Connection = true; TrustServerCertificate = true; MultipleActiveResultSets = true;");
        }
    }
}
