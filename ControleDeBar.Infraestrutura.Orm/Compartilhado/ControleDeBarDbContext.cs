using ControleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Infraestrutura.Orm.ModuloMesa;
using Microsoft.EntityFrameworkCore;

namespace ControleDeBar.Infraestrutura.Orm.Compartilhado;

public class ControleDeBarDbContext : DbContext
{
    public DbSet<Mesa> Mesas { get; set; }

    public ControleDeBarDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MapeadorMesaEmOrm());

        base.OnModelCreating(modelBuilder);
    }
}
