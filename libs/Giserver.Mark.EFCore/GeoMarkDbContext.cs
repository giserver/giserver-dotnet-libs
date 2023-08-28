namespace Giserver.Mark.EFCore;

internal class GeoMarkDbContext : DbContext
{
    public DbSet<Layer> Layers => Set<Layer>();
    public DbSet<Marker> Markers => Set<Marker>();

    public GeoMarkDbContext(DbContextOptions<GeoMarkDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.HasDefaultSchema("geo_marker");

        modelBuilder.Entity<Layer>()
            .HasMany<Marker>()
            .WithOne()
            .HasForeignKey(x => x.LayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}