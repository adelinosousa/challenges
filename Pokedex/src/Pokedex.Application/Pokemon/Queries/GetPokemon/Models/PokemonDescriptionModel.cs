using System.Text.Json.Serialization;

namespace Pokedex.Application.Pokemon
{
    public class PokemonDescriptionModel
    {
        [JsonPropertyName("flavor_text")]
        public string Text { get; set; }
    }
}
