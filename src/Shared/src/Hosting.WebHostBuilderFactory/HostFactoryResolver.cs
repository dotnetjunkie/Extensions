// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Microsoft.Extensions.Hosting.HostBuilderFactory
{
    internal class HostFactoryResolver
    {
        public static readonly string CreateHostBuilder = nameof(CreateHostBuilder);
        public static readonly string CreateWebHostBuilder = nameof(CreateWebHostBuilder);
        public static readonly string BuildWebHost = nameof(BuildWebHost);

        public static FactoryResolutionResult<TWebhost, TWebhostBuilder, THostBuilder> ResolveHostFactory<TWebhost, TWebhostBuilder, THostBuilder>(Assembly assembly)
        {
            var programType = assembly?.EntryPoint?.DeclaringType;
            if (programType == null)
            {
                return FactoryResolutionResult<TWebhost, TWebhostBuilder, THostBuilder>.NoEntryPoint();
            }

            // We want to give priority to BuildWebHost over CreateWebHostBuilder and CreateHostBuilder for backwards
            // compatibility with existing projects that follow the old pattern.
            var factory = programType.GetTypeInfo().GetDeclaredMethod(BuildWebHost);
            if (IsFactory<TWebhost>(factory))
            {
                return FactoryResolutionResult<TWebhost, TWebhostBuilder, THostBuilder>.FoundBuildWebHost(factory, programType);
            }

            factory = programType.GetTypeInfo().GetDeclaredMethod(CreateWebHostBuilder);
            if (IsFactory<TWebhostBuilder>(factory))
            {
                return FactoryResolutionResult<TWebhost, TWebhostBuilder, THostBuilder>.FoundCreateWebHostBuilder(factory, programType);
            }

            factory = programType.GetTypeInfo().GetDeclaredMethod(CreateHostBuilder);
            if (IsFactory<THostBuilder>(factory))
            {
                return FactoryResolutionResult<TWebhost, TWebhostBuilder, THostBuilder>.FoundCreateHostBuilder(factory, programType);
            }

            return FactoryResolutionResult<TWebhost, TWebhostBuilder, THostBuilder>.NoFactory(programType);
        }

        private static bool IsFactory<TReturn>(MethodInfo factory)
        {
            return factory != null
                && typeof(TReturn).IsAssignableFrom(factory.ReturnType)
                && factory.GetParameters().Length == 1
                && typeof(string[]).Equals(factory.GetParameters()[0].ParameterType);
        }

        public static Func<string[], IServiceProvider> ResolveServicesFactory(Assembly assembly)
        {
            var findResult = ResolveHostFactory<object, object, object>(assembly);
            switch (findResult.ResultKind)
            {
                case FactoryResolutionResultKind.NoEntryPoint:
                case FactoryResolutionResultKind.NoFactory:
                    return null;
                case FactoryResolutionResultKind.BuildWebHost:
                    return args =>
                    {
                        var webHost = findResult.WebHostFactory(args);
                        return GetServiceProvider(webHost);
                    };
                case FactoryResolutionResultKind.CreateWebHostBuilder:
                    return args =>
                    {
                        var webHostBuilder = findResult.WebHostBuilderFactory(args);
                        var webHost = webHostBuilder.GetType().GetMethod("Build").Invoke(webHostBuilder, Array.Empty<object>());
                        return GetServiceProvider(webHost);
                    };
                case FactoryResolutionResultKind.CreateHostBuilder:
                    return args =>
                    {
                        var HostBuilder = findResult.HostBuilderFactory(args);
                        var host = HostBuilder.GetType().GetMethod("Build").Invoke(HostBuilder, Array.Empty<object>());
                        return GetServiceProvider(host);
                    };
                default:
                    throw new InvalidOperationException();
            }
        }

        private static IServiceProvider GetServiceProvider(object host)
        {
            var hostType = host.GetType();
            var servicesProperty = hostType.GetTypeInfo().GetDeclaredProperty("Services");
            return (IServiceProvider)servicesProperty.GetValue(host);
        }
    }
}
