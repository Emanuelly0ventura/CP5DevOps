using DimDim.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<DimGroup> DimGroups => Set<DimGroup>();
    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DimGroup>(entity =>
        {
            entity.ToTable("DimGroups");
            entity.Property(x => x.Name).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(250);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("Expenses");
            entity.Property(x => x.Title).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Amount).HasColumnType("decimal(10,2)");
            entity.Property(x => x.Category).HasMaxLength(60);
            entity.HasOne(x => x.DimGroup)
                .WithMany(x => x.Expenses)
                .HasForeignKey(x => x.DimGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
