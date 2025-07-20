﻿using NSubstitute;
using NUnit.Framework;
using SlackNet.Blocks;
using SlackNet.Events;
using SlackNet.Handlers;
using SlackNet.Interaction;
using SlackNet.Interaction.Experimental;

namespace SlackNet.Tests.Configuration;

public abstract class FactorySlackHandlerConfigurationWithDependencyResolverTests<TConfig, TDependencyResolver> : FactorySlackHandlerConfigurationTests<TConfig> where TConfig : FactorySlackServiceConfigurationWithDependencyResolver<TConfig, TDependencyResolver>
{
    [Test]
    public void UseHttpFactory()
    {
        UseService<IHttp, TestHttp>(
            c => c.UseHttp(r => new TestHttp()),
            s => s.GetHttp());
    }

    [Test]
    public void UseJsonSettingsFactory()
    {
        UseService<SlackJsonSettings, TestJsonSettings>(
            c => c.UseJsonSettings(r => new TestJsonSettings()),
            s => s.GetJsonSettings());
    }

    [Test]
    public void UseFactoryResolverFactory()
    {
        UseService<ISlackTypeResolver, TestTypeResolver>(
            c => c.UseTypeResolver(r => new TestTypeResolver()),
            s => s.GetTypeResolver());
    }

    [Test]
    public void UseUrlBuilderFactory()
    {
        UseService<ISlackUrlBuilder, TestUrlBuilder>(
            c => c.UseUrlBuilder(r => new TestUrlBuilder()),
            s => s.GetUrlBuilder());
    }

    [Test]
    public void UseLoggerFactory()
    {
        UseService<ILogger, TestLogger>(
            c => c.UseLogger(r => new TestLogger()),
            s => s.GetLogger());
    }

    [Test]
    public void UseWebSocketFactoryFactory()
    {
        UseService<IWebSocketFactory, TestWebSocketFactory>(
            c => c.UseWebSocketFactory(r => new TestWebSocketFactory()),
            s => s.GetWebSocketFactory());
    }

    [Test]
    public void UseRequestListenerFactory()
    {
        var sut = Configure(c => c.UseRequestListener(r => new TestRequestListener(ResolveDependency<InstanceTracker>(r))));
        var instanceTracker = InstanceTracker;

        RequestListenersShouldBeCreatedOnceOnEnumeration(sut, instanceTracker);
    }

    [Test]
    public void UseHandlerFactoryFactory()
    {
        UseService<ISlackHandlerFactory, TestHandlerFactory>(
            c => c.UseHandlerFactory(r => new TestHandlerFactory()),
            s => s.GetHandlerFactory());
    }

    [Test]
    public void UseApiClientFactory()
    {
        UseService<ISlackApiClient, TestApiClient>(
            c => c.UseApiClient(r => new TestApiClient()),
            s => s.GetApiClient());
    }

    [Test]
    public void UseSocketModeClientFactory()
    {
        UseService<ISlackSocketModeClient, TestSocketModeClient>(
            c => c.UseSocketModeClient(r => new TestSocketModeClient()),
            s => s.GetSocketModeClient());
    }

    [Test]
    public void ReplaceEventHandling_WithFactory()
    {
        ReplaceRequestHandling<IEventHandler, TestEventHandler>(
            c => c.ReplaceEventHandling(r => new TestEventHandler(ResolveDependency<InstanceTracker>(r))),
            (hf, ctx) => hf.CreateEventHandler(ctx));
    }

    [Test]
    public void ReplaceBlockActionHandling_WithFactory()
    {
        ReplaceRequestHandling<IAsyncBlockActionHandler, TestAsyncBlockActionHandler>(
            c => c.ReplaceBlockActionHandling(r => new TestAsyncBlockActionHandler(ResolveDependency<InstanceTracker>(r))),
            (hf, ctx) => hf.CreateBlockActionHandler(ctx));
    }

    [Test]
    public void ReplaceBlockOptionProvider_WithFactory()
    {
        ReplaceRequestHandling<IBlockOptionProvider, TestBlockOptionProvider>(
            c => c.ReplaceBlockOptionProviding(r => new TestBlockOptionProvider(ResolveDependency<InstanceTracker>(r))),
            (hf, ctx) => hf.CreateBlockOptionProvider(ctx));
    }

    [Test]
    public void ReplaceMessageShortcutHandling_WithFactory()
    {
        ReplaceRequestHandling<IAsyncMessageShortcutHandler, TestAsyncMessageShortcutHandler>(
            c => c.ReplaceMessageShortcutHandling(r => new TestAsyncMessageShortcutHandler(ResolveDependency<InstanceTracker>(r))),
            (hf, ctx) => hf.CreateMessageShortcutHandler(ctx));
    }

    [Test]
    public void ReplaceGlobalShortcutHandling_WithFactory()
    {
        ReplaceRequestHandling<IAsyncGlobalShortcutHandler, TestAsyncGlobalShortcutHandler>(
            c => c.ReplaceGlobalShortcutHandling(r => new TestAsyncGlobalShortcutHandler(ResolveDependency<InstanceTracker>(r))),
            (hf, ctx) => hf.CreateGlobalShortcutHandler(ctx));
    }

    [Test]
    public void ReplaceViewSubmissionHandling_WithFactory()
    {
        ReplaceRequestHandling<IAsyncViewSubmissionHandler, TestAsyncViewSubmissionHandler>(
            c => c.ReplaceViewSubmissionHandling(r => new TestAsyncViewSubmissionHandler(ResolveDependency<InstanceTracker>(r))),
            (hf, ctx) => hf.CreateViewSubmissionHandler(ctx));
    }

    [Test]
    public void ReplaceSlashCommandHandling_WithFactory()
    {
        ReplaceRequestHandling<IAsyncSlashCommandHandler, TestAsyncSlashCommandHandler>(
            c => c.ReplaceSlashCommandHandling(r => new TestAsyncSlashCommandHandler(ResolveDependency<InstanceTracker>(r))),
            (hf, ctx) => hf.CreateSlashCommandHandler(ctx));
    }

    [Test]
    public void ReplaceLegacyInteractiveMessageHandling_WithFactory()
    {
        ReplaceRequestHandling<IInteractiveMessageHandler, TestInteractiveMessageHandler>(
            c => c.ReplaceLegacyInteractiveMessageHandling(r => new TestInteractiveMessageHandler(ResolveDependency<InstanceTracker>(r))),
            (hf, ctx) => hf.CreateLegacyInteractiveMessageHandler(ctx));
    }

    [Test]
    public void ReplaceLegacyOptionProviding_WithFactory()
    {
        ReplaceRequestHandling<IOptionProvider, TestOptionProvider>(
            c => c.ReplaceLegacyOptionProviding(r => new TestOptionProvider(ResolveDependency<InstanceTracker>(r))),
            (hf, ctx) => hf.CreateLegacyOptionProvider(ctx));
    }

    [Test]
    public void ReplaceLegacyDialogSubmissionHandling_WithFactory()
    {
        ReplaceRequestHandling<IDialogSubmissionHandler, TestDialogSubmissionHandler>(
            c => c.ReplaceLegacyDialogSubmissionHandling(r => new TestDialogSubmissionHandler(ResolveDependency<InstanceTracker>(r))),
            (hf, ctx) => hf.CreateLegacyDialogSubmissionHandler(ctx));
    }

    [Test]
    public void RegisterEventHandlerFactory()
    {
        RegisterEventHandler(
            registerGenericHandler: c => c.RegisterEventHandler(r => new TestEventHandler(ResolveDependency<InstanceTracker>(r))),
            registerTypedHandler: c => c.RegisterEventHandler(r => new TestEventHandler<Hello>(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterBlockActionHandlerFactory()
    {
        RegisterBlockActionHandler(
            registerKeyedButtonHandler: (c, key) => c.RegisterBlockActionHandler(key, r => new TestBlockActionHandler<ButtonAction>(ResolveDependency<InstanceTracker>(r))),
            registerButtonHandler: c => c.RegisterBlockActionHandler(r => new TestBlockActionHandler<DatePickerAction>(ResolveDependency<InstanceTracker>(r))),
            registerGenericHandler: c => c.RegisterBlockActionHandler(r => new TestBlockActionHandler(ResolveDependency<InstanceTracker>(r))),
            registerKeyedAsyncDatePickerHandler: (c, key) => c.RegisterAsyncBlockActionHandler(key, r => new TestAsyncBlockActionHandler<ButtonAction>(ResolveDependency<InstanceTracker>(r))),
            registerAsyncDatePickerHandler: c => c.RegisterAsyncBlockActionHandler(r => new TestAsyncBlockActionHandler<DatePickerAction>(ResolveDependency<InstanceTracker>(r))),
            registerGenericAsyncHandler: c => c.RegisterAsyncBlockActionHandler(r => new TestAsyncBlockActionHandler(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterBlockOptionProviderFactory()
    {
        RegisterBlockOptionProvider(
            registerProvider: (c, action) => c.RegisterBlockOptionProvider(action, r => new TestBlockOptionProvider(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterMessageShortcutHandlerFactory()
    {
        RegisterMessageShortcutHandler(
            registerHandler: c => c.RegisterMessageShortcutHandler(r => new TestMessageShortcutHandler(ResolveDependency<InstanceTracker>(r))),
            registerAsyncHandler: c => c.RegisterAsyncMessageShortcutHandler(r => new TestAsyncMessageShortcutHandler(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterMessageShortcutHandlerFactory_Keyed()
    {
        RegisterMessageShortcutHandler_Keyed(
            registerHandler: (c, key) => c.RegisterMessageShortcutHandler(key, r => new TestMessageShortcutHandler(ResolveDependency<InstanceTracker>(r))),
            registerAsyncHandler: (c, key) => c.RegisterAsyncMessageShortcutHandler(key, r => new TestAsyncMessageShortcutHandler(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterGlobalShortcutHandlerFactory()
    {
        RegisterGlobalShortcutHandler(
            registerHandler: c => c.RegisterGlobalShortcutHandler(r => new TestGlobalShortcutHandler(ResolveDependency<InstanceTracker>(r))),
            registerAsyncHandler: c => c.RegisterAsyncGlobalShortcutHandler(r => new TestAsyncGlobalShortcutHandler(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterGlobalShortcutHandlerFactory_Keyed()
    {
        RegisterGlobalShortcutHandler_Keyed(
            registerHandler: (c, key) => c.RegisterGlobalShortcutHandler(key, r => new TestGlobalShortcutHandler(ResolveDependency<InstanceTracker>(r))),
            registerAsyncHandler: (c, key) => c.RegisterAsyncGlobalShortcutHandler(key, r => new TestAsyncGlobalShortcutHandler(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterViewSubmissionHandlerFactory_HandleSubmissions()
    {
        RegisterViewSubmissionHandler_HandleSubmissions(
            registerHandler: (c, key) => c.RegisterViewSubmissionHandler(key, r => new TestViewSubmissionHandler(ResolveDependency<InstanceTracker>(r))),
            registerAsyncHandler: (c, key) => c.RegisterAsyncViewSubmissionHandler(key, r => new TestAsyncViewSubmissionHandler(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterViewSubmissionHandlerFactory_HandleCloses()
    {
        RegisterViewSubmissionHandler_HandleCloses(
            registerHandler: (c, key) => c.RegisterViewSubmissionHandler(key, r => new TestViewSubmissionHandler(ResolveDependency<InstanceTracker>(r))),
            registerAsyncHandler: (c, key) => c.RegisterAsyncViewSubmissionHandler(key, r => new TestAsyncViewSubmissionHandler(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterSlashCommandHandlerFactory_InvalidCommandName_Throws()
    {
        RegisterSlashCommandHandlerWithInvalidCommandName(
            c => c.RegisterSlashCommandHandler("foo", r => Substitute.For<ISlashCommandHandler>()),
            c => c.RegisterAsyncSlashCommandHandler("foo", r => Substitute.For<IAsyncSlashCommandHandler>()));
    }

    [Test]
    public void RegisterSlashCommandHandlerFactory()
    {
        RegisterSlashCommandHandler(
            registerHandler: (c, command) => c.RegisterSlashCommandHandler(command, r => new TestSlashCommandHandler(ResolveDependency<InstanceTracker>(r))),
            registerAsyncHandler: (c, command) => c.RegisterAsyncSlashCommandHandler(command, r => new TestAsyncSlashCommandHandler(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterInteractiveMessageHandlerFactory()
    {
        RegisterInteractiveMessageHandler(
            registerHandler: (c, action) => c.RegisterInteractiveMessageHandler(action, r => new TestInteractiveMessageHandler(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterOptionProviderFactory()
    {
        RegisterOptionProvider(
            registerHandler: (c, action) => c.RegisterOptionProvider(action, r => new TestOptionProvider(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterDialogSubmissionHandlerFactory_HandleSubmissions()
    {
        RegisterDialogSubmissionHandler_HandleSubmissions(
            registerHandler: (c, key) => c.RegisterDialogSubmissionHandler(key, r => new TestDialogSubmissionHandler(ResolveDependency<InstanceTracker>(r))));
    }

    [Test]
    public void RegisterDialogSubmissionHandlerFactory_HandleCancellations()
    {
        RegisterDialogSubmissionHandler_HandleCancellations(
            registerHandler: (c, key) => c.RegisterDialogSubmissionHandler(key, r => new TestDialogSubmissionHandler(ResolveDependency<InstanceTracker>(r))));
    }

    protected abstract T ResolveDependency<T>(TDependencyResolver resolver) where T : class;
}