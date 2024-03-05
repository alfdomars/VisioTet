using Microsoft.EntityFrameworkCore;
using VisioTet.Models;

namespace VisioTet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Discount> Discounts { get; set; }
    }
}
