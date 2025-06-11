mergeInto(LibraryManager.library, {
	getFirestore_FirebaseWebGL_firestore: function(name, databaseId) {
		return Module["FirebaseWebGL"]._firestore.getFirestore(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	initializeFirestore_FirebaseWebGL_firestore: function(name, settings, localCacheType, databaseId, autoClear) {
		return Module["FirebaseWebGL"]._firestore.initializeFirestore(Module.FirebaseWebGL._util.UTF8ToString(name), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(settings)), Module.FirebaseWebGL._util.UTF8ToString(localCacheType), Module.FirebaseWebGL._util.UTF8ToString(databaseId), autoClear === 0 ? false : true);
	},
	Firestore_toJSON_FirebaseWebGL_firestore: function(name, databaseId) {
		return Module["FirebaseWebGL"]._firestore.Firestore_toJSON(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	runFunctionWithSettings_FirebaseWebGL_firestore: function(name, settings, typeInnerSettingsParam, nameSettingsParam, idInnerSettings) {
		return Module["FirebaseWebGL"]._firestore.runFunctionWithSettings(Module.FirebaseWebGL._util.UTF8ToString(name), settings === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(settings)), Module.FirebaseWebGL._util.UTF8ToString(typeInnerSettingsParam), Module.FirebaseWebGL._util.UTF8ToString(nameSettingsParam), idInnerSettings);
	},
	runFunctionWithFirestoreStringOrObject_FirebaseWebGL_firestore: function(name, databaseId, funcName, stringOrObject) {
		return Module["FirebaseWebGL"]._firestore.runFunctionWithFirestoreStringOrObject(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(funcName), stringOrObject === 0 ? null : Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(stringOrObject)));
	},
	runFunctionWithFirestorePathPathSegments_FirebaseWebGL_firestore: function(name, databaseId, funcName, path, pathSegments, length) {
		return Module["FirebaseWebGL"]._firestore.runFunctionWithFirestorePathPathSegments(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(funcName), Module.FirebaseWebGL._util.UTF8ToString(path), pathSegments === 0 ? null : Module.FirebaseWebGL._util.getStringArrayFromPtr(pathSegments, length));
	},
	runTaskWithFirestoreStringOrObject_FirebaseWebGL_firestore: function(name, databaseId, funcName, stringOrObject, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestore.runTaskWithFirestoreStringOrObject(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(funcName), stringOrObject === 0 ? null : Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(stringOrObject)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	clear_FirebaseWebGL_firestore: function(name, index) {
		Module["FirebaseWebGL"]._firestore.clear(Module.FirebaseWebGL._util.UTF8ToString(name), index);
	},
	connectFirestoreEmulator_FirebaseWebGL_firestore: function(name, databaseId, host, port, options) {
		return Module["FirebaseWebGL"]._firestore.connectFirestoreEmulator(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(host), port, options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)) );
	},
	runTaskWithReferenceData_FirebaseWebGL_firestore: function(name, databaseId, referenceType, referencePath, funcName, data, options, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestore.runTaskWithReferenceData(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referenceType), Module.FirebaseWebGL._util.UTF8ToString(referencePath), Module.FirebaseWebGL._util.UTF8ToString(funcName), data === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(data)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	Transaction_get_FirebaseWebGL_firestore: function(name, databaseId, referenceType, referencePath, callback, taskCompletionSource, index) {
		Module["FirebaseWebGL"]._firestore.Transaction_get(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referenceType), Module.FirebaseWebGL._util.UTF8ToString(referencePath), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), index);
	},
	WriteBatch_commit_FirebaseWebGL_firestore: function(callback, taskCompletionSource, index) {
		Module["FirebaseWebGL"]._firestore.WriteBatch_commit(callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), index);
	},
	AtomicOperation_runFunctionWithReferenceData_FirebaseWebGL_firestore: function(name, databaseId, referenceType, referencePath, funcName, data, options, source, index) {
		return Module["FirebaseWebGL"]._firestore.AtomicOperation_runFunctionWithReferenceData(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referenceType), Module.FirebaseWebGL._util.UTF8ToString(referencePath), Module.FirebaseWebGL._util.UTF8ToString(funcName), data === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(data)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)), Module.FirebaseWebGL._util.UTF8ToString(source), index);
	},
	runFunctionWithReferenceDataPathPathSegments_FirebaseWebGL_firestore: function(name, databaseId, referenceType, referencePath, funcName, path, pathSegments, length) {
		return Module["FirebaseWebGL"]._firestore.runFunctionWithReferenceDataPathPathSegments(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referenceType), Module.FirebaseWebGL._util.UTF8ToString(referencePath), Module.FirebaseWebGL._util.UTF8ToString(funcName), path === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(path), pathSegments === 0 ? null : Module.FirebaseWebGL._util.getStringArrayFromPtr(pathSegments, length));
	},
	FieldPath_isEqual_FirebaseWebGL_firestore: function(fieldNames, length, otherFieldNames, otherLength) {
		return Module["FirebaseWebGL"]._firestore.FieldPath_isEqual(fieldNames === 0 ? null : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), otherFieldNames === 0 ? null : Module.FirebaseWebGL._util.getStringArrayFromPtr(otherFieldNames, otherLength));
	},
	GeoPoint_isEqual_FirebaseWebGL_firestore: function(latitude, longitude, otherLatitude, otherLongitude) {
		return Module["FirebaseWebGL"]._firestore.GeoPoint_isEqual(latitude, longitude, otherLatitude, otherLongitude);		
	},
	runTaskWithQueryReturnQuerySnapshot_FirebaseWebGL_firestore: function(name, databaseId, query, funcName, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestore.runTaskWithQueryReturnQuerySnapshot(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)), Module.FirebaseWebGL._util.UTF8ToString(funcName), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	runTaskWithQueryReturnAggregateQuerySnapshot_FirebaseWebGL_firestore: function(name, databaseId, query, aggregateSpec, funcName, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestore.runTaskWithQueryReturnAggregateQuerySnapshot(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)), aggregateSpec === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(aggregateSpec)), Module.FirebaseWebGL._util.UTF8ToString(funcName) ,callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	DocumentSnapshot_data_FirebaseWebGL_firestore: function(name, databaseId, documentSnapshot, options) {
		return Module["FirebaseWebGL"]._firestore.DocumentSnapshot_data(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(documentSnapshot)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	DocumentSnapshot_exists_FirebaseWebGL_firestore: function(name, databaseId, documentSnapshot) {
		return Module["FirebaseWebGL"]._firestore.DocumentSnapshot_exists(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(documentSnapshot)));
	},
	DocumentSnapshot_get_FirebaseWebGL_firestore: function(name, databaseId, documentSnapshot, fieldNames, length, options) {
		return Module["FirebaseWebGL"]._firestore.DocumentSnapshot_get(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(documentSnapshot)), length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	runFunctionReturnAggregateField_FirebaseWebGL_firestore: function(funcName, fieldNames, length) {
		return Module["FirebaseWebGL"]._firestore.runFunctionReturnAggregateField(Module.FirebaseWebGL._util.UTF8ToString(funcName), fieldNames === 0 ? null : length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length));
	},
	updateDocMoreFieldsAndValues_FirebaseWebGL_firestore: function(name, databaseId, referencePath, fieldNames, length, value, moreFields, moreValues, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestore.updateDocMoreFieldsAndValues(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referencePath), length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(value)), moreFields === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(moreFields)), moreValues === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(moreValues)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	AtomicOperation_updateMoreFieldsAndValues_FirebaseWebGL_firestore: function(name, databaseId, referencePath, fieldNames, length, value, moreFields, moreValues, source, index) {
		return Module["FirebaseWebGL"]._firestore.AtomicOperation_updateMoreFieldsAndValues(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referencePath), length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(value)), moreFields === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(moreFields)), moreValues === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(moreValues)), Module.FirebaseWebGL._util.UTF8ToString(source), index);
	},
	runFunctionReturnFieldValue_FirebaseWebGL_firestore: function(methodName, elements) {
		return Module["FirebaseWebGL"]._firestore.runFunctionReturnFieldValue(Module.FirebaseWebGL._util.UTF8ToString(methodName), elements === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(elements)));
	},
	FieldValue_isEqual_FirebaseWebGL_firestore: function(methodName, elements, otherMethodName, otherElements) {
		return Module["FirebaseWebGL"]._firestore.FieldValue_isEqual(Module.FirebaseWebGL._util.UTF8ToString(methodName), elements === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(elements)), Module.FirebaseWebGL._util.UTF8ToString(otherMethodName), otherElements === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(otherElements)));
	},
	runTransaction_FirebaseWebGL_firestore: function(name, databaseId, updateCallbackId, updateCallback, finishedId, finishedCallback, removeFinishedCallback, completeTaskCallbackId, completeTaskCallback, callback, taskCompletionSource, options) {
		Module["FirebaseWebGL"]._firestore.runTransaction(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId),  Module.FirebaseWebGL._util.UTF8ToString(updateCallbackId), updateCallback, Module.FirebaseWebGL._util.UTF8ToString(finishedId), finishedCallback, removeFinishedCallback, Module.FirebaseWebGL._util.UTF8ToString(completeTaskCallbackId), completeTaskCallback, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), options === 0 ? undefined : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	AtomicOperation_clear_FirebaseWebGL_firestore: function(source, index) {
		Module["FirebaseWebGL"]._firestore.AtomicOperation_clear(Module.FirebaseWebGL._util.UTF8ToString(source), index);
	},
	writeBatch_FirebaseWebGL_firestore: function(name, databaseId) {
		return Module["FirebaseWebGL"]._firestore.writeBatch(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	where_FirebaseWebGL_firestore: function(fieldNames, length, opStr, value, name, databaseId) {
		return Module["FirebaseWebGL"]._firestore.where(length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), Module.FirebaseWebGL._util.UTF8ToString(opStr), Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(value)), name === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(name), databaseId === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	orderBy_FirebaseWebGL_firestore: function(fieldNames, length, directionStr) {
		return Module["FirebaseWebGL"]._firestore.orderBy(length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), Module.FirebaseWebGL._util.UTF8ToString(directionStr));
	},
	limit_FirebaseWebGL_firestore: function(limit, limitFunc) {
		return Module["FirebaseWebGL"]._firestore.limit(limit, Module.FirebaseWebGL._util.UTF8ToString(limitFunc));
	},
	startOrEnd_FirebaseWebGL_firestore: function(func, fieldValues, name, databaseId) {
		return Module["FirebaseWebGL"]._firestore.startOrEnd(Module.FirebaseWebGL._util.UTF8ToString(func), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(fieldValues)), name === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(name), databaseId === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	query_FirebaseWebGL_firestore: function(query, name, databaseId, compositeFilter, queryConstraints) {
		return Module["FirebaseWebGL"]._firestore.query(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)), Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), compositeFilter == 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(compositeFilter)) ,queryConstraints === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(queryConstraints)));	
	},
	collectionGroup_FirebaseWebGL_firestore: function(name, databaseId, collectionId) {
		return Module["FirebaseWebGL"]._firestore.collectionGroup(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(collectionId));	
	},
	namedQuery_FirebaseWebGL_firestore: function(name, databaseId, queryName, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestore.namedQuery(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(queryName), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	loadBundle_FirebaseWebGL_firestore: function(name, databaseId, bundleData, length) {
		return Module["FirebaseWebGL"]._firestore.loadBundle(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.getByteArrayFromPtr(bundleData, length));
	},
	loadBundleString_FirebaseWebGL_firestore: function(name, databaseId, bundleData) {
		return Module["FirebaseWebGL"]._firestore.loadBundle(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(bundleData));
	},
	onProgress_LoadBundleTask_FirebaseWebGL_firestore: function(id, nextId, next, errorId, error, completeId, complete) {
		return Module["FirebaseWebGL"]._firestore.onProgress_LoadBundleTask(id, Module.FirebaseWebGL._util.UTF8ToString(nextId), next === 0 ? null : next, Module.FirebaseWebGL._util.UTF8ToString(errorId), error === 0 ? null : error, Module.FirebaseWebGL._util.UTF8ToString(completeId), complete === 0 ? null : complete);
	},
	getAwaiter_LoadBundleTask_FirebaseWebGL_firestore: function(id, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestore.getAwaiter_LoadBundleTask(id, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	onSnapshotDocumentReference_FirebaseWebGL_firestore: function(name, databaseId, referencePath, options, onNextId, onNext, onErrorId, onError) {
		return Module["FirebaseWebGL"]._firestore.onSnapshotDocumentReference(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referencePath), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)), Module.FirebaseWebGL._util.UTF8ToString(onNextId), onNext, Module.FirebaseWebGL._util.UTF8ToString(onErrorId), onError === 0 ? null : onError);
	},
	onSnapshotQuery_FirebaseWebGL_firestore: function(name, databaseId, query, options, onNextId, onNext, onErrorId, onError) {
		return Module["FirebaseWebGL"]._firestore.onSnapshotQuery(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)), Module.FirebaseWebGL._util.UTF8ToString(onNextId), onNext, Module.FirebaseWebGL._util.UTF8ToString(onErrorId), onError === 0 ? null : onError);
	},
	onSnapshotsInSync_FirebaseWebGL_firestore: function(name, databaseId, onSyncId, onSync) {
		return Module["FirebaseWebGL"]._firestore.onSnapshotsInSync(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(onSyncId), onSync);
	},
	queryEqual_FirebaseWebGL_firestore: function(nameLeft, databaseIdLeft, queryLeft, nameRight, databaseIdRight, queryRight) {
		return Module["FirebaseWebGL"]._firestore.queryEqual(Module.FirebaseWebGL._util.UTF8ToString(nameLeft), Module.FirebaseWebGL._util.UTF8ToString(databaseIdLeft), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(queryLeft)), Module.FirebaseWebGL._util.UTF8ToString(nameRight), Module.FirebaseWebGL._util.UTF8ToString(databaseIdRight), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(queryRight)));
	},
	refEqual_FirebaseWebGL_firestore: function(nameLeft, databaseIdLeft, referencePathLeft, nameRight, databaseIdRight, referencePathRight) {
		return Module["FirebaseWebGL"]._firestore.refEqual(Module.FirebaseWebGL._util.UTF8ToString(nameLeft), Module.FirebaseWebGL._util.UTF8ToString(databaseIdLeft), Module.FirebaseWebGL._util.UTF8ToString(referencePathLeft), Module.FirebaseWebGL._util.UTF8ToString(nameRight), Module.FirebaseWebGL._util.UTF8ToString(databaseIdRight), Module.FirebaseWebGL._util.UTF8ToString(referencePathRight));
	},
	snapshotEqualDocument_FirebaseWebGL_firestore: function(leftSnapshot, leftName, leftDatabaseId, rightSnapshot, rightName, rightDatabaseId) {
		return Module["FirebaseWebGL"]._firestore.snapshotEqualDocument(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(leftSnapshot)), Module.FirebaseWebGL._util.UTF8ToString(leftName), Module.FirebaseWebGL._util.UTF8ToString(leftDatabaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(rightSnapshot)), Module.FirebaseWebGL._util.UTF8ToString(rightName), Module.FirebaseWebGL._util.UTF8ToString(rightDatabaseId));
	},
	runFunctionWithPersistentCacheIndexManager_FirebaseWebGL_firestore: function(indexManager, funcName) {
		return Module["FirebaseWebGL"]._firestore.runFunctionWithPersistentCacheIndexManager(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(indexManager)), Module.FirebaseWebGL._util.UTF8ToString(funcName));
	},
	getPersistentCacheIndexManager_FirebaseWebGL_firestore: function(name, databaseId) {
		return Module["FirebaseWebGL"]._firestore.getPersistentCacheIndexManager(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	}
});