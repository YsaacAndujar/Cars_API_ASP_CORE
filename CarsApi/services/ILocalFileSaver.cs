using CarsApi.Validations;

namespace CarsApi.services
{
    public interface ILocalFileSaver
    {
        Task<String> SaveAsync(IFormFile formFile, FileEnumType fileEnumType);
        Task RemoveAsync(string route, FileEnumType fileEnumType);
    }
}
 