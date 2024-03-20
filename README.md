using Microsoft.EntityFrameworkCore;
using SPM3._0Service.Models;

namespace SPM3._0Service.Data
{
    public class OracleDbContext : DbContext
    {
        public DbSet<Authority> AUTHORITIES_MSB { get; set; }
        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authority>().ToTable("AUTHORITIES_MSB", "cardcolv")
        }
    }
}

builder.Services.AddDbContext<OracleDbContext>(options =>
{
    var conStrBuilder = new OracleConnectionStringBuilder(
          builder.Configuration.GetConnectionString("DefaultConnection"));
    //conStrBuilder.Password = builder.Configuration["DbPassword"];

    options.UseOracle(conStrBuilder.ConnectionString, options => options.UseOracleSQLCompatibility("11"));
});
