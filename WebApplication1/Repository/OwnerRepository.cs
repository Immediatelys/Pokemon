using Swashbuckle.AspNetCore.SwaggerGen;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;
        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerByPokemon(int PokeId)
        {
            return _context.PokemonOwners.Where(p => p.Pokemon.Id == PokeId).Select(o => o.Owner).ToList(); 
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.OrderBy(o => o.Id).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int OwnerId)
        {
            return _context.PokemonOwners.Where(o => o.Owner.Id == OwnerId).Select(p => p.Pokemon).ToList();
        }

        public bool OwnwersExist(int OwnerId)
        {
            return _context.Owners.Any(o => o.Id == OwnerId);
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);    
            return Save();
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
