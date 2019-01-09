// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MockHostTypes;
using System;
using Xunit;

namespace Microsoft.Extensions.Hosting.HostBuilderFactory.Tests
{
    public class HostFactoryResolverTests
    {
        [Fact]
        public void CanFindHostBuilder_CreateHostBuilderPattern()
        {
            // Arrange & Act
            var resolverResult = HostFactoryResolver.ResolveHostFactory<IWebHost, IWebHostBuilder, IHostBuilder>(typeof(CreateHostBuilderPatternTestSite.Program).Assembly);

            // Assert
            Assert.Equal(FactoryResolutionResultKind.CreateHostBuilder, resolverResult.ResultKind);
            Assert.Null(resolverResult.HostBuilderFactory);
            Assert.Null(resolverResult.WebHostFactory);
            Assert.NotNull(resolverResult.HostBuilderFactory);
            Assert.IsAssignableFrom<IHostBuilder>(resolverResult.HostBuilderFactory(Array.Empty<string>()));
        }
        /*
        [Fact]
        public void CanFindHost_CreateHostBuilderPattern()
        {
            // Arrange & Act
            var resolverResult = HostFactoryResolver.ResolveHostFactory<IHost, IHostBuilder>(typeof(IStartupInjectionAssemblyName.Startup).Assembly);

            // Assert
            Assert.Equal(FactoryResolutionResultKind.Success, resolverResult.ResultKind);
            Assert.NotNull(resolverResult.HostBuilderFactory);
            Assert.NotNull(resolverResult.HostFactory);
        }

        [Fact]
        public void CanNotFindHostBuilder_BuildHostPattern()
        {
            // Arrange & Act
            var resolverResult = HostFactoryResolver.ResolveHostBuilderFactory<IHost, IHostBuilder>(typeof(BuildHostPatternTestSite.Startup).Assembly);

            // Assert
            Assert.Equal(FactoryResolutionResultKind.NoCreateHostBuilder, resolverResult.ResultKind);
            Assert.Null(resolverResult.HostBuilderFactory);
            Assert.Null(resolverResult.HostFactory);
        }

        [Fact]
        public void CanNotFindHostBuilder_CreateHostBuilderIncorrectSignature()
        {
            // Arrange & Act
            var resolverResult = HostFactoryResolver.ResolveHostBuilderFactory<IHost, IHostBuilder>(typeof(CreateHostBuilderInvalidSignature.Startup).Assembly);

            // Assert
            Assert.Equal(FactoryResolutionResultKind.NoCreateHostBuilder, resolverResult.ResultKind);
            Assert.Null(resolverResult.HostBuilderFactory);
            Assert.Null(resolverResult.HostFactory);
        }

        [Fact]
        public void CanNotFindHost_BuildHostIncorrectSignature()
        {
            // Arrange & Act
            var resolverResult = HostFactoryResolver.ResolveHostFactory<IHost, IHostBuilder>(typeof(BuildHostInvalidSignature.Startup).Assembly);

            // Assert
            Assert.Equal(FactoryResolutionResultKind.NoBuildHost, resolverResult.ResultKind);
            Assert.Null(resolverResult.HostBuilderFactory);
            Assert.Null(resolverResult.HostFactory);
        }

        [Fact]
        public void CanFindHost_BuildHostPattern()
        {
            // Arrange & Act
            var resolverResult = HostFactoryResolver.ResolveHostFactory<IHost, IHostBuilder>(typeof(BuildHostPatternTestSite.Startup).Assembly);

            // Assert
            Assert.Equal(FactoryResolutionResultKind.Success, resolverResult.ResultKind);
            Assert.Null(resolverResult.HostBuilderFactory);
            Assert.NotNull(resolverResult.HostFactory);
        }*/
    }
}
