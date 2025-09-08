mergeInto(LibraryManager.library, {
	unsubscribe_FirebaseWebGL_util: function(index) {
		Module.FirebaseWebGL._util.unsubscribe(index);
	},
	setLogLevel_FirebaseWebGL_util: function(module, logLevel) {
		return Module["FirebaseWebGL"]._util.setLogLevel(Module.FirebaseWebGL._util.UTF8ToString(module), Module.FirebaseWebGL._util.UTF8ToString(logLevel));
	},
	runOnPointerDown_FirebaseWebGL_util: function(nextId, next) {
		return Module["FirebaseWebGL"]._util.runOnPointerDown(Module.FirebaseWebGL._util.UTF8ToString(nextId), next === 0 ? null : next);
	},
});