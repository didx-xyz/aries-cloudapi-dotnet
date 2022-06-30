using AriesCloudAPI.Configuration;
using AriesCloudAPI.Core.Interfaces;
using AriesCloudAPI.Core.Options;
using AriesCloudAPI.Services;
using AriesCloudAPI.WebClient.Clients;
using AriesCloudAPI.WebClient.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;  

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Builder extension methods for registering core services
    /// </summary>
    public static class AriesCloudAPIBuilderExtensionsCore
    {
        /// <summary>
        /// Adds the required platform services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddRequiredPlatformServices(this IAriesCloudAPIBuilder builder)
        {
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();            
            builder.Services.AddOptions();
            builder.Services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<AriesCloudAPIOptions>>().Value);
            builder.Services.AddHttpClient();

            return builder;
        }

        /// <summary>
        /// Adds the required web client services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IAriesCloudAPIBuilder AddWebClients(this IAriesCloudAPIBuilder builder)
        {
            //builder.Services.AddSingleton<AriesCloudAPIOptions>(
            //    resolver => {
            //        var o = resolver.GetRequiredService<IOptions<AriesCloudAPIOptions>>().Value;

            //        return new AriesCloudAPIOptions { BaseUri = o.BaseUri};
            //    }
            //);

            builder.Services.AddScoped<TenantAdminServiceAgent>();
            builder.Services.AddScoped<ConnectionServiceAgent>();

            //TODO: move 
            builder.Services.AddScoped<IEntityKeyResolver, EntityKeyResolver>();

            return builder;
        }

        /// <summary>
        /// Adds the default cookie handlers and corresponding configuration
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddCookieAuthentication(this IAriesCloudAPIBuilder builder)
        //{
        //    builder.Services.AddAuthentication(AriesCloudAPIConstants.DefaultCookieAuthenticationScheme)
        //        .AddCookie(AriesCloudAPIConstants.DefaultCookieAuthenticationScheme)
        //        .AddCookie(AriesCloudAPIConstants.ExternalCookieAuthenticationScheme);

        //    builder.Services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureInternalCookieOptions>();
        //    builder.Services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureInternalCookieOptions>();
        //    builder.Services.AddTransientDecorator<IAuthenticationService, AriesCloudAPIAuthenticationService>();
        //    builder.Services.AddTransientDecorator<IAuthenticationHandlerProvider, FederatedSignoutAuthenticationHandlerProvider>();

        //    return builder;
        //}

        /// <summary>
        /// Adds the default endpoints.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddDefaultEndpoints(this IAriesCloudAPIBuilder builder)
        //{
        //    builder.Services.AddTransient<IEndpointRouter, EndpointRouter>();

        //    builder.AddEndpoint<AuthorizeCallbackEndpoint>(EndpointNames.Authorize, ProtocolRoutePaths.AuthorizeCallback.EnsureLeadingSlash());
        //    builder.AddEndpoint<AuthorizeEndpoint>(EndpointNames.Authorize, ProtocolRoutePaths.Authorize.EnsureLeadingSlash());
        //    builder.AddEndpoint<CheckSessionEndpoint>(EndpointNames.CheckSession, ProtocolRoutePaths.CheckSession.EnsureLeadingSlash());
        //    builder.AddEndpoint<DeviceAuthorizationEndpoint>(EndpointNames.DeviceAuthorization, ProtocolRoutePaths.DeviceAuthorization.EnsureLeadingSlash());
        //    builder.AddEndpoint<DiscoveryKeyEndpoint>(EndpointNames.Discovery, ProtocolRoutePaths.DiscoveryWebKeys.EnsureLeadingSlash());
        //    builder.AddEndpoint<DiscoveryEndpoint>(EndpointNames.Discovery, ProtocolRoutePaths.DiscoveryConfiguration.EnsureLeadingSlash());
        //    builder.AddEndpoint<EndSessionCallbackEndpoint>(EndpointNames.EndSession, ProtocolRoutePaths.EndSessionCallback.EnsureLeadingSlash());
        //    builder.AddEndpoint<EndSessionEndpoint>(EndpointNames.EndSession, ProtocolRoutePaths.EndSession.EnsureLeadingSlash());
        //    builder.AddEndpoint<IntrospectionEndpoint>(EndpointNames.Introspection, ProtocolRoutePaths.Introspection.EnsureLeadingSlash());
        //    builder.AddEndpoint<TokenRevocationEndpoint>(EndpointNames.Revocation, ProtocolRoutePaths.Revocation.EnsureLeadingSlash());
        //    builder.AddEndpoint<TokenEndpoint>(EndpointNames.Token, ProtocolRoutePaths.Token.EnsureLeadingSlash());
        //    builder.AddEndpoint<UserInfoEndpoint>(EndpointNames.UserInfo, ProtocolRoutePaths.UserInfo.EnsureLeadingSlash());

        //    return builder;
        //}

        /// <summary>
        /// Adds the endpoint.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddEndpoint<T>(this IAriesCloudAPIBuilder builder, string name, PathString path)
        //    where T : class, IEndpointHandler
        //{
        //    builder.Services.AddTransient<T>();
        //    builder.Services.AddSingleton(new AriesCloudAPI.Hosting.Endpoint(name, path, typeof(T)));

        //    return builder;
        //}

        /// <summary>
        /// Adds the core services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddCoreServices(this IAriesCloudAPIBuilder builder)
        //{
        //    builder.Services.AddTransient<ISecretsListParser, SecretParser>();
        //    builder.Services.AddTransient<ISecretsListValidator, SecretValidator>();
        //    builder.Services.AddTransient<ExtensionGrantValidator>();
        //    builder.Services.AddTransient<BearerTokenUsageValidator>();
        //    builder.Services.AddTransient<JwtRequestValidator>();

        //    builder.Services.AddTransient<ReturnUrlParser>();
        //    builder.Services.AddTransient<AriesCloudAPITools>();

        //    builder.Services.AddTransient<IReturnUrlParser, OidcReturnUrlParser>();
        //    builder.Services.AddScoped<IUserSession, DefaultUserSession>();
        //    builder.Services.AddTransient(typeof(MessageCookie<>));

        //    builder.Services.AddCors();
        //    builder.Services.AddTransientDecorator<ICorsPolicyProvider, CorsPolicyProvider>();

        //    return builder;
        //}

        /// <summary>
        /// Adds the pluggable services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddPluggableServices(this IAriesCloudAPIBuilder builder)
        //{
        //    builder.Services.TryAddTransient<IPersistedGrantService, DefaultPersistedGrantService>();
        //    builder.Services.TryAddTransient<IKeyMaterialService, DefaultKeyMaterialService>();
        //    builder.Services.TryAddTransient<ITokenService, DefaultTokenService>();
        //    builder.Services.TryAddTransient<ITokenCreationService, DefaultTokenCreationService>();
        //    builder.Services.TryAddTransient<IClaimsService, DefaultClaimsService>();
        //    builder.Services.TryAddTransient<IRefreshTokenService, DefaultRefreshTokenService>();
        //    builder.Services.TryAddTransient<IDeviceFlowCodeService, DefaultDeviceFlowCodeService>();
        //    builder.Services.TryAddTransient<IConsentService, DefaultConsentService>();
        //    builder.Services.TryAddTransient<ICorsPolicyService, DefaultCorsPolicyService>();
        //    builder.Services.TryAddTransient<IProfileService, DefaultProfileService>();
        //    builder.Services.TryAddTransient<IConsentMessageStore, ConsentMessageStore>();
        //    builder.Services.TryAddTransient<IMessageStore<LogoutMessage>, ProtectedDataMessageStore<LogoutMessage>>();
        //    builder.Services.TryAddTransient<IMessageStore<LogoutNotificationContext>, ProtectedDataMessageStore<LogoutNotificationContext>>();
        //    builder.Services.TryAddTransient<IMessageStore<ErrorMessage>, ProtectedDataMessageStore<ErrorMessage>>();
        //    builder.Services.TryAddTransient<IAriesCloudAPIInteractionService, DefaultAriesCloudAPIInteractionService>();
        //    builder.Services.TryAddTransient<IDeviceFlowInteractionService, DefaultDeviceFlowInteractionService>();
        //    builder.Services.TryAddTransient<IAuthorizationCodeStore, DefaultAuthorizationCodeStore>();
        //    builder.Services.TryAddTransient<IRefreshTokenStore, DefaultRefreshTokenStore>();
        //    builder.Services.TryAddTransient<IReferenceTokenStore, DefaultReferenceTokenStore>();
        //    builder.Services.TryAddTransient<IUserConsentStore, DefaultUserConsentStore>();
        //    builder.Services.TryAddTransient<IHandleGenerationService, DefaultHandleGenerationService>();
        //    builder.Services.TryAddTransient<IPersistentGrantSerializer, PersistentGrantSerializer>();
        //    builder.Services.TryAddTransient<IEventService, DefaultEventService>();
        //    builder.Services.TryAddTransient<IEventSink, DefaultEventSink>();
        //    builder.Services.TryAddTransient<IUserCodeService, DefaultUserCodeService>();
        //    builder.Services.TryAddTransient<IUserCodeGenerator, NumericUserCodeGenerator>();
        //    builder.Services.TryAddTransient<ILogoutNotificationService, LogoutNotificationService>();
        //    builder.Services.TryAddTransient<IBackChannelLogoutService, DefaultBackChannelLogoutService>();
        //    builder.Services.TryAddTransient<IResourceValidator, DefaultResourceValidator>();
        //    builder.Services.TryAddTransient<IScopeParser, DefaultScopeParser>();

        //    builder.AddJwtRequestUriHttpClient();
        //    builder.AddBackChannelLogoutHttpClient();

        //    builder.Services.AddTransient<IClientSecretValidator, ClientSecretValidator>();
        //    builder.Services.AddTransient<IApiSecretValidator, ApiSecretValidator>();

        //    builder.Services.TryAddTransient<IDeviceFlowThrottlingService, DistributedDeviceFlowThrottlingService>();
        //    builder.Services.AddDistributedMemoryCache();

        //    return builder;
        //}

        /// <summary>
        /// Adds the validators.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddValidators(this IAriesCloudAPIBuilder builder)
        //{
        //    // core
        //    builder.Services.TryAddTransient<IEndSessionRequestValidator, EndSessionRequestValidator>();
        //    builder.Services.TryAddTransient<ITokenRevocationRequestValidator, TokenRevocationRequestValidator>();
        //    builder.Services.TryAddTransient<IAuthorizeRequestValidator, AuthorizeRequestValidator>();
        //    builder.Services.TryAddTransient<ITokenRequestValidator, TokenRequestValidator>();
        //    builder.Services.TryAddTransient<IRedirectUriValidator, StrictRedirectUriValidator>();
        //    builder.Services.TryAddTransient<ITokenValidator, TokenValidator>();
        //    builder.Services.TryAddTransient<IIntrospectionRequestValidator, IntrospectionRequestValidator>();
        //    builder.Services.TryAddTransient<IResourceOwnerPasswordValidator, NotSupportedResourceOwnerPasswordValidator>();
        //    builder.Services.TryAddTransient<ICustomTokenRequestValidator, DefaultCustomTokenRequestValidator>();
        //    builder.Services.TryAddTransient<IUserInfoRequestValidator, UserInfoRequestValidator>();
        //    builder.Services.TryAddTransient<IClientConfigurationValidator, DefaultClientConfigurationValidator>();
        //    builder.Services.TryAddTransient<IDeviceAuthorizationRequestValidator, DeviceAuthorizationRequestValidator>();
        //    builder.Services.TryAddTransient<IDeviceCodeValidator, DeviceCodeValidator>();

        //    // optional
        //    builder.Services.TryAddTransient<ICustomTokenValidator, DefaultCustomTokenValidator>();
        //    builder.Services.TryAddTransient<ICustomAuthorizeRequestValidator, DefaultCustomAuthorizeRequestValidator>();

        //    return builder;
        //}

        /// <summary>
        /// Adds the response generators.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddResponseGenerators(this IAriesCloudAPIBuilder builder)
        //{
        //    builder.Services.TryAddTransient<ITokenResponseGenerator, TokenResponseGenerator>();
        //    builder.Services.TryAddTransient<IUserInfoResponseGenerator, UserInfoResponseGenerator>();
        //    builder.Services.TryAddTransient<IIntrospectionResponseGenerator, IntrospectionResponseGenerator>();
        //    builder.Services.TryAddTransient<IAuthorizeInteractionResponseGenerator, AuthorizeInteractionResponseGenerator>();
        //    builder.Services.TryAddTransient<IAuthorizeResponseGenerator, AuthorizeResponseGenerator>();
        //    builder.Services.TryAddTransient<IDiscoveryResponseGenerator, DiscoveryResponseGenerator>();
        //    builder.Services.TryAddTransient<ITokenRevocationResponseGenerator, TokenRevocationResponseGenerator>();
        //    builder.Services.TryAddTransient<IDeviceAuthorizationResponseGenerator, DeviceAuthorizationResponseGenerator>();

        //    return builder;
        //}

        /// <summary>
        /// Adds the default secret parsers.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddDefaultSecretParsers(this IAriesCloudAPIBuilder builder)
        //{
        //    builder.Services.AddTransient<ISecretParser, BasicAuthenticationSecretParser>();
        //    builder.Services.AddTransient<ISecretParser, PostBodySecretParser>();

        //    return builder;
        //}

        /// <summary>
        /// Adds the default secret validators.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        //public static IAriesCloudAPIBuilder AddDefaultSecretValidators(this IAriesCloudAPIBuilder builder)
        //{
        //    builder.Services.AddTransient<ISecretValidator, HashedSharedSecretValidator>();

        //    return builder;
        //}

        //internal static void AddTransientDecorator<TService, TImplementation>(this IServiceCollection services)
        //    where TService : class
        //    where TImplementation : class, TService
        //{
        //    services.AddDecorator<TService>();
        //    services.AddTransient<TService, TImplementation>();
        //}

        //internal static void AddDecorator<TService>(this IServiceCollection services)
        //{
        //    var registration = services.LastOrDefault(x => x.ServiceType == typeof(TService));
        //    if (registration == null)
        //    {
        //        throw new InvalidOperationException("Service type: " + typeof(TService).Name + " not registered.");
        //    }
        //    if (services.Any(x => x.ServiceType == typeof(Decorator<TService>)))
        //    {
        //        throw new InvalidOperationException("Decorator already registered for type: " + typeof(TService).Name + ".");
        //    }

        //    services.Remove(registration);

        //    if (registration.ImplementationInstance != null)
        //    {
        //        var type = registration.ImplementationInstance.GetType();
        //        var innerType = typeof(Decorator<,>).MakeGenericType(typeof(TService), type);
        //        services.Add(new ServiceDescriptor(typeof(Decorator<TService>), innerType, ServiceLifetime.Transient));
        //        services.Add(new ServiceDescriptor(type, registration.ImplementationInstance));
        //    }
        //    else if (registration.ImplementationFactory != null)
        //    {
        //        services.Add(new ServiceDescriptor(typeof(Decorator<TService>), provider =>
        //        {
        //            return new DisposableDecorator<TService>((TService)registration.ImplementationFactory(provider));
        //        }, registration.Lifetime));
        //    }
        //    else
        //    {
        //        var type = registration.ImplementationType;
        //        var innerType = typeof(Decorator<,>).MakeGenericType(typeof(TService), registration.ImplementationType);
        //        services.Add(new ServiceDescriptor(typeof(Decorator<TService>), innerType, ServiceLifetime.Transient));
        //        services.Add(new ServiceDescriptor(type, type, registration.Lifetime));
        //    }
        //}
    }
}