using System.Collections.Generic;
using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;
using UnityEngine;

namespace _App_v2.Scripts._Core.Firebase.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly List<IAnalyticsProvider> _analyticsProviders = new();

        void IAnalyticsService.AddProvider(IAnalyticsProvider provider)
        {
            if (_analyticsProviders.Contains(provider))
            {
                Debug.LogWarning($"[AnalyticsService] Provider {provider?.ProviderName} already added");
                return;
            }

            _analyticsProviders.Add(provider);
            Debug.Log($"[AnalyticsService] Provider added: {provider?.ProviderName}. Total providers: {_analyticsProviders.Count}");
        }

        void IAnalyticsService.Initialize() => InitializeProviders();

        void IAnalyticsService.SetUserProperty(IUserProperty property)
        {
            if (property == null)
            {
                Debug.LogError("User property is null");
                return;
            }
            
            foreach (var provider in _analyticsProviders)
            {
                if (provider == null || !provider.IsInitialized)
                    continue;
                
                provider.SetUserProperty(property);
            }
        }

        void IAnalyticsService.SendEvent(IAnalyticsEvent analyticsEvent)
        {
            if (analyticsEvent == null)
            {
                Debug.LogError("[AnalyticsService] Analytics event is null");
                return;
            }

            Debug.Log($"[AnalyticsService] SendEvent called: {analyticsEvent.Name}. Provider count: {_analyticsProviders.Count}");

            foreach (var provider in _analyticsProviders)
            {
                if (provider == null)
                {
                    Debug.LogWarning("[AnalyticsService] Skipping null provider");
                    continue;
                }

                if (!provider.IsInitialized)
                {
                    Debug.LogWarning($"[AnalyticsService] Provider {provider.ProviderName} is not initialized, skipping");
                    continue;
                }

                Debug.Log($"[AnalyticsService] Sending event to provider: {provider.ProviderName}");
                provider.SendEvent(analyticsEvent);
            }
        }

        void IAnalyticsService.SendWindowEvent(string windowName)
        {
            foreach (var provider in _analyticsProviders)
            {
                if (provider == null || !provider.IsInitialized || provider.AnalyticsHelper == null)
                    continue;
                
                provider.SendEvent(provider.AnalyticsHelper.GetWindowEvent(windowName));
            }
        }

        private void InitializeProviders()
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.Initialize();
            }
        }
    }
}