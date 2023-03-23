using infnet_bl6_daw_tp1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace infnet_bl6_daw_tp1.Service
{
    public class infnet_bl6_daw_tp1DbContext : DbContext
    {
        public infnet_bl6_daw_tp1DbContext(DbContextOptions<infnet_bl6_daw_tp1DbContext> options) : base(options) { }

        public DbSet<Amigo> Amigos { get; set; } = default!;
    }
}
