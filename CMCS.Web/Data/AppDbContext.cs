using Microsoft.EntityFrameworkCore;
using CMCS.Web.Models;

namespace CMCS.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Claim> Claims { get; set; }
    }
}

