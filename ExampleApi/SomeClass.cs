using Microsoft.FeatureManagement;

namespace WebApplication2;

public class SomeClass
{
    private readonly IFeatureManager _featureManager;

    public SomeClass(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }
    
    public async Task DoSomething()
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.SomeFeature))
        {
            // Выполняем функционал, закрытый фича флагом
        }

        // Основной функционал функции
    }
}