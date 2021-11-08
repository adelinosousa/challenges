using Pokedex.Application.Pokemon;
using System.Threading.Tasks;

namespace Pokedex.Application.Common.Interfaces
{
    public interface IPokemonTranslatorService
    {
        Task<GetPokemonQueryResponse> GetTranslatePokemon(string pokemonName);
    }
}
