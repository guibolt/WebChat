using Microsoft.EntityFrameworkCore;
using WebChat.Models;

namespace WebChat.Db
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) =>  Database.EnsureCreated(); 
        public DbSet<RespostaChat> RespostaChat { get; set; }
    }
}
