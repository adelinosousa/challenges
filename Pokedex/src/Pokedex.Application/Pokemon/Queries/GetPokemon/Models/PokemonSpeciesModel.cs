using System.Text.Json.Serialization;

namespace Pokedex.Application.Pokemon
{
    public class PokemonSpeciesModel
    {
        public string Url { get; set; }

        [JsonPropertyName("flavor_text_entries")]
        public PokemonDescriptionModel[] TextEntries { get; set; }

        public PokemonHabitatModel Habitat { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
    }
}
