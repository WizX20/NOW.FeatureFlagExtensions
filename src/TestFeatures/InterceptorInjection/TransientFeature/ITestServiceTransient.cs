﻿namespace TestFeatures.InterceptorInjection.TransientFeature
{
    public interface ITestServiceTransient
    {
        string Name { get; }

        string GetTest();
    }
}