﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Application.Common.Factories.Translate;
using Pokedex.Application.Common.Interfaces;
using Pokedex.Application.Common.Services;
using System.Reflection;

namespace Pokedex.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddHttpClient();

            services.AddScoped<ITranslateFactory, TranslateFactory>();
            services.AddScoped<IPokemonTranslatorService, PokemonTranslatorService>();

            return services;
        }
    }
}
