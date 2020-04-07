using BulkDataProcessorExamples.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkDataProcessorExamples.EntityFramework
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public BlogDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
