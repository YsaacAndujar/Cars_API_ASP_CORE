using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace CarsApi.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;
            var valueProvider = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProvider == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            try
            {
                var deserializedObject = JsonConvert.DeserializeObject<T>(valueProvider.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(deserializedObject);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(modelName, "Invalid type for List<int>");
            }
            return Task.CompletedTask;
        }
    }
}
