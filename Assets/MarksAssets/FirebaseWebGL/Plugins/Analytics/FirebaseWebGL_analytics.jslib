mergeInto(LibraryManager.library, {
	getAnalytics_FirebaseWebGL_analytics: function(appName) {
		return Module["FirebaseWebGL"]._analytics.getAnalytics(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName));
	},
	initializeAnalytics_FirebaseWebGL_analytics: function(appName, options) {
		return Module["FirebaseWebGL"]._analytics.initializeAnalytics(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	getGoogleAnalyticsClientId_FirebaseWebGL_analytics: function(appName, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._analytics.getGoogleAnalyticsClientId(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	logEvent_FirebaseWebGL_analytics: function(appName, eventName, eventParams, options) {
		return Module["FirebaseWebGL"]._analytics.logEvent(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), Module.FirebaseWebGL._util.UTF8ToString(eventName), eventParams === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(eventParams)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	isSupported_FirebaseWebGL_analytics: function(callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._analytics.isSupported(callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	setAnalyticsCollectionEnabled_FirebaseWebGL_analytics: function(appName, enabled) {
		return Module["FirebaseWebGL"]._analytics.setAnalyticsCollectionEnabled(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), enabled === 0 ? false : true);
	},
	setUserId_FirebaseWebGL_analytics: function(appName, id, options) {
		return Module["FirebaseWebGL"]._analytics.setUserId(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), Module.FirebaseWebGL._util.UTF8ToString(id), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	setUserProperties_FirebaseWebGL_analytics: function(appName, properties, options) {
		return Module["FirebaseWebGL"]._analytics.setUserProperties(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(properties)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	set_FirebaseWebGL_analytics: function(func, json) {
		return Module["FirebaseWebGL"]._analytics.set(Module.FirebaseWebGL._util.UTF8ToString(func), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(json)));
	}
});