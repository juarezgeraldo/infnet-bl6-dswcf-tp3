using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace infnet_bl6_daw_tp1.Service
{
    public class infnet_bl6_daw_tp1DbContextFactory : IDesignTimeDbContextFactory<infnet_bl6_daw_tp1DbContext>
    {
        public infnet_bl6_daw_tp1DbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<infnet_bl6_daw_tp1DbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=banco_amigos;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            //"Server=(localdb)\\mssqllocaldb;Database=banco_amigos;Trusted_Connection=True;MultipleActiveResultSets=true; User Id=sa; Password=Abc123456;");
            //                "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=banco_amigos;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return new infnet_bl6_daw_tp1DbContext(optionsBuilder.Options);
        }
    }
}
