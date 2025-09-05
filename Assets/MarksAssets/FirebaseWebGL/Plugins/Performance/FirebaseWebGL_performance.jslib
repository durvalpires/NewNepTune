mergeInto(LibraryManager.library, {
	setOrGetProperty_FirebaseWebGL_performance: function(appName, property, value) {
		return Module["FirebaseWebGL"]._performance.setOrGetProperty(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), Module.FirebaseWebGL._util.UTF8ToString(property), value === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(value).toLowerCase() === 'true');
	},
	getPerformance_FirebaseWebGL_performance: function(appName) {
        return Module["FirebaseWebGL"]._performance.getPerformance(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName));
    },
	initializePerformance_FirebaseWebGL_performance: function(appName, settings) {
        return Module["FirebaseWebGL"]._performance.initializePerformance(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), settings === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(settings)));
	},
	trace_FirebaseWebGL_performance: function(appName, name) {
        return Module["FirebaseWebGL"]._performance.trace(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), name === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(name));
	},
	PerformanceTrace_clear_FirebaseWebGL_performance: function(index) {
		Module["FirebaseWebGL"]._performance.PerformanceTrace_clear(index);
	},
	PerformanceTrace_getAttribute_FirebaseWebGL_performance: function(id, attr) {
        return Module["FirebaseWebGL"]._performance.PerformanceTrace_getAttribute(id, Module.FirebaseWebGL._util.UTF8ToString(attr));
	},
	PerformanceTrace_getAttributes_FirebaseWebGL_performance: function(id) {
        return Module["FirebaseWebGL"]._performance.PerformanceTrace_getAttributes(id);
	},
	PerformanceTrace_getMetric_FirebaseWebGL_performance: function(id, metricName) {
        return Module["FirebaseWebGL"]._performance.PerformanceTrace_getMetric(id, Module.FirebaseWebGL._util.UTF8ToString(metricName));
	},
	PerformanceTrace_incrementMetric_FirebaseWebGL_performance: function(id, metricName, num) {
		return Module["FirebaseWebGL"]._performance.PerformanceTrace_incrementMetric(id, Module.FirebaseWebGL._util.UTF8ToString(metricName), num === 0 ? null : Number(Module.FirebaseWebGL._util.UTF8ToString(num)));
	},
	PerformanceTrace_putAttribute_FirebaseWebGL_performance: function(id, attr, value) {
		return Module["FirebaseWebGL"]._performance.PerformanceTrace_putAttribute(id, Module.FirebaseWebGL._util.UTF8ToString(attr), Module.FirebaseWebGL._util.UTF8ToString(value));
	},
	PerformanceTrace_putMetric_FirebaseWebGL_performance: function(id, metricName, num) {
		return Module["FirebaseWebGL"]._performance.PerformanceTrace_putMetric(id, Module.FirebaseWebGL._util.UTF8ToString(metricName), num === 0 ? null : Number(Module.FirebaseWebGL._util.UTF8ToString(num)));
	},
	PerformanceTrace_record_FirebaseWebGL_performance: function(id, startTime, duration, options) {
		return Module["FirebaseWebGL"]._performance.PerformanceTrace_record(id, Number(Module.FirebaseWebGL._util.UTF8ToString(startTime)), Number(Module.FirebaseWebGL._util.UTF8ToString(duration)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	PerformanceTrace_removeAttribute_FirebaseWebGL_performance: function(id, attr) {
        return Module["FirebaseWebGL"]._performance.PerformanceTrace_removeAttribute(id, Module.FirebaseWebGL._util.UTF8ToString(attr));
	},
	PerformanceTrace_startOrStop_FirebaseWebGL_performance: function(id, startOrStop) {
		return Module["FirebaseWebGL"]._performance.PerformanceTrace_startOrStop(id, Module.FirebaseWebGL._util.UTF8ToString(startOrStop));
	}
});