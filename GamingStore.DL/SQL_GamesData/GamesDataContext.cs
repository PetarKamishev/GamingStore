using GamingStore.GamingStore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GamingStore.GamingStore.DL.GamesData
{
    public class GamesDataContext : DbContext
    {


        public GamesDataContext()
        {
            
        }

        public GamesDataContext(DbContextOptions<GamesDataContext> options): base(options)
        {
            
        }
        public DbSet<Games> Games { get; set; }
    }
}
