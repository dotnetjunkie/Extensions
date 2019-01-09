// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Microsoft.Extensions.Hosting.HostBuilderFactory
{
    internal class FactoryResolutionResult<TWebHost,TWebHostBuilder, THostBuilder>
    {
        public FactoryResolutionResultKind ResultKind { get; set; }
        public Type ProgramType { get; set; }
        public Func<string[], TWebHost> WebHostFactory { get; set; }
        public Func<string[], TWebHostBuilder> WebHostBuilderFactory { get; set; }
        public Func<string[], THostBuilder> HostBuilderFactory { get; set; }

        internal static FactoryResolutionResult<TWebHost, TWebHostBuilder, THostBuilder> NoEntryPoint() =>
            new FactoryResolutionResult<TWebHost, TWebHostBuilder, THostBuilder>
            {
                ResultKind = FactoryResolutionResultKind.NoEntryPoint
            };

        internal static FactoryResolutionResult<TWebHost, TWebHostBuilder, THostBuilder> NoFactory(Type programType) =>
            new FactoryResolutionResult<TWebHost, TWebHostBuilder, THostBuilder>
            {
                ProgramType = programType,
                ResultKind = FactoryResolutionResultKind.NoFactory
            };

        internal static FactoryResolutionResult<TWebHost, TWebHostBuilder, THostBuilder> FoundBuildWebHost(MethodInfo factory, Type programType)
            => new FactoryResolutionResult<TWebHost, TWebHostBuilder, THostBuilder>
        {
            ProgramType = programType,
            ResultKind = FactoryResolutionResultKind.BuildWebHost,
            WebHostFactory = args => (TWebHost)factory.Invoke(null, new object[] { args })
        };

        internal static FactoryResolutionResult<TWebHost, TWebHostBuilder, THostBuilder> FoundCreateWebHostBuilder(MethodInfo factory, Type programType)
            => new FactoryResolutionResult<TWebHost, TWebHostBuilder, THostBuilder>
        {
            ProgramType = programType,
            ResultKind = FactoryResolutionResultKind.CreateWebHostBuilder,
            WebHostBuilderFactory = args => (TWebHostBuilder)factory.Invoke(null, new object[] { args }),
        };

        internal static FactoryResolutionResult<TWebHost, TWebHostBuilder, THostBuilder> FoundCreateHostBuilder(MethodInfo factory, Type programType)
            => new FactoryResolutionResult<TWebHost, TWebHostBuilder, THostBuilder>
        {
            ProgramType = programType,
            ResultKind = FactoryResolutionResultKind.CreateHostBuilder,
            HostBuilderFactory = args => (THostBuilder)factory.Invoke(null, new object[] { args })
        };
    }
}
