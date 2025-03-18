using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for registering LaTeX error detection services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds LaTeX error detection services to the service collection
    /// </summary>
    public static IServiceCollection AddLaTeXErrorDetection(this IServiceCollection services)
    {
        // Register the error analyzer
        services.AddSingleton<iErrorAnalyser, ErrorAnalyser>();
        
        // Register the error presenter
        services.AddSingleton<iErrorPresenter>(provider => {
            var errorAnalyser = provider.GetRequiredService<iErrorAnalyser>();
            return new ErrorPresenter(errorAnalyser);
        });
        
        // Register the error checking facade
        // services.AddSingleton<iErrorCheckingFacade>(provider => {
        //     var errorAnalyser = provider.GetRequiredService<iErrorAnalyser>();
        //     var errorPresenter = provider.GetRequiredService<iErrorPresenter>();
        //     return new ErrorCheckingFacade(errorAnalyser, errorPresenter);
        // });
        
        return services;
    }
}