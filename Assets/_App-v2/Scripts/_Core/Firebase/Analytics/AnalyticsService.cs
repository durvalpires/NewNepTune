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
                return;
            
            _analyticsProviders.Add(provider);
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
                Debug.LogError("Analytics event is null");
                return;
            }
            
            foreach (var provider in _analyticsProviders)
            {
                if (provider == null || !provider.IsInitialized)
                    continue;
                
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