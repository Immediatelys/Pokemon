using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int id);

        ICollection<Owner> GetOwnerByPokemon(int PokeId);

        ICollection<Pokemon> GetPokemonByOwner(int OwnerId);
        bool OwnwersExist(int OwnerId);

        bool CreateOwner(Owner owner);

        bool Save();
    }
}
