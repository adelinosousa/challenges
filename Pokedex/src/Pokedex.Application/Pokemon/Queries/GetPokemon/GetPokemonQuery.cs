using MediatR;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Pokedex.Application.Pokemon
{
    public class GetPokemonQuery : IRequest<GetPokemonQueryResponse>
    {
        public string Name { get; set; }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetPokemonQuery, GetPokemonQueryResponse>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public GetUsersQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<GetPokemonQueryResponse> Handle(GetPokemonQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var pokeapiResponse = await Fetch<PokemonModel>($"https://pokeapi.co/api/v2/pokemon/{request.Name}", cancellationToken);
                if (pokeapiResponse != null && !string.IsNullOrWhiteSpace(pokeapiResponse.Species?.Url))
                {
                    pokeapiResponse.Species = await Fetch<PokemonSpeciesModel>(pokeapiResponse.Species.Url, cancellationToken);
                    if (pokeapiResponse.Species != null)
                    {
                        PokemonDescriptionModel description = null;

                        if (pokeapiResponse.Species.TextEntries != null && pokeapiResponse.Species.TextEntries.Length > 0)
                        {
                            var rnd = new Random();
                            description = pokeapiResponse.Species.TextEntries[rnd.Next(pokeapiResponse.Species.TextEntries.Length)];
                        }

                        return new GetPokemonQueryResponse
                        {
                            Name = pokeapiResponse.Name,
                            Description = description?.Text,
                            Habitat = pokeapiResponse.Species.Habitat?.Name,
                            IsLegendary = pokeapiResponse.Species.IsLegendary
                        };
                    }
                }
            }

            return new GetPokemonQueryResponse();
        }

        private async Task<T> Fetch<T>(string url, CancellationToken cancellationToken) where T : class
        {
            var httpclient = httpClientFactory.CreateClient();

            using var response = await httpclient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode && response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
            }

            return null;
        }
    }
}
