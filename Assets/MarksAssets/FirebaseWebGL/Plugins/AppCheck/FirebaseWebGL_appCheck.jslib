mergeInto(LibraryManager.library, {
    initializeAppCheck_FirebaseWebGL_appCheck: function(appName, options) {
        return Module["FirebaseWebGL"]._appCheck.initializeAppCheck(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
    },
    onTokenChanged_FirebaseWebGL_appCheck: function(appName, isTokenAutoRefreshEnabled, type, siteKey, onNextId, onNext, onErrorId, onError) {
		return Module["FirebaseWebGL"]._appCheck.onTokenChanged(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), isTokenAutoRefreshEnabled === 0 ? false : true,  Module.FirebaseWebGL._util.UTF8ToString(type), Module.FirebaseWebGL._util.UTF8ToString(siteKey), Module.FirebaseWebGL._util.UTF8ToString(onNextId), onNext, Module.FirebaseWebGL._util.UTF8ToString(onErrorId), onError === 0 ? null : onError);
    },
    getLimitedUseToken_FirebaseWebGL_appCheck: function(appName, isTokenAutoRefreshEnabled, type, siteKey, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._appCheck.getLimitedUseToken(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), isTokenAutoRefreshEnabled === 0 ? false : true,  Module.FirebaseWebGL._util.UTF8ToString(type), Module.FirebaseWebGL._util.UTF8ToString(siteKey), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
    },
    getToken_FirebaseWebGL_appCheck: function(appName, isTokenAutoRefreshEnabled, type, siteKey, forceRefresh, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._appCheck.getToken(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), isTokenAutoRefreshEnabled === 0 ? false : true,  Module.FirebaseWebGL._util.UTF8ToString(type), Module.FirebaseWebGL._util.UTF8ToString(siteKey), forceRefresh === 0 ? false : true ,callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
    },
    setTokenAutoRefreshEnabled_FirebaseWebGL_appCheck: function(appName, isTokenAutoRefreshEnabled, type, siteKey, isTokenAutoRefreshEnabledNew) {
        return Module["FirebaseWebGL"]._appCheck.setTokenAutoRefreshEnabled(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), isTokenAutoRefreshEnabled === 0 ? false : true,  Module.FirebaseWebGL._util.UTF8ToString(type), Module.FirebaseWebGL._util.UTF8ToString(siteKey), isTokenAutoRefreshEnabledNew === 0 ? false : true);
    },
    customProvider_FirebaseWebGL_appCheck: function(isCompletedId, isCompleted, getResultId, getResult) {
        return Module["FirebaseWebGL"]._appCheck.customProvider(Module.FirebaseWebGL._util.UTF8ToString(isCompletedId), isCompleted, Module.FirebaseWebGL._util.UTF8ToString(getResultId), getResult);
    },
    CustomProvider_clear_FirebaseWebGL_appCheck: function(id) {
        Module["FirebaseWebGL"]._appCheck.CustomProvider_clear(id);
    }
});