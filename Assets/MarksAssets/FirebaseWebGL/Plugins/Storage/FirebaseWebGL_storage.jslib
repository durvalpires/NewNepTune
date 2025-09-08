mergeInto(LibraryManager.library, {
	setOrGetOperationRetryTime_FirebaseWebGL_storage: function(isGet, value, operation, appName, bucketUrl) {
		return Module["FirebaseWebGL"]._storage.setOrGetOperationRetryTime(isGet === 0 ? false : true, value, Module.FirebaseWebGL._util.UTF8ToString(operation), Module.FirebaseWebGL._util.UTF8ToString(appName), Module.FirebaseWebGL._util.UTF8ToString(bucketUrl));
	},
	getStorage_FirebaseWebGL_storage: function(appName, bucketUrl) {
		return Module["FirebaseWebGL"]._storage.getStorage(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), bucketUrl === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(bucketUrl));
	},
	connectStorageEmulator_FirebaseWebGL_storage: function(appName, url, host, port, options) {
		return Module["FirebaseWebGL"]._storage.connectStorageEmulator(Module.FirebaseWebGL._util.UTF8ToString(appName), url === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(url), Module.FirebaseWebGL._util.UTF8ToString(host), port, options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	ref_FirebaseWebGL_storage: function(appName, bucketUrl, url) {
		return Module["FirebaseWebGL"]._storage.ref(appName === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(appName), bucketUrl === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(bucketUrl), url === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(url));
	},
	refFromRef_FirebaseWebGL_storage: function(ref, path) {
		return Module["FirebaseWebGL"]._storage.refFromRef(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), Module.FirebaseWebGL._util.UTF8ToString(path));
	},
	uploadBytes_FirebaseWebGL_storage: function(ref, data, length, metadata, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._storage.uploadBytes(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), Module.FirebaseWebGL._util.getByteArrayFromPtr(data, length), metadata === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(metadata)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	uploadString_FirebaseWebGL_storage: function(ref, value, format, metadata, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._storage.uploadString(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), Module.FirebaseWebGL._util.UTF8ToString(value), Module.FirebaseWebGL._util.UTF8ToString(format), metadata === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(metadata)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	updateMetadata_FirebaseWebGL_storage: function(ref, metadata, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._storage.updateMetadata(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(metadata)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	list_FirebaseWebGL_storage: function(func, ref, options, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._storage.list(Module.FirebaseWebGL._util.UTF8ToString(func), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	getMetadata_FirebaseWebGL_storage: function(ref, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._storage.getMetadata(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	getDownloadURL_FirebaseWebGL_storage: function(ref, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._storage.getDownloadURL(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	getBytes_FirebaseWebGL_storage: function(ref, maxDownloadSizeBytes, callback, taskCompletionSource, errorCallback, errorCallbackId) {
		Module["FirebaseWebGL"]._storage.getBytes(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), maxDownloadSizeBytes, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), errorCallback, Module.FirebaseWebGL._util.UTF8ToString(errorCallbackId));
	},
	deleteObject_FirebaseWebGL_storage: function(ref, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._storage.deleteObject(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	uploadBytesResumable_FirebaseWebGL_storage: function(ref, data, length, metadata) {
		return Module["FirebaseWebGL"]._storage.uploadBytesResumable(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(ref)), Module.FirebaseWebGL._util.getByteArrayFromPtr(data, length), metadata === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(metadata)));
	},
	getAwaiter_UploadTask_FirebaseWebGL_storage: function(id, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._storage.getAwaiter_UploadTask(id, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	getSnapshot_UploadTask_FirebaseWebGL_storage: function(id) {
		return Module["FirebaseWebGL"]._storage.getSnapshot_UploadTask(id);
	},
	runFuncFrom_UploadTask_FirebaseWebGL_storage: function(id, func) {
		return Module["FirebaseWebGL"]._storage.runFuncFrom_UploadTask(id, Module.FirebaseWebGL._util.UTF8ToString(func));
	},
	on_UploadTask_FirebaseWebGL_storage: function(event, id, nextId, next, errorId, error, completeId, complete) {
		return Module["FirebaseWebGL"]._storage.on_UploadTask(Module.FirebaseWebGL._util.UTF8ToString(event), id, Module.FirebaseWebGL._util.UTF8ToString(nextId), next === 0 ? null : next, Module.FirebaseWebGL._util.UTF8ToString(errorId), error === 0 ? null : error, Module.FirebaseWebGL._util.UTF8ToString(completeId), complete === 0 ? null : complete);		
	}
});