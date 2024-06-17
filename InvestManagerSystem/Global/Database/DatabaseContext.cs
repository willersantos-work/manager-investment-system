using InvestManagerSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InvestManagerSystem.Global.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<FinancerProduct> FinancerProduct { get; set; }
        public DbSet<Investment> Investment { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
