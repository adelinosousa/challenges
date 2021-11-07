using Pokedex.Application.Common.Interfaces;
using System.Net.Http;
using System;
using Pokedex.Application.Common.Enums;

namespace Pokedex.Application.Common.Factories.Translate
{
    public class TranslateFactory : ITranslateFactory
    {
        private readonly IHttpClientFactory httpClientFactory;

        public TranslateFactory(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public ITranslate GetTranslator(TranslateOption translateOption) => translateOption switch
        {
            TranslateOption.Shakespeare => new FunTranslation("shakespeare", httpClientFactory),
            TranslateOption.Yoda => new FunTranslation("yoda", httpClientFactory),
            _ => throw new ArgumentException($"Invalid translate option {translateOption}")
        };
    }
}
