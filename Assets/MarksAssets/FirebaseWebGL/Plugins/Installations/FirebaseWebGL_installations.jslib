mergeInto(LibraryManager.library, {
    getInstallations_FirebaseWebGL_installations: function(appName) {
        return Module["FirebaseWebGL"]._installations.getInstallations(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName));
    },
    onIdChange_FirebaseWebGL_installations: function(appName, onNextId, onNext) {
        return Module["FirebaseWebGL"]._installations.onIdChange(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), Module.FirebaseWebGL._util.UTF8ToString(onNextId), onNext);
    },
    runInstallationsFunc_FirebaseWebGL_installations: function(func, appName, forceRefresh, callback, taskCompletionSource) {
        Module["FirebaseWebGL"]._installations.runInstallationsFunc(Module.FirebaseWebGL._util.UTF8ToString(func), appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), forceRefresh === 0 ? false : true, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
    }
});