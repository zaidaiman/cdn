using Microsoft.EntityFrameworkCore;

public abstract class BaseContext : DbContext
{
    public BaseContext(DbContextOptions options) : base(options) { }
    public virtual DbSet<User> User => Set<User>();
}
