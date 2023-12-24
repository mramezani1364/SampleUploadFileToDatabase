
using Microsoft.EntityFrameworkCore;
using UploadFileToDatabase.Aggregate;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
       
    }
    public DbSet<User> User { get; set; }
}

