
using Core.ResultModels;

namespace Infrastracture.Caching
{
    public interface ICacher
    {
        Task<int?> GetCode(string key);
        Task<ResultModel> SetCode(string key, int value);
    }
}