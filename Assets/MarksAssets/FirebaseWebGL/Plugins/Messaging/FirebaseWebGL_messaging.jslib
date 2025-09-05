mergeInto(LibraryManager.library, {
    getMessaging_FirebaseWebGL_messaging: function(appName) {
        return Module["FirebaseWebGL"]._messaging.getMessaging(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName));
    },
    isSupported_FirebaseWebGL_messaging: function(callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._messaging.isSupported(callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
    deleteOrGetToken_FirebaseWebGL_messaging: function(func, appName, options, callback, taskCompletionSource) {
        Module["FirebaseWebGL"]._messaging.deleteOrGetToken(Module.FirebaseWebGL._util.UTF8ToString(func), appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
    },
    onMessage_FirebaseWebGL_appCheck: function(appName, onNextId, onNext) {
        return Module["FirebaseWebGL"]._messaging.onMessage(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), Module.FirebaseWebGL._util.UTF8ToString(onNextId), onNext);
    },
    requestPermission_FirebaseWebGL_messaging: function(callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._messaging.requestPermission(callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
    }
});