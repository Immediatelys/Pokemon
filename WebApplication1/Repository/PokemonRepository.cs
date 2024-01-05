using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class PokemeonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemeonRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault(); ;
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where( p => p.Pokemon.Id == pokeId);
            if(review.Count() <= 0)
                return 0;

            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }


        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemons.Any(p => p.Id == pokeId);
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            // tao thuc the giua pokemon va owner
            var pokemonOwnerEntity = _context.Owners.Where(a => a.Id == ownerId).FirstOrDefault();
            // tao thuc the giua pokemon va category
            var category = _context.Categories.Where(a => a.Id ==categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon
            };

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory
            {
                Category = category,
                Pokemon = pokemon
            };

            _context.Add(pokemonCategory);

            _context.Add(pokemon);

            return Save();
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }
    }
}
