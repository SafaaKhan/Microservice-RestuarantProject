using Mango.Web.Models;
using Mango.Web.Models.Dtos;

namespace Mango.Web.Service.IService
{
    public interface IBaseService:IDisposable
    {
        ResponseDto responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
