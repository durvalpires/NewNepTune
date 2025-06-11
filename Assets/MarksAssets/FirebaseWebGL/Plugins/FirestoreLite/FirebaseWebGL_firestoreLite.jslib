mergeInto(LibraryManager.library, {
	getFirestore_FirebaseWebGL_firestoreLite: function(name, databaseId) {
		return Module["FirebaseWebGL"]._firestoreLite.getFirestore(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	initializeFirestore_FirebaseWebGL_firestoreLite: function(name, settings, databaseId) {
		return Module["FirebaseWebGL"]._firestoreLite.initializeFirestore(Module.FirebaseWebGL._util.UTF8ToString(name), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(settings)), Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	Firestore_toJSON_FirebaseWebGL_firestoreLite: function(name, databaseId) {
		return Module["FirebaseWebGL"]._firestoreLite.Firestore_toJSON(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	runFunctionWithFirestoreStringOrObject_FirebaseWebGL_firestoreLite: function(name, databaseId, funcName, stringOrObject) {
		return Module["FirebaseWebGL"]._firestoreLite.runFunctionWithFirestoreStringOrObject(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(funcName), stringOrObject === 0 ? null : Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(stringOrObject)));
	},
	runFunctionWithFirestorePathPathSegments_FirebaseWebGL_firestoreLite: function(name, databaseId, funcName, path, pathSegments, length) {
		return Module["FirebaseWebGL"]._firestoreLite.runFunctionWithFirestorePathPathSegments(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(funcName), Module.FirebaseWebGL._util.UTF8ToString(path), pathSegments === 0 ? null : Module.FirebaseWebGL._util.getStringArrayFromPtr(pathSegments, length));
	},
	runTaskWithFirestoreStringOrObject_FirebaseWebGL_firestoreLite: function(name, databaseId, funcName, stringOrObject, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestoreLite.runTaskWithFirestoreStringOrObject(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(funcName), stringOrObject === 0 ? null : Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(stringOrObject)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	connectFirestoreEmulator_FirebaseWebGL_firestoreLite: function(name, databaseId, host, port, options) {
		return Module["FirebaseWebGL"]._firestoreLite.connectFirestoreEmulator(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(host), port, options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)) );
	},
	runTaskWithReferenceData_FirebaseWebGL_firestoreLite: function(name, databaseId, referenceType, referencePath, funcName, data, options, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestoreLite.runTaskWithReferenceData(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referenceType), Module.FirebaseWebGL._util.UTF8ToString(referencePath), Module.FirebaseWebGL._util.UTF8ToString(funcName), data === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(data)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	Transaction_get_FirebaseWebGL_firestoreLite: function(name, databaseId, referenceType, referencePath, callback, taskCompletionSource, index) {
		Module["FirebaseWebGL"]._firestoreLite.Transaction_get(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referenceType), Module.FirebaseWebGL._util.UTF8ToString(referencePath), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), index);
	},
	WriteBatch_commit_FirebaseWebGL_firestoreLite: function(callback, taskCompletionSource, index) {
		Module["FirebaseWebGL"]._firestoreLite.WriteBatch_commit(callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), index);
	},
	AtomicOperation_runFunctionWithReferenceData_FirebaseWebGL_firestoreLite: function(name, databaseId, referenceType, referencePath, funcName, data, options, source, index) {
		return Module["FirebaseWebGL"]._firestoreLite.AtomicOperation_runFunctionWithReferenceData(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referenceType), Module.FirebaseWebGL._util.UTF8ToString(referencePath), Module.FirebaseWebGL._util.UTF8ToString(funcName), data === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(data)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)), Module.FirebaseWebGL._util.UTF8ToString(source), index);
	},
	runFunctionWithReferenceDataPathPathSegments_FirebaseWebGL_firestoreLite: function(name, databaseId, referenceType, referencePath, funcName, path, pathSegments, length) {
		return Module["FirebaseWebGL"]._firestoreLite.runFunctionWithReferenceDataPathPathSegments(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referenceType), Module.FirebaseWebGL._util.UTF8ToString(referencePath), Module.FirebaseWebGL._util.UTF8ToString(funcName), path === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(path), pathSegments === 0 ? null : Module.FirebaseWebGL._util.getStringArrayFromPtr(pathSegments, length));
	},
	FieldPath_isEqual_FirebaseWebGL_firestoreLite: function(fieldNames, length, otherFieldNames, otherLength) {
		return Module["FirebaseWebGL"]._firestoreLite.FieldPath_isEqual(fieldNames === 0 ? null : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), otherFieldNames === 0 ? null : Module.FirebaseWebGL._util.getStringArrayFromPtr(otherFieldNames, otherLength));
	},
	GeoPoint_isEqual_FirebaseWebGL_firestoreLite: function(latitude, longitude, otherLatitude, otherLongitude) {
		return Module["FirebaseWebGL"]._firestoreLite.GeoPoint_isEqual(latitude, longitude, otherLatitude, otherLongitude);		
	},
	runTaskWithQueryReturnQuerySnapshot_FirebaseWebGL_firestoreLite: function(name, databaseId, query, funcName, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestoreLite.runTaskWithQueryReturnQuerySnapshot(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)), Module.FirebaseWebGL._util.UTF8ToString(funcName), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	runTaskWithQueryReturnAggregateQuerySnapshot_FirebaseWebGL_firestoreLite: function(name, databaseId, query, aggregateSpec, funcName, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestoreLite.runTaskWithQueryReturnAggregateQuerySnapshot(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)), aggregateSpec === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(aggregateSpec)), Module.FirebaseWebGL._util.UTF8ToString(funcName) ,callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	DocumentSnapshot_data_FirebaseWebGL_firestoreLite: function(name, databaseId, documentSnapshot, options) {
		return Module["FirebaseWebGL"]._firestoreLite.DocumentSnapshot_data(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(documentSnapshot)), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	DocumentSnapshot_exists_FirebaseWebGL_firestoreLite: function(name, databaseId, documentSnapshot) {
		return Module["FirebaseWebGL"]._firestoreLite.DocumentSnapshot_exists(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(documentSnapshot)));
	},
	DocumentSnapshot_get_FirebaseWebGL_firestoreLite: function(name, databaseId, documentSnapshot, fieldNames, length, options) {
		return Module["FirebaseWebGL"]._firestoreLite.DocumentSnapshot_get(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(documentSnapshot)), length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), options === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	runFunctionReturnAggregateField_FirebaseWebGL_firestoreLite: function(funcName, fieldNames, length) {
		return Module["FirebaseWebGL"]._firestoreLite.runFunctionReturnAggregateField(Module.FirebaseWebGL._util.UTF8ToString(funcName), fieldNames === 0 ? null : length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length));
	},
	updateDocMoreFieldsAndValues_FirebaseWebGL_firestoreLite: function(name, databaseId, referencePath, fieldNames, length, value, moreFields, moreValues, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._firestoreLite.updateDocMoreFieldsAndValues(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referencePath), length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(value)), moreFields === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(moreFields)), moreValues === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(moreValues)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	AtomicOperation_updateMoreFieldsAndValues_FirebaseWebGL_firestoreLite: function(name, databaseId, referencePath, fieldNames, length, value, moreFields, moreValues, source, index) {
		return Module["FirebaseWebGL"]._firestoreLite.AtomicOperation_updateMoreFieldsAndValues(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(referencePath), length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(value)), moreFields === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(moreFields)), moreValues === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(moreValues)), Module.FirebaseWebGL._util.UTF8ToString(source), index);
	},
	runFunctionReturnFieldValue_FirebaseWebGL_firestoreLite: function(methodName, elements) {
		return Module["FirebaseWebGL"]._firestoreLite.runFunctionReturnFieldValue(Module.FirebaseWebGL._util.UTF8ToString(methodName), elements === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(elements)));
	},
	FieldValue_isEqual_FirebaseWebGL_firestoreLite: function(methodName, elements, otherMethodName, otherElements) {
		return Module["FirebaseWebGL"]._firestoreLite.FieldValue_isEqual(Module.FirebaseWebGL._util.UTF8ToString(methodName), elements === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(elements)), Module.FirebaseWebGL._util.UTF8ToString(otherMethodName), otherElements === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(otherElements)));
	},
	runTransaction_FirebaseWebGL_firestoreLite: function(name, databaseId, updateCallbackId, updateCallback, finishedId, finishedCallback, removeFinishedCallback, completeTaskCallbackId, completeTaskCallback, callback, taskCompletionSource, options) {
		Module["FirebaseWebGL"]._firestoreLite.runTransaction(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId),  Module.FirebaseWebGL._util.UTF8ToString(updateCallbackId), updateCallback, Module.FirebaseWebGL._util.UTF8ToString(finishedId), finishedCallback, removeFinishedCallback, Module.FirebaseWebGL._util.UTF8ToString(completeTaskCallbackId), completeTaskCallback, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), options === 0 ? undefined : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	AtomicOperation_clear_FirebaseWebGL_firestoreLite: function(source, index) {
		Module["FirebaseWebGL"]._firestoreLite.AtomicOperation_clear(Module.FirebaseWebGL._util.UTF8ToString(source), index);
	},
	writeBatch_FirebaseWebGL_firestoreLite: function(name, databaseId) {
		return Module["FirebaseWebGL"]._firestoreLite.writeBatch(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	where_FirebaseWebGL_firestoreLite: function(fieldNames, length, opStr, value, name, databaseId) {
		return Module["FirebaseWebGL"]._firestoreLite.where(length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), Module.FirebaseWebGL._util.UTF8ToString(opStr), Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(value)), name === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(name), databaseId === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	orderBy_FirebaseWebGL_firestoreLite: function(fieldNames, length, directionStr) {
		return Module["FirebaseWebGL"]._firestoreLite.orderBy(length === 0 ? Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, 1)[0] : Module.FirebaseWebGL._util.getStringArrayFromPtr(fieldNames, length), Module.FirebaseWebGL._util.UTF8ToString(directionStr));
	},
	limit_FirebaseWebGL_firestoreLite: function(limit, limitFunc) {
		return Module["FirebaseWebGL"]._firestoreLite.limit(limit, Module.FirebaseWebGL._util.UTF8ToString(limitFunc));
	},
	startOrEnd_FirebaseWebGL_firestoreLite: function(func, fieldValues, name, databaseId) {
		return Module["FirebaseWebGL"]._firestoreLite.startOrEnd(Module.FirebaseWebGL._util.UTF8ToString(func), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(fieldValues)), name === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(name), databaseId === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(databaseId));
	},
	query_FirebaseWebGL_firestoreLite: function(query, name, databaseId, compositeFilter, queryConstraints) {
		return Module["FirebaseWebGL"]._firestoreLite.query(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(query)), Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), compositeFilter == 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(compositeFilter)) ,queryConstraints === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(queryConstraints)));	
	},
	collectionGroup_FirebaseWebGL_firestoreLite: function(name, databaseId, collectionId) {
		return Module["FirebaseWebGL"]._firestoreLite.collectionGroup(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(databaseId), Module.FirebaseWebGL._util.UTF8ToString(collectionId));	
	},
	queryEqual_FirebaseWebGL_firestoreLite: function(nameLeft, databaseIdLeft, queryLeft, nameRight, databaseIdRight, queryRight) {
		return Module["FirebaseWebGL"]._firestoreLite.queryEqual(Module.FirebaseWebGL._util.UTF8ToString(nameLeft), Module.FirebaseWebGL._util.UTF8ToString(databaseIdLeft), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(queryLeft)), Module.FirebaseWebGL._util.UTF8ToString(nameRight), Module.FirebaseWebGL._util.UTF8ToString(databaseIdRight), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(queryRight)));
	},
	refEqual_FirebaseWebGL_firestoreLite: function(nameLeft, databaseIdLeft, referencePathLeft, nameRight, databaseIdRight, referencePathRight) {
		return Module["FirebaseWebGL"]._firestoreLite.refEqual(Module.FirebaseWebGL._util.UTF8ToString(nameLeft), Module.FirebaseWebGL._util.UTF8ToString(databaseIdLeft), Module.FirebaseWebGL._util.UTF8ToString(referencePathLeft), Module.FirebaseWebGL._util.UTF8ToString(nameRight), Module.FirebaseWebGL._util.UTF8ToString(databaseIdRight), Module.FirebaseWebGL._util.UTF8ToString(referencePathRight));
	},
	snapshotEqualDocument_FirebaseWebGL_firestoreLite: function(leftSnapshot, leftName, leftDatabaseId, rightSnapshot, rightName, rightDatabaseId) {
		return Module["FirebaseWebGL"]._firestoreLite.snapshotEqualDocument(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(leftSnapshot)), Module.FirebaseWebGL._util.UTF8ToString(leftName), Module.FirebaseWebGL._util.UTF8ToString(leftDatabaseId), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(rightSnapshot)), Module.FirebaseWebGL._util.UTF8ToString(rightName), Module.FirebaseWebGL._util.UTF8ToString(rightDatabaseId));
	}
});