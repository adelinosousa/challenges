using Pokedex.Application.Common.Enums;

namespace Pokedex.Application.Common.Interfaces
{
    public interface ITranslateFactory
    {
        ITranslate GetTranslator(TranslateOption translateOption);
    }
}
