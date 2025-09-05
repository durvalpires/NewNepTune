mergeInto(LibraryManager.library, {
	setOrGetProperty_FirebaseWebGL_remoteConfig: function(appName, property, value) {
		return Module["FirebaseWebGL"]._remoteConfig.setOrGetProperty(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), Module.FirebaseWebGL._util.UTF8ToString(property), value === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(value)));
	},
	getRemoteConfig_FirebaseWebGL_remoteConfig: function(appName) {
		return Module["FirebaseWebGL"]._remoteConfig.getRemoteConfig(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName));
	},
	funcReturnsPromise_FirebaseWebGL_remoteConfig: function(appName, func, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._remoteConfig.funcReturnsPromise(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), Module.FirebaseWebGL._util.UTF8ToString(func), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	func_FirebaseWebGL_remoteConfig: function(appName, func, key) {
		return Module["FirebaseWebGL"]._remoteConfig.func(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), Module.FirebaseWebGL._util.UTF8ToString(func), key === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(key));
	},
	isSupported_FirebaseWebGL_remoteConfig: function(callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._remoteConfig.isSupported(callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	setLogLevel_FirebaseWebGL_remoteConfig: function(appName, logLevel) {
		return Module["FirebaseWebGL"]._remoteConfig.setLogLevel(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), Module.FirebaseWebGL._util.UTF8ToString(logLevel));
	}
});