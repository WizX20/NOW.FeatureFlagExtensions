﻿namespace FeatureTestApplication.TestFeatures.InterceptorInjection.SingletonFeature
{
    public interface ITestServiceSingleton
    {
        string Name { get; }

        string GetTest();
    }
}