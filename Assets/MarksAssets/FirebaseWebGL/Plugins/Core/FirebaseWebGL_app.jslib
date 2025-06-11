mergeInto(LibraryManager.library, {
	modulesLoaded_FirebaseWebGL_app: function(callback) {
		if (Module["FirebaseWebGL"] && Module["FirebaseWebGL"].allLoaded) return;
		window["FirebaseWebGL"] = window["FirebaseWebGL"] || {};
		window["FirebaseWebGL"].modulesLoaded = callback;
	},
	getSDKVersion_FirebaseWebGL_app: function() {
		return Module["FirebaseWebGL"]._app.getSDKVersion();
	},
	initializeApp_FirebaseWebGL_app: function(options, configOrName, callback) {
		return Module["FirebaseWebGL"]._app.initializeApp(options === 0 ? undefined : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)), Module.FirebaseWebGL._util.UTF8ToString(configOrName), callback);
	},
	getApps_FirebaseWebGL_app: function() {
		return Module["FirebaseWebGL"]._app.getApps();
	},
	getApp_FirebaseWebGL_app: function(name) {
		return Module["FirebaseWebGL"]._app.getApp(name === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(name));
	},
	setAutomaticDataCollectionEnabled_FirebaseWebGL_app: function(name, automaticDataCollectionEnabled) {
		return Module["FirebaseWebGL"]._app.setAutomaticDataCollectionEnabled(Module.FirebaseWebGL._util.UTF8ToString(name), automaticDataCollectionEnabled === 0 ? false : true);
	},
	getAutomaticDataCollectionEnabled_FirebaseWebGL_app: function(name) {
		return Module["FirebaseWebGL"]._app.getAutomaticDataCollectionEnabled(Module.FirebaseWebGL._util.UTF8ToString(name));
	},
	registerVersion_FirebaseWebGL_app: function(libraryKeyOrName, version, variant) {
		return Module["FirebaseWebGL"]._app.registerVersion(Module.FirebaseWebGL._util.UTF8ToString(libraryKeyOrName), Module.FirebaseWebGL._util.UTF8ToString(version), Module.FirebaseWebGL._util.UTF8ToString(variant))
	},
	onLog_FirebaseWebGL_app: function(logCallback, options) {
		Module["FirebaseWebGL"]._app.onLog(logCallback === 0 ? null : logCallback, options === 0 ? undefined : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	deleteApp_FirebaseWebGL_app: function(name, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._app.deleteApp(Module.FirebaseWebGL._util.UTF8ToString(name), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	}
});