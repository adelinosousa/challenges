using MediatR;
using Pokedex.Application.Common.Enums;
using Pokedex.Application.Common.Interfaces;
using Pokedex.Application.Pokemon;
using System;
using System.Threading.Tasks;

namespace Pokedex.Application.Common.Services
{
    public class PokemonTranslatorService : IPokemonTranslatorService
    {
        private readonly ISender mediator;
        private readonly ITranslateFactory translateFactory;

        public PokemonTranslatorService(ISender mediator, ITranslateFactory translateFactory)
        {
            this.mediator = mediator;
            this.translateFactory = translateFactory;
        }

        public async Task<GetPokemonQueryResponse> GetTranslatePokemon(string pokemonName)
        {
            var pokemon = await mediator.Send(new GetPokemonQuery { Name = pokemonName });

            if (string.IsNullOrWhiteSpace(pokemon.Description))
                return pokemon;

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

            return pokemon;
        }
    }
}
