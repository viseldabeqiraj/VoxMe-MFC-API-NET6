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


        //public DbSet<MovingData> MovingDatas { get; set; }


    }
}
