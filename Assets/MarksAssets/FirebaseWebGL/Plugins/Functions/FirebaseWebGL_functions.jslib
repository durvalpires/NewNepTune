mergeInto(LibraryManager.library, {
	setOrGetProperty_FirebaseWebGL_functions: function(property, appName, regionOrCustomDomain, value) {
		return Module["FirebaseWebGL"]._functions.setOrGetProperty(Module.FirebaseWebGL._util.UTF8ToString(property), appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), regionOrCustomDomain === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(regionOrCustomDomain), value === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(value));
	},
	getFunctions_FirebaseWebGL_functions: function(appName, regionOrCustomDomain) {
		return Module["FirebaseWebGL"]._functions.getFunctions(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), regionOrCustomDomain === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(regionOrCustomDomain));
	},
	connectFunctionsEmulator_FirebaseWebGL_functions: function(appName, regionOrCustomDomain, host, port) {
		return Module["FirebaseWebGL"]._functions.connectFunctionsEmulator(Module.FirebaseWebGL._util.UTF8ToString(appName), regionOrCustomDomain === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(regionOrCustomDomain), Module.FirebaseWebGL._util.UTF8ToString(host), port);
	},
	httpsCallable_FirebaseWebGL_functions: function(appName, regionOrCustomDomain, func, nameOrUrl, options) {
		return Module["FirebaseWebGL"]._functions.httpsCallable(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), regionOrCustomDomain === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(regionOrCustomDomain), Module.FirebaseWebGL._util.UTF8ToString(func), Module.FirebaseWebGL._util.UTF8ToString(nameOrUrl), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	runCallable_FirebaseWebGL_functions: function(id, requestData, callback, taskCompletionSource, errorCallback, errorCallbackId) {
		Module["FirebaseWebGL"]._functions.runCallable(id, requestData === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(requestData)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), errorCallback, Module.FirebaseWebGL._util.UTF8ToString(errorCallbackId));
	},
});