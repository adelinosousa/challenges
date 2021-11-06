using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Pokemon;
using System.Threading.Tasks;

namespace Pokedex.Controllers
{
    public class PokemonController : ApiControllerBase
    {
        [HttpGet, Route("{pokemonName}")]
        public async Task<IActionResult> Get(string pokemonName)
        {
            return Ok(await Mediator.Send(new GetPokemonQuery { Name = pokemonName }));
        }
    }
}
