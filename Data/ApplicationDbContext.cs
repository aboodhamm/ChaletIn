using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Chaletin.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            SqlConnectionStringBuilder ConnectionString = new SqlConnectionStringBuilder()
            {
                DataSource = ".",
                InitialCatalog = "Chaletin",
                IntegratedSecurity = true, //for Windows Authrntication
            };
            optionBuilder.UseSqlServer(ConnectionString.ToString());
        }
    }
}
