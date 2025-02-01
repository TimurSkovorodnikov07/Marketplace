using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace server_app.Presentation.Filters.ModelBinders;

public class TagsBinder : IModelBinder
{
    public TagsBinder(IModelBinder defaultModelBinder, ILogger<TagsBinder> logger)
    {
        _defaultModelBinder = defaultModelBinder;
        _logger = logger;
    }
    private readonly IModelBinder _defaultModelBinder;
    private readonly ILogger<TagsBinder> _logger;
    
    
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var result = bindingContext.ValueProvider.GetValue("Tags");
        
        if(result == ValueProviderResult.None) return _defaultModelBinder.BindModelAsync(bindingContext);
        
        if (string.IsNullOrEmpty(result.FirstValue))
        {
            bindingContext.Result = ModelBindingResult.Failed();
            _logger.LogTrace("Tags is empty");
            
            return Task.CompletedTask;
        }
        
        var tagsObject = JsonSerializer.Deserialize(result.FirstValue, typeof(List<string>));
        if (tagsObject is List<string> tagsList)
        {   
            bindingContext.Result = ModelBindingResult.Success(tagsList); 
            return Task.CompletedTask;
        }
        
        bindingContext.Result = ModelBindingResult.Failed();
        _logger.LogTrace("This object not List<string>");
        
        return Task.CompletedTask;
    }
}