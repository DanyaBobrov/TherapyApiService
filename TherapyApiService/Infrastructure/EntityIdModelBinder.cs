using Microsoft.AspNetCore.Mvc.ModelBinding;
using TherapyApiService.Heplers;
using TherapyApiService.Models;

namespace TherapyApiService.Infrastructure
{
    public class EntityIdModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            string value = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(value))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Incorrect value");
                return Task.CompletedTask;
            }

            try
            {
                var data = value[0..(value.Length - 1)];
                var checkSumSymbol = value[value.Length - 1];
                var payload = Crockford.FromBase32String(data);
                var entityId = new EntityId(payload);
                if (entityId.CheckSum != checkSumSymbol)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Incorrect value");
                    return Task.CompletedTask;
                }

                bindingContext.Result = ModelBindingResult.Success(entityId);
                return Task.CompletedTask;
            }
            catch
            {
                bindingContext.Result = ModelBindingResult.Failed();
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Incorrect value");
                return Task.CompletedTask;
            }
        }
    }
}