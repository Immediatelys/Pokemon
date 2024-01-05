﻿using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();

        Pokemon GetPokemon(int id);

        Pokemon GetPokemon(string name);

        decimal GetPokemonRating(int id);

        bool PokemonExists(int pokeId);

        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        bool Save();
    }
}
