using Pokedex.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokedex.Application.Common.Factories.Translate
{
    public class FunTranslation : ITranslate
    {
        private readonly string translationType;
        private readonly IHttpClientFactory httpClientFactory;

        public FunTranslation(string translationType, IHttpClientFactory httpClientFactory)
        {
            this.translationType = translationType;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<string> Translate(string value)
        {
            var httpclient = httpClientFactory.CreateClient();

            using var response = await httpclient.PostAsync(
                $"https://api.funtranslations.com/translate/{translationType}.json", 
                new FormUrlEncodedContent(new Dictionary<string, string>{
                    { "text", value }
                })
            );

            if (response.IsSuccessStatusCode && response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var content = await response.Content.ReadAsStringAsync();
                var funTranslationResult = JsonSerializer.Deserialize<FunTranslationModel>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

                if (funTranslationResult.Success?.Total > 0 
                    && !string.IsNullOrWhiteSpace(funTranslationResult.Contents?.Translated))
                {
                    return funTranslationResult.Contents.Translated;
                }
            }

            return null;
        }
    }
}
