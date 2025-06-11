mergeInto(LibraryManager.library, {
	enableLogger_FirebaseWebGL_database: function(logCallback) {
		Module["FirebaseWebGL"]._database.enableLogger(logCallback);
	},
	enableLogging_FirebaseWebGL_database: function(enabled, persistent) {
		return Module["FirebaseWebGL"]._database.enableLogging(enabled === 0 ? false : true, persistent === 0 ? false : true);
	},
	getDatabase_FirebaseWebGL_database: function(appName, url) {
		return Module["FirebaseWebGL"]._database.getDatabase(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), url === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(url));
	},
	connectDatabaseEmulator_FirebaseWebGL_database: function(appName, url, host, port, options) {
		return Module["FirebaseWebGL"]._database.connectDatabaseEmulator(Module.FirebaseWebGL._util.UTF8ToString(appName), url === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(url), Module.FirebaseWebGL._util.UTF8ToString(host), port, options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)) );
	},
	forceFunction_FirebaseWebGL_database: function(func) {
		return Module["FirebaseWebGL"]._database.forceFunction(Module.FirebaseWebGL._util.UTF8ToString(func));
	},
	serverTimestamp_FirebaseWebGL_database: function() {
		return Module["FirebaseWebGL"]._database.serverTimestamp();
	},
	increment_FirebaseWebGL_database: function(delta) {
		return Module["FirebaseWebGL"]._database.increment(delta);
	},
	goOnline_FirebaseWebGL_database: function(appName, url) {
		return Module["FirebaseWebGL"]._database.goOnline(Module.FirebaseWebGL._util.UTF8ToString(appName), url === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(url));
	},
	goOffline_FirebaseWebGL_database: function(appName, url) {
		return Module["FirebaseWebGL"]._database.goOffline(Module.FirebaseWebGL._util.UTF8ToString(appName), url === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(url));
	},
	ref_FirebaseWebGL_database: function(appName, url, path) {
		return Module["FirebaseWebGL"]._database.ref(Module.FirebaseWebGL._util.UTF8ToString(appName), url === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(url), path === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(path));
	},
	refFromURL_FirebaseWebGL_database: function(appName, dbUrl, url) {
		return Module["FirebaseWebGL"]._database.refFromURL(Module.FirebaseWebGL._util.UTF8ToString(appName), dbUrl === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(dbUrl), Module.FirebaseWebGL._util.UTF8ToString(url));
	},
	query_FirebaseWebGL_database: function(query, queryConstraints) {
		return Module["FirebaseWebGL"]._database.query(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)), queryConstraints === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(queryConstraints)));	
	},
	Query_clear_FirebaseWebGL_database: function(id) {
		Module["FirebaseWebGL"]._database.Query_clear(id);
	},
	Query_equals_FirebaseWebGL_database: function(current, query) {
		return Module["FirebaseWebGL"]._database.Query_equals(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(current)), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)));
	},
	set_FirebaseWebGL_database: function(func, ref, value, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._database.set(Module.FirebaseWebGL._util.UTF8ToString(func), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), value === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(value)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	setWithPriority_FirebaseWebGL_database: function(ref, value, priority, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._database.setWithPriority(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), value === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(value)), priority === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(priority)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	get_FirebaseWebGL_database: function(query, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._database.get(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	remove_FirebaseWebGL_database: function(ref, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._database.remove(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	update_FirebaseWebGL_database: function(ref, values, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._database.update(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(values)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	push_FirebaseWebGL_database: function(parent, value, callback, taskCompletionSource) {
		return Module["FirebaseWebGL"]._database.push(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(parent)), value === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(value)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	child_FirebaseWebGL_database: function(parent, path) {
		return Module["FirebaseWebGL"]._database.child(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(parent)), Module.FirebaseWebGL._util.UTF8ToString(path));
	},
	getValueKeyConstraint_FirebaseWebGL_database: function(func, value, key) {
		return Module["FirebaseWebGL"]._database.getValueKeyConstraint(Module.FirebaseWebGL._util.UTF8ToString(func), value === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(value)), key === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(key));
	},
	getLimitConstraint_FirebaseWebGL_database: function(func, limit) {
		return Module["FirebaseWebGL"]._database.getLimitConstraint(Module.FirebaseWebGL._util.UTF8ToString(func), limit);	
	},
	getOrderByConstraint_FirebaseWebGL_database: function(func, value) {
		return Module["FirebaseWebGL"]._database.getOrderByConstraint(Module.FirebaseWebGL._util.UTF8ToString(func), value === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(value));	
	},
	onValueOrOnChild_FirebaseWebGL_database: function(func, query, snapshotId, callback, cancelCallbackId, cancelCallback, options) {
		return Module["FirebaseWebGL"]._database.onValueOrOnChild(Module.FirebaseWebGL._util.UTF8ToString(func), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)), Module.FirebaseWebGL._util.UTF8ToString(snapshotId), callback, Module.FirebaseWebGL._util.UTF8ToString(cancelCallbackId), cancelCallback === 0 ? null : cancelCallback, options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	onDisconnect_func_FirebaseWebGL_database: function(func, id, value, callback, taskCompletionSource, priority) {
		Module["FirebaseWebGL"]._database.onDisconnect_func(Module.FirebaseWebGL._util.UTF8ToString(func), id, value === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(value)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), priority === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(priority)));
	},
	OnDisconnect_clear_FirebaseWebGL_database: function(id) {
		Module["FirebaseWebGL"]._database.OnDisconnect_clear(id);
	},
	onDisconnect_FirebaseWebGL_database: function(ref) {
		return Module["FirebaseWebGL"]._database.onDisconnect(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)));
	},
	runTransaction_FirebaseWebGL_database: function(ref, transactionUpdateId, transactionUpdate, options, callback, taskCompletionSource, removeTransactionCallback) {
		Module["FirebaseWebGL"]._database.runTransaction(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), Module.FirebaseWebGL._util.UTF8ToString(transactionUpdateId), transactionUpdate, options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), removeTransactionCallback);
	}
});