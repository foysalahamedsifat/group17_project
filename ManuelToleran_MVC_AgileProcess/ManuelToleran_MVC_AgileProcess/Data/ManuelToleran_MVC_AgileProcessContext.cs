using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ManuelToleran_MVC_AgileProcess.Models;

namespace ManuelToleran_MVC_AgileProcess.Data
{
    public class ManuelToleran_MVC_AgileProcessContext : DbContext
    {
        public ManuelToleran_MVC_AgileProcessContext (DbContextOptions<ManuelToleran_MVC_AgileProcessContext> options)
            : base(options)
        {
        }

        public DbSet<ManuelToleran_MVC_AgileProcess.Models.Movie> Movie { get; set; } = default!;
    }
}
