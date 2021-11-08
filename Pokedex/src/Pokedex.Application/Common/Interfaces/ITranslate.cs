using System.Threading.Tasks;

namespace Pokedex.Application.Common.Interfaces
{
    public interface ITranslate
    {
        Task<string> Translate(string value);
    }
}
