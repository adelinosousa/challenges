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
        private readonly ITranslateFactory translateFactory;

        public PokemonController(ITranslateFactory translateFactory)
        {
            this.translateFactory = translateFactory;
        }

        [HttpGet, Route("{pokemonName}")]
        public async Task<IActionResult> Get(string pokemonName) => Ok(await Mediator.Send(new GetPokemonQuery { Name = pokemonName }));

        [HttpGet, Route("{pokemonName}/translated")]
        public async Task<IActionResult> Translated(string pokemonName)
        {
            var pokemon = await Mediator.Send(new GetPokemonQuery { Name = pokemonName });

            if (string.IsNullOrWhiteSpace(pokemon.Description))
                return Ok(pokemon);

            var translationType = TranslateOption.Shakespeare;

            if (pokemon.IsLegendary
                || (!string.IsNullOrWhiteSpace(pokemon.Habitat) && pokemon.Habitat.Equals("cave", StringComparison.InvariantCultureIgnoreCase)))
            {
                translationType = TranslateOption.Yoda;
            }

            var translator = translateFactory.GetTranslator(translationType);
            var translationResult = await translator.Translate(pokemon.Description);

            if (translationResult != null)
                pokemon.Description = translationResult;

            return Ok(pokemon);
        }
    }
}
