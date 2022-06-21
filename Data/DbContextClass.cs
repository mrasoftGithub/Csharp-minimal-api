namespace MinimalAPI.Data;

public class DbContextClass : DbContext
{
    public DbContextClass(DbContextOptions<DbContextClass> options) : base(options) { }

    public DbSet<EIGENAAR> EIGENAAR => Set<EIGENAAR>();
}
