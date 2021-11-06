using FluentAssertions;
using NUnit.Framework;
using Pokedex.Application.Pokemon;
using System.Threading.Tasks;
using NSubstitute;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Text.Json;

namespace Pokedex.Application.UnitTests.Pokemon.Queries.GetPokemon
{
    public class GetPokemonQueryTests : TestBase
    {
        private GetUsersQueryHandler queryHandler;

        [SetUp]
        public void Setup()
        {
            queryHandler = new GetUsersQueryHandler(httpClientFactory);
        }

        [Test]
        public async Task ShouldNotReturnNull()
        {
            var query = new GetPokemonQuery();

            var result = await queryHandler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldReturnPokemon()
        {
            var query = new GetPokemonQuery { Name = "mewtwo" };

            httpClientFactory.CreateClient().Returns((e) => GetMockHttpClient(HttpStatusCode.OK, new PokemonModel
            {
                Name = query.Name,
                Species = new PokemonSpeciesModel
                {
                    Url = "https://mock.uri",
                }
            }), (e) => GetMockHttpClient(HttpStatusCode.OK, new PokemonSpeciesModel
            {
                IsLegendary = true
            }));

            var result = await queryHandler.Handle(query, CancellationToken.None);

            result.Name.Should().NotBeEmpty();
            result.IsLegendary.Should().BeTrue();
        }
    }

    
}
