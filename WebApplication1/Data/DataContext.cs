using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
               
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Pokemon> Pokemons { get; set; }   

        public DbSet<Country>  Countries { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<PokemonCategory> PokemonCategories { get; set; }

        public DbSet<PokemonOwner> PokemonOwners { get; set;}

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //liên kết giữa 2 bảng
            modelBuilder.Entity<PokemonCategory>()
                    .HasKey(pc => new {pc.CategoryId, pc.PokemonId});
            //tạo mối quan hệ 1 nhiều 
            modelBuilder.Entity<PokemonCategory>()
                     .HasOne(p => p.Pokemon)
                     .WithMany(pc => pc.PokemonCategories)
                     .HasForeignKey(c => c.CategoryId);
            modelBuilder.Entity<PokemonCategory>()
                     .HasOne(p => p.Category)
                     .WithMany(pc => pc.PokemonCategories)
                     .HasForeignKey(c => c.PokemonId);
            
            modelBuilder.Entity<PokemonOwner>()
                    .HasKey(pc => new { pc.OwnerId, pc.PokemonId });
            modelBuilder.Entity<PokemonOwner>()
                     .HasOne(p => p.Pokemon)
                     .WithMany(pc => pc.PokemonOwners)
                     .HasForeignKey(c => c.OwnerId);
            modelBuilder.Entity<PokemonOwner>()
                     .HasOne(p => p.Owner)
                     .WithMany(pc => pc.PokemonOwners)
                     .HasForeignKey(c => c.PokemonId);



        }
    }
}
