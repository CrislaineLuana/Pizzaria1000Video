using Microsoft.EntityFrameworkCore;
using Pizzaria1000Video.Models;

namespace Pizzaria1000Video.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {      
        }


        public DbSet<PizzaModel> Pizzas { get; set; }
    }
}
