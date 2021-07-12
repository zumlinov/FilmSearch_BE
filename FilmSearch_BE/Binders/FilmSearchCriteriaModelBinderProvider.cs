using FilmSearchClasses.Dtos.SearchParams;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FilmSearch_BE.Binders
{
    public class FilmSearchCriteriaModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinder binder = new FilmSearchCriteriaModelBinder();

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(SearchParam[]) ? binder : null;
        }
    }
}
