using MFC_VoxMe_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MFC_VoxMe_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }


        public DbSet<MovingData> MovingDatas { get; set; }
        public DbSet<Resources> Resources { get; set; }
        public DbSet<Materials> Materials { get; set; }

    }
}
