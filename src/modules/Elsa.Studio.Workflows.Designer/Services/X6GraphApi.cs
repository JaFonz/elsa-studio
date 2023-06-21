using System.Text.Json;
using System.Text.Json.Serialization;
using Elsa.Api.Client.Activities;
using Elsa.Api.Client.Converters;
using Elsa.Studio.Workflows.Designer.Models;
using Microsoft.JSInterop;

namespace Elsa.Studio.Workflows.Designer.Services;

public class X6GraphApi
{
    private readonly IJSObjectReference _module;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _containerId;

    public X6GraphApi(IJSObjectReference module, IServiceProvider serviceProvider, string containerId)
    {
        _module = module;
        _serviceProvider = serviceProvider;
        _containerId = containerId;
    }
    
    public async Task<JsonElement> ReadGraphAsync() => await InvokeAsync(module => module.InvokeAsync<JsonElement>("readGraph", _containerId));
    public async Task DisposeGraphAsync() => await InvokeAsync(module => module.InvokeVoidAsync("disposeGraph", _containerId));
    public async Task SetGridColorAsync(string color) => await InvokeAsync(module => module.InvokeVoidAsync("setGridColor", _containerId, color));
    public async Task AddActivityNodeAsync(X6Node node) => await InvokeAsync(module => module.InvokeVoidAsync("addActivityNode", _containerId, node));
    public async Task LoadGraphAsync(X6Graph graph) => await InvokeAsync(module => module.InvokeVoidAsync("loadGraph", _containerId, graph));
    public async Task ZoomToFitAsync() => await InvokeAsync(module => module.InvokeVoidAsync("zoomToFit", _containerId));
    public async Task CenterContentAsync() => await InvokeAsync(module => module.InvokeVoidAsync("centerContent", _containerId));
    
    public async Task UpdateActivityAsync(Activity activity)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        serializerOptions.Converters.Add(new ActivityJsonConverterFactory(_serviceProvider));
        serializerOptions.Converters.Add(new ExpressionJsonConverterFactory());
        serializerOptions.Converters.Add(new JsonStringEnumConverter());

        var activityElement = JsonSerializer.SerializeToElement(activity, serializerOptions);
        await InvokeAsync(module => module.InvokeVoidAsync("updateActivity", _containerId, activityElement));
    }

    private async Task InvokeAsync(Func<IJSObjectReference, ValueTask> func) => await func(_module);
    private async Task<T> InvokeAsync<T>(Func<IJSObjectReference, ValueTask<T>> func) => await func(_module);
}