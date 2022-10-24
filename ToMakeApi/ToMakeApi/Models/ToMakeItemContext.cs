using Microsoft.EntityFrameworkCore;

namespace ToMakeApi.Models
{
    public class ToMakeItemContext : DbContext
    {
        public ToMakeItemContext(DbContextOptions<ToMakeItemContext> options) : base(options)
        {

        }

        protected ToMakeItemContext()
        {

        }

        public DbSet<ToMakeItem> ToMakeItems { get; set; } = null!;
    }
}
