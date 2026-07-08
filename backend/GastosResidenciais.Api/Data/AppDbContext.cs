using GastosResidenciais.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Api.Data;

/// <summary>
/// Contexto do Entity Framework Core responsável por mapear as entidades
/// do domínio para o banco SQLite (arquivo gastos.db), garantindo persistência
/// dos dados entre execuções da aplicação.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Person> People => Set<Person>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Age).IsRequired();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(500);

            // decimal(18,2) evita problemas de arredondamento com valores monetários
            entity.Property(t => t.Value).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(t => t.Type).IsRequired();

            // Regra de negócio: ao deletar uma pessoa, todas as suas transações
            // devem ser apagadas junto (delete em cascata).
            entity.HasOne(t => t.Person)
                  .WithMany(p => p.Transactions)
                  .HasForeignKey(t => t.PersonId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
