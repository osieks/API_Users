using Microsoft.EntityFrameworkCore;

namespace APIKrolowie.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer("Server=.\\DESKTOP-LOU85FK;Database=UsersDB2;Trusted_Connection=true;TrustServerCertificate=true;");
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-LOU85FK;Initial Catalog=UsersDB; Integrated Security=False;Trusted_Connection=true;TrustServerCertificate=true;");
        }

        public DbSet<Models.Users> Users { get; set; }
    }
}
