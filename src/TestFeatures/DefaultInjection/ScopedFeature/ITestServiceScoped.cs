﻿namespace TestFeatures.DefaultInjection.ScopedFeature
{
    public interface ITestServiceScoped
    {
        string Name { get; }

        string GetTest();
    }
}