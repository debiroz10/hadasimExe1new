using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hadasimExe1new.Models;
using hadsimnew.Models;

namespace hadasimExe1new.Data
{
    public class hadasimExe1newContext : DbContext
    {
        public hadasimExe1newContext (DbContextOptions<hadasimExe1newContext> options)
            : base(options)
        {
        }

        public DbSet<hadasimExe1new.Models.Client> Client { get; set; } = default!;

        public DbSet<hadsimnew.Models.Vaccination>? Vaccination { get; set; }
    }
}
