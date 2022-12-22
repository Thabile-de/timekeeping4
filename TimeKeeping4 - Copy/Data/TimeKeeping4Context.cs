using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeKeeping4.Model;

namespace TimeKeeping4.Data
{
    public class TimeKeeping4Context : DbContext
    {
        public TimeKeeping4Context (DbContextOptions<TimeKeeping4Context> options)
            : base(options)
        {
        }

        public DbSet<TimeKeeping4.Model.Login> Login { get; set; } = default!;

        public DbSet<TimeKeeping4.Model.Registration> Registration { get; set; }
    }
}
