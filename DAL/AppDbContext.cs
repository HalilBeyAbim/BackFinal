using BackFinalEdu.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackFinalEdu.DAL
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Slider> Sliders { get; set; }
    }
    
    
}
