// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AriesCloudAPI.ResponseHandling;
using AriesCloudAPI.Services;
using AriesCloudAPI.Stores;
using AriesCloudAPI.Validation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using AriesCloudAPI;
using AriesCloudAPI.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Builder extension methods for registering additional services 
    /// </summary>
    public static class AriesCloudAPIBuilderExtensionsAdditional
    {
        /// <summary>
        /// Adds the extension grant validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddExtensionGrantValidator<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IExtensionGrantValidator
        {
            builder.Services.AddTransient<IExtensionGrantValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds a redirect URI validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddRedirectUriValidator<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IRedirectUriValidator
        {
            builder.Services.AddTransient<IRedirectUriValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds a an "AppAuth" (OAuth 2.0 for Native Apps) compliant redirect URI validator (does strict validation but also allows http://127.0.0.1 with random port)
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddAppAuthRedirectUriValidator(this IAriesCloudAPIBuilder builder)
        {
            return builder.AddRedirectUriValidator<StrictRedirectUriValidatorAppAuth>();
        }

        /// <summary>
        /// Adds the resource owner validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddResourceOwnerValidator<T>(this IAriesCloudAPIBuilder builder)
           where T : class, IResourceOwnerPasswordValidator
        {
            builder.Services.AddTransient<IResourceOwnerPasswordValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds the profile service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddProfileService<T>(this IAriesCloudAPIBuilder builder)
           where T : class, IProfileService
        {
            builder.Services.AddTransient<IProfileService, T>();

            return builder;
        }
        
        /// <summary>
        /// Adds a resource validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddResourceValidator<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IResourceValidator
        {
            builder.Services.AddTransient<IResourceValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds a scope parser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddScopeParser<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IScopeParser
        {
            builder.Services.AddTransient<IScopeParser, T>();

            return builder;
        }

        /// <summary>
        /// Adds a client store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddClientStore<T>(this IAriesCloudAPIBuilder builder)
           where T : class, IClientStore
        {
            builder.Services.TryAddTransient(typeof(T));
            builder.Services.AddTransient<IClientStore, ValidatingClientStore<T>>();

            return builder;
        }

        /// <summary>
        /// Adds a resource store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddResourceStore<T>(this IAriesCloudAPIBuilder builder)
           where T : class, IResourceStore
        {
            builder.Services.AddTransient<IResourceStore, T>();

            return builder;
        }

        /// <summary>
        /// Adds a device flow store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        public static IAriesCloudAPIBuilder AddDeviceFlowStore<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IDeviceFlowStore
        {
            builder.Services.AddTransient<IDeviceFlowStore, T>();

            return builder;
        }

        /// <summary>
        /// Adds a persisted grant store.
        /// </summary>
        /// <typeparam name="T">The type of the concrete grant store that is registered in DI.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns>The builder.</returns>
        public static IAriesCloudAPIBuilder AddPersistedGrantStore<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IPersistedGrantStore
        {
            builder.Services.AddTransient<IPersistedGrantStore, T>();

            return builder;
        }

        /// <summary>
        /// Adds a CORS policy service.
        /// </summary>
        /// <typeparam name="T">The type of the concrete scope store class that is registered in DI.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddCorsPolicyService<T>(this IAriesCloudAPIBuilder builder)
            where T : class, ICorsPolicyService
        {
            builder.Services.AddTransient<ICorsPolicyService, T>();
            return builder;
        }

        /// <summary>
        /// Adds a CORS policy service cache.
        /// </summary>
        /// <typeparam name="T">The type of the concrete CORS policy service that is registered in DI.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddCorsPolicyCache<T>(this IAriesCloudAPIBuilder builder)
            where T : class, ICorsPolicyService
        {
            builder.Services.TryAddTransient(typeof(T));
            builder.Services.AddTransient<ICorsPolicyService, CachingCorsPolicyService<T>>();
            return builder;
        }

        /// <summary>
        /// Adds the secret parser.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddSecretParser<T>(this IAriesCloudAPIBuilder builder)
            where T : class, ISecretParser
        {
            builder.Services.AddTransient<ISecretParser, T>();

            return builder;
        }

        /// <summary>
        /// Adds the secret validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddSecretValidator<T>(this IAriesCloudAPIBuilder builder)
            where T : class, ISecretValidator
        {
            builder.Services.AddTransient<ISecretValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds the client store cache.
        /// </summary>
        /// <typeparam name="T">The type of the concrete client store class that is registered in DI.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddClientStoreCache<T>(this IAriesCloudAPIBuilder builder)
            where T : IClientStore
        {
            builder.Services.TryAddTransient(typeof(T));
            builder.Services.AddTransient<ValidatingClientStore<T>>();
            builder.Services.AddTransient<IClientStore, CachingClientStore<ValidatingClientStore<T>>>();

            return builder;
        }

        /// <summary>
        /// Adds the client store cache.
        /// </summary>
        /// <typeparam name="T">The type of the concrete scope store class that is registered in DI.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddResourceStoreCache<T>(this IAriesCloudAPIBuilder builder)
            where T : IResourceStore
        {
            builder.Services.TryAddTransient(typeof(T));
            builder.Services.AddTransient<IResourceStore, CachingResourceStore<T>>();
            return builder;
        }

        /// <summary>
        /// Adds the authorize interaction response generator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddAuthorizeInteractionResponseGenerator<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IAuthorizeInteractionResponseGenerator
        {
            builder.Services.AddTransient<IAuthorizeInteractionResponseGenerator, T>();

            return builder;
        }

        /// <summary>
        /// Adds the custom authorize request validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddCustomAuthorizeRequestValidator<T>(this IAriesCloudAPIBuilder builder)
           where T : class, ICustomAuthorizeRequestValidator
        {
            builder.Services.AddTransient<ICustomAuthorizeRequestValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds the custom authorize request validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddCustomTokenRequestValidator<T>(this IAriesCloudAPIBuilder builder)
           where T : class, ICustomTokenRequestValidator
        {
            builder.Services.AddTransient<ICustomTokenRequestValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds support for client authentication using JWT bearer assertions.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddJwtBearerClientAuthentication(this IAriesCloudAPIBuilder builder)
        {
            builder.Services.TryAddTransient<IReplayCache, DefaultReplayCache>();
            builder.AddSecretParser<JwtBearerClientAssertionSecretParser>();
            builder.AddSecretValidator<PrivateKeyJwtSecretValidator>();

            return builder;
        }

        /// <summary>
        /// Adds a client configuration validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddClientConfigurationValidator<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IClientConfigurationValidator
        {
            builder.Services.AddTransient<IClientConfigurationValidator, T>();

            return builder;
        }

        /// <summary>
        /// Adds the X509 secret validators for mutual TLS.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddMutualTlsSecretValidators(this IAriesCloudAPIBuilder builder)
        {
            builder.AddSecretParser<MutualTlsSecretParser>();
            builder.AddSecretValidator<X509ThumbprintSecretValidator>();
            builder.AddSecretValidator<X509NameSecretValidator>();

            return builder;
        }

        /// <summary>
        /// Adds a custom back-channel logout service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddBackChannelLogoutService<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IBackChannelLogoutService
        {
            builder.Services.AddTransient<IBackChannelLogoutService, T>();

            return builder;
        }

        // todo: check with later previews of ASP.NET Core if this is still required
        /// <summary>
        /// Adds configuration for the HttpClient used for back-channel logout notifications.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configureClient">The configruation callback.</param>
        /// <returns></returns>
        public static IHttpClientBuilder AddBackChannelLogoutHttpClient(this IAriesCloudAPIBuilder builder, Action<HttpClient> configureClient = null)
        {
            const string name = AriesCloudAPIConstants.HttpClients.BackChannelLogoutHttpClient;
            IHttpClientBuilder httpBuilder;

            if (configureClient != null)
            {
                httpBuilder = builder.Services.AddHttpClient(name, configureClient);
            }
            else
            {
                httpBuilder = builder.Services.AddHttpClient(name)
                    .ConfigureHttpClient(client => {
                        client.Timeout = TimeSpan.FromSeconds(AriesCloudAPIConstants.HttpClients.DefaultTimeoutSeconds);
                    });
            }

            builder.Services.AddTransient<IBackChannelLogoutHttpClient>(s =>
            {
                var httpClientFactory = s.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(name);
                var loggerFactory = s.GetRequiredService<ILoggerFactory>();
                
                return new DefaultBackChannelLogoutHttpClient(httpClient, loggerFactory);
            });

            return httpBuilder;
        }


        // todo: check with later previews of ASP.NET Core if this is still required
        /// <summary>
        /// Adds configuration for the HttpClient used for JWT request_uri requests.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configureClient">The configruation callback.</param>
        /// <returns></returns>
        public static IHttpClientBuilder AddJwtRequestUriHttpClient(this IAriesCloudAPIBuilder builder, Action<HttpClient> configureClient = null)
        {
            const string name = AriesCloudAPIConstants.HttpClients.JwtRequestUriHttpClient;
            IHttpClientBuilder httpBuilder;

            if (configureClient != null)
            {
                httpBuilder = builder.Services.AddHttpClient(name, configureClient);
            }
            else
            {
                httpBuilder = builder.Services.AddHttpClient(name)
                    .ConfigureHttpClient(client => {
                        client.Timeout = TimeSpan.FromSeconds(AriesCloudAPIConstants.HttpClients.DefaultTimeoutSeconds);
                    });
            }
            
            builder.Services.AddTransient<IJwtRequestUriHttpClient, DefaultJwtRequestUriHttpClient>(s =>
            {
                var httpClientFactory = s.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(name);
                var loggerFactory = s.GetRequiredService<ILoggerFactory>();
                var options = s.GetRequiredService<AriesCloudAPIOptions>();

                return new DefaultJwtRequestUriHttpClient(httpClient, options, loggerFactory);
            });

            return httpBuilder;
        }

        /// <summary>
        /// Adds a custom authorization request parameter store.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddAuthorizationParametersMessageStore<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IAuthorizationParametersMessageStore
        {
            builder.Services.AddTransient<IAuthorizationParametersMessageStore, T>();

            return builder;
        }

        /// <summary>
        /// Adds a custom user session.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddUserSession<T>(this IAriesCloudAPIBuilder builder)
            where T : class, IUserSession
        {
            // This is added as scoped due to the note regarding the AuthenticateAsync
            // method in the AriesCloudAPI.Services.DefaultUserSession implementation.
            builder.Services.AddScoped<IUserSession, T>();

            return builder;
        }
    }
}