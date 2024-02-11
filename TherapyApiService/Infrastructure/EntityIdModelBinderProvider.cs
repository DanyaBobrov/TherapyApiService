using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using TherapyApiService.Models;

namespace TherapyApiService.Infrastructure
{
    public class EntityIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(EntityId))
                return new BinderTypeModelBinder(typeof(EntityIdModelBinder));

            return default;
        }
    }
}