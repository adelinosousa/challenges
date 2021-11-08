using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Common.Enums;
using Pokedex.Application.Common.Interfaces;
using Pokedex.Application.Pokemon;
using System;
using System.Threading.Tasks;

namespace Pokedex.Controllers
{
    public class PokemonController : ApiControllerBase
    {
        private readonly IPokemonTranslatorService pokemonTranslator;

        public PokemonController(IPokemonTranslatorService pokemonTranslator)
        {
            this.pokemonTranslator = pokemonTranslator;
        }

        [HttpGet, Route("{pokemonName}")]
        public async Task<IActionResult> Get(string pokemonName) => Ok(await Mediator.Send(new GetPokemonQuery { Name = pokemonName }));

        [HttpGet, Route("{pokemonName}/translated")]
        public async Task<IActionResult> Translated(string pokemonName) => Ok(await pokemonTranslator.GetTranslatePokemon(pokemonName));
    }
}
