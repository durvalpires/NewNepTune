using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _App_v2.Scripts._Core.Firebase.Crashlytics
{
    public class CrashlyticsService : IDisposable
    {
        public async void Initialize()
        {
            await UniTask.SwitchToMainThread();
            #if !UNITY_WEBGL
            global::Firebase.Crashlytics.Crashlytics.IsCrashlyticsCollectionEnabled = true;
            global::Firebase.Crashlytics.Crashlytics.ReportUncaughtExceptionsAsFatal = true;
            #endif

            
            Application.logMessageReceived += HandleLogMessage;
        }

        public async void Dispose()
        {
            await UniTask.SwitchToMainThread();
            Application.logMessageReceived -= HandleLogMessage;
        }

        private async void HandleLogMessage(string condition, string stackTrace, LogType type)
        {
            if (type != LogType.Exception && type != LogType.Error)
                return;
            
            await UniTask.SwitchToMainThread();
            #if !UNITY_WEBGL
            global::Firebase.Crashlytics.Crashlytics.Log($"{condition}\n{stackTrace}");
            #endif

        }
    }
}