mergeInto(LibraryManager.library, {
	getAuth_FirebaseWebGL_auth: function(name) {
		return Module["FirebaseWebGL"]._auth.getAuth(Module.FirebaseWebGL._util.UTF8ToString(name));
	},
	initializeAuth_FirebaseWebGL_auth: function(name, deps) {
		return Module["FirebaseWebGL"]._auth.initializeAuth(Module.FirebaseWebGL._util.UTF8ToString(name), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(deps)));
	},
	setLanguageCode_FirebaseWebGL_auth: function(name, languageCode) {
		return Module["FirebaseWebGL"]._auth.setLanguageCode(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(languageCode));
	},
	setTenantId_FirebaseWebGL_auth: function(name, tenantId) {
		return Module["FirebaseWebGL"]._auth.setTenantId(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(tenantId));
	},
	setAppVerificationDisabledForTesting_FirebaseWebGL_auth: function(name, value) {
		return Module["FirebaseWebGL"]._auth.setAppVerificationDisabledForTesting(Module.FirebaseWebGL._util.UTF8ToString(name), value === 0 ? false : true);
	},
	createUserWithEmailAndPassword_FirebaseWebGL_auth: function(name, email, password, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.createUserWithEmailAndPassword(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(email), Module.FirebaseWebGL._util.UTF8ToString(password), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	signInWithEmailAndPassword_FirebaseWebGL_auth: function(name, email, password, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.signInWithEmailAndPassword(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(email), Module.FirebaseWebGL._util.UTF8ToString(password), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	onAuthStateChangedNext_FirebaseWebGL_auth: function(name, nextId, next) {
		return Module["FirebaseWebGL"]._auth.onAuthStateChangedNext(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(nextId), next);
	},
	onIdTokenChanged_FirebaseWebGL_auth: function(name, nextId, next) {
		return Module["FirebaseWebGL"]._auth.onIdTokenChanged(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(nextId), next);
	},
	beforeAuthStateChanged_FirebaseWebGL_auth: function(name, callbackId, callback, abortId, onAbort) {
		return Module["FirebaseWebGL"]._auth.beforeAuthStateChanged(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(callbackId), callback, Module.FirebaseWebGL._util.UTF8ToString(abortId), onAbort === 0 ? null : onAbort);
	},
	deleteUser_FirebaseWebGL_auth: function(uid, callback, taskCompletionSource, autoClear) {
		Module["FirebaseWebGL"]._auth.deleteUser(Module.FirebaseWebGL._util.UTF8ToString(uid), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), autoClear === 0 ? false : true);
	},
	reloadUser_FirebaseWebGL_auth: function(uid, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.reloadUser(Module.FirebaseWebGL._util.UTF8ToString(uid), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	clearUser_FirebaseWebGL_auth: function(uid) {
		Module["FirebaseWebGL"]._auth.clearUser(Module.FirebaseWebGL._util.UTF8ToString(uid));
	},
	getIdTokenUser_FirebaseWebGL_auth: function(uid, forceRefresh, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.getIdTokenUser(Module.FirebaseWebGL._util.UTF8ToString(uid), forceRefresh === 0 ? false : true, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	getIdTokenResultUser_FirebaseWebGL_auth: function(uid, forceRefresh, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.getIdTokenResultUser(Module.FirebaseWebGL._util.UTF8ToString(uid), forceRefresh === 0 ? false : true, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	getPropertyFromUser_FirebaseWebGL_auth: function(uid, property) {
		return Module["FirebaseWebGL"]._auth.getPropertyFromUser(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(property));
	},
	getMetadataFromUser_FirebaseWebGL_auth: function(uid) {
		return Module["FirebaseWebGL"]._auth.getMetadataFromUser(Module.FirebaseWebGL._util.UTF8ToString(uid));
	},
	unlink_FirebaseWebGL_auth: function(uid, providerId, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.unlink(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(providerId), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	connectAuthEmulator_FirebaseWebGL_auth: function(name, url, options) {
		return Module["FirebaseWebGL"]._auth.connectAuthEmulator(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(url), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(options)));
	},
	sendEmailVerification_FirebaseWebGL_auth: function(uid, actionCodeSettings, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.sendEmailVerification(Module.FirebaseWebGL._util.UTF8ToString(uid), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(actionCodeSettings)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	applyActionCode_FirebaseWebGL_auth: function(name, oobCode, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.applyActionCode(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(oobCode), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	getCurrentUserFromAuth_FirebaseWebGL_auth: function(name) {
		return Module["FirebaseWebGL"]._auth.getCurrentUserFromAuth(Module.FirebaseWebGL._util.UTF8ToString(name));
	},
	getPropertyFromAuth_FirebaseWebGL_auth: function(name, property) {
		return Module["FirebaseWebGL"]._auth.getPropertyFromAuth(name === 0 ? null : Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(property));
	},
	signOut_FirebaseWebGL_auth: function(name, callback, taskCompletionSource, autoClear) {
		Module["FirebaseWebGL"]._auth.signOut(Module.FirebaseWebGL._util.UTF8ToString(name), callback, UTF8ToString(taskCompletionSource), autoClear === 0 ? false : true);
	},
	sendPasswordResetEmail_FirebaseWebGL_auth: function(name, email, actionCodeSettings, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.sendPasswordResetEmail(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(email), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(actionCodeSettings)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	confirmPasswordReset_FirebaseWebGL_auth: function(name, oobCode, newPassword, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.confirmPasswordReset(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(oobCode), Module.FirebaseWebGL._util.UTF8ToString(newPassword), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	sendSignInLinkToEmail_FirebaseWebGL_auth: function(name, email, actionCodeSettings, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.sendSignInLinkToEmail(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(email), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(actionCodeSettings)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	isSignInWithEmailLink_FirebaseWebGL_auth: function(name, emailLink) {
		return Module["FirebaseWebGL"]._auth.isSignInWithEmailLink(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(emailLink));
	},
	signInWithEmailLink_FirebaseWebGL_auth: function(name, email, emailLink, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.signInWithEmailLink(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(email), Module.FirebaseWebGL._util.UTF8ToString(emailLink), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	emailAuthProvider_credentialWithLink_FirebaseWebGL_auth: function(email, emailLink) {
		return Module["FirebaseWebGL"]._auth.emailAuthProvider_credentialWithLink(Module.FirebaseWebGL._util.UTF8ToString(email), Module.FirebaseWebGL._util.UTF8ToString(emailLink));
	},
	signInAnonymously_FirebaseWebGL_auth: function(name, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.signInAnonymously(Module.FirebaseWebGL._util.UTF8ToString(name), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	fetchSignInMethodsForEmail_FirebaseWebGL_auth: function(name, email, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.fetchSignInMethodsForEmail(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(email), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	AuthProvider_credential_FirebaseWebGL_auth: function(authProvider, idToken, accessToken) {
		return Module["FirebaseWebGL"]._auth.AuthProvider_credential(Module.FirebaseWebGL._util.UTF8ToString(authProvider), Module.FirebaseWebGL._util.UTF8ToString(idToken), Module.FirebaseWebGL._util.UTF8ToString(accessToken)); 
	},
	AuthProvider_credentialFromResult_FirebaseWebGL_auth: function(authProvider, userCredential) {
		return Module["FirebaseWebGL"]._auth.AuthProvider_credentialFromResult(Module.FirebaseWebGL._util.UTF8ToString(authProvider), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(userCredential))); 
	},
	AuthProvider_credentialFromError_FirebaseWebGL_auth: function(authProvider, error) {
		return Module["FirebaseWebGL"]._auth.AuthProvider_credentialFromError(Module.FirebaseWebGL._util.UTF8ToString(authProvider), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(error))); 
	},
	AuthProvider_credentialFromJSON_FirebaseWebGL_auth: function(authProvider, json) {
		return Module["FirebaseWebGL"]._auth.AuthProvider_credentialFromJSON(Module.FirebaseWebGL._util.UTF8ToString(authProvider), Module.FirebaseWebGL._util.UTF8ToString(json)); 
	},
	signInWithCustomToken_FirebaseWebGL_auth: function(name, customToken, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.signInWithCustomToken(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(customToken), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	revokeAccessToken_FirebaseWebGL_auth: function(name, token, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.revokeAccessToken(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(token), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	signInWithCredential_FirebaseWebGL_auth: function(name, credential, credentialClass, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.signInWithCredential(Module.FirebaseWebGL._util.UTF8ToString(name), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(credential)), Module.FirebaseWebGL._util.UTF8ToString(credentialClass), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	signInWithPopup_FirebaseWebGL_auth: function(name, authProviderClass, provider, resolver, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.signInWithPopup(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(authProviderClass), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(provider)), resolver === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(resolver), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	signInWithRedirect_FirebaseWebGL_auth: function(name, authProviderClass, provider, resolver, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.signInWithRedirect(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(authProviderClass), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(provider)), resolver === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(resolver), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	getRedirectResult_FirebaseWebGL_auth: function(name, resolver, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.getRedirectResult(Module.FirebaseWebGL._util.UTF8ToString(name), resolver === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(resolver), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	linkWithCredential_FirebaseWebGL_auth: function(uid, credential, credentialClass, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.linkWithCredential(Module.FirebaseWebGL._util.UTF8ToString(uid), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(credential)), Module.FirebaseWebGL._util.UTF8ToString(credentialClass), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	linkWithPopup_FirebaseWebGL_auth: function(uid, authProviderClass, provider, resolver, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.linkWithPopup(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(authProviderClass), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(provider)), resolver === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(resolver), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	linkWithRedirect_FirebaseWebGL_auth: function(uid, authProviderClass, provider, resolver, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.linkWithRedirect(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(authProviderClass), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(provider)), resolver === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(resolver), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	linkWithPhoneNumber_FirebaseWebGL_auth: function(uid, phoneNumber, appVerifierId, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.linkWithPhoneNumber(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(phoneNumber), appVerifierId, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	reauthenticateWithCredential_FirebaseWebGL_auth: function(uid, credential, credentialClass, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.reauthenticateWithCredential(Module.FirebaseWebGL._util.UTF8ToString(uid), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(credential)), Module.FirebaseWebGL._util.UTF8ToString(credentialClass), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	reauthenticateWithPopup_FirebaseWebGL_auth: function(uid, authProviderClass, provider, resolver, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.reauthenticateWithPopup(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(authProviderClass), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(provider)), resolver === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(resolver), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	reauthenticateWithRedirect_FirebaseWebGL_auth: function(uid, authProviderClass, provider, resolver, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.reauthenticateWithRedirect(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(authProviderClass), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(provider)), resolver === 0 ? undefined : Module.FirebaseWebGL._util.UTF8ToString(resolver), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	reauthenticateWithPhoneNumber_FirebaseWebGL_auth: function(uid, phoneNumber, appVerifierId, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.reauthenticateWithPhoneNumber(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(phoneNumber), appVerifierId, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	OAuthProvider_credential_FirebaseWebGL_auth: function(oauth, parameters) {
		return Module["FirebaseWebGL"]._auth.OAuthProvider_credential(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(oauth)), Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(parameters))); 
	},
	getAdditionalUserInfo_FirebaseWebGL_auth: function(userCredential) {
		return Module["FirebaseWebGL"]._auth.getAdditionalUserInfo(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(userCredential))); 
	},
	checkActionCode_FirebaseWebGL_auth: function(name, oobCode, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.checkActionCode(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(oobCode), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	verifyPasswordResetCode_FirebaseWebGL_auth: function(name, code, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.verifyPasswordResetCode(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(code), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	setPersistence_FirebaseWebGL_auth: function(name, persistence, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.setPersistence(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(persistence), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	updateCurrentUser_FirebaseWebGL_auth: function(name, uid, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.updateCurrentUser(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(uid), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	updateEmail_FirebaseWebGL_auth: function(uid, newEmail, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.updateEmail(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(newEmail), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	updatePassword_FirebaseWebGL_auth: function(uid, newPassword, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.updatePassword(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(newPassword), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	updateProfile_FirebaseWebGL_auth: function(uid, profile, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.updateProfile(Module.FirebaseWebGL._util.UTF8ToString(uid), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(profile)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	updatePhoneNumber_FirebaseWebGL_auth: function(uid, credential, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.updatePhoneNumber(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(credential), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	validatePassword_FirebaseWebGL_auth: function(name, password, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.validatePassword(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(password), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	useDeviceLanguage_FirebaseWebGL_auth: function(name) {
		return Module["FirebaseWebGL"]._auth.useDeviceLanguage(Module.FirebaseWebGL._util.UTF8ToString(name));
	},
	verifyBeforeUpdateEmail_FirebaseWebGL_auth: function(uid, newEmail, actionCodeSettings, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.verifyBeforeUpdateEmail(Module.FirebaseWebGL._util.UTF8ToString(uid), Module.FirebaseWebGL._util.UTF8ToString(newEmail), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(actionCodeSettings)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	parseActionCodeURL_FirebaseWebGL_auth: function(link) {
		return Module["FirebaseWebGL"]._auth.parseActionCodeURL(Module.FirebaseWebGL._util.UTF8ToString(link));
	},
	authStateReady_FirebaseWebGL_auth: function(name, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.authStateReady(Module.FirebaseWebGL._util.UTF8ToString(name), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	recaptchaVerifierConstructor_FirebaseWebGL_auth: function(name, id, parameters, callbackId, callback, expiredCallbackId, expiredCallback, errorCallbackId, errorCallback) {
		return Module["FirebaseWebGL"]._auth.recaptchaVerifierConstructor(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(id), parameters === 0 ? null : JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(parameters)), Module.FirebaseWebGL._util.UTF8ToString(callbackId), callback === 0 ? null : callback, Module.FirebaseWebGL._util.UTF8ToString(expiredCallbackId), expiredCallback === 0 ? null : expiredCallback , Module.FirebaseWebGL._util.UTF8ToString(errorCallbackId), errorCallback === 0 ? null : errorCallback);
	},
	recaptchaVerify_FirebaseWebGL_auth: function(id, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.recaptchaVerify(Module.FirebaseWebGL._util.UTF8ToString(id), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	recaptchaRender_FirebaseWebGL_auth: function(id, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.recaptchaRender(Module.FirebaseWebGL._util.UTF8ToString(id), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	recaptchaClear_FirebaseWebGL_auth: function(id) {
		return Module["FirebaseWebGL"]._auth.recaptchaClear(Module.FirebaseWebGL._util.UTF8ToString(id));
	},
	recaptchaGetResponse_FirebaseWebGL_auth: function(widgetId) {
		return Module["FirebaseWebGL"]._auth.recaptchaGetResponse(Module.FirebaseWebGL._util.UTF8ToString(widgetId));
	},
	recaptchaReset_FirebaseWebGL_auth: function(widgetId) {
		return Module["FirebaseWebGL"]._auth.recaptchaReset(Module.FirebaseWebGL._util.UTF8ToString(widgetId));
	},
	recaptchaExecute_FirebaseWebGL_auth: function(widgetId, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.recaptchaExecute(Module.FirebaseWebGL._util.UTF8ToString(widgetId), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	initializeRecaptchaConfig_FirebaseWebGL_auth: function(name, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.initializeRecaptchaConfig(Module.FirebaseWebGL._util.UTF8ToString(name), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	signInWithPhoneNumber_FirebaseWebGL_auth: function(name, phoneNumber, appVerifierId, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.signInWithPhoneNumber(Module.FirebaseWebGL._util.UTF8ToString(name), Module.FirebaseWebGL._util.UTF8ToString(phoneNumber), appVerifierId, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	ConfirmResult_confirm_FirebaseWebGL_auth: function(id, verificationCode, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.ConfirmResult_confirm(id, Module.FirebaseWebGL._util.UTF8ToString(verificationCode), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	ConfirmResult_clear_FirebaseWebGL_auth: function(id) {
		Module["FirebaseWebGL"]._auth.ConfirmResult_clear(id);
	},
	PhoneAuthProvider_verifyPhoneNumber_FirebaseWebGL_auth: function(name, provider, appVerifierId, phoneOptions, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.PhoneAuthProvider_verifyPhoneNumber(Module.FirebaseWebGL._util.UTF8ToString(name), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(provider)), appVerifierId, Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(phoneOptions)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	AuthCredential_fromJSON_FirebaseWebGL_auth: function(authCredential, json) {
		return Module["FirebaseWebGL"]._auth.AuthCredential_fromJSON(Module.FirebaseWebGL._util.UTF8ToString(authCredential), Module.FirebaseWebGL._util.UTF8ToString(json));
	},
	PhoneMultiFactorGenerator_assertion_FirebaseWebGL_auth: function(credential) {
		return Module["FirebaseWebGL"]._auth.PhoneMultiFactorGenerator_assertion(Module.FirebaseWebGL._util.UTF8ToString(credential));
	},
	TotpMultiFactorGenerator_assertionForEnrollment_FirebaseWebGL_auth: function(secret, oneTimePassword) {
		return Module["FirebaseWebGL"]._auth.TotpMultiFactorGenerator_assertionForEnrollment(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(secret)), Module.FirebaseWebGL._util.UTF8ToString(oneTimePassword));
	},
	TotpMultiFactorGenerator_assertionForSignIn_FirebaseWebGL_auth: function(enrollmentId, oneTimePassword) {
		return Module["FirebaseWebGL"]._auth.TotpMultiFactorGenerator_assertionForSignIn(Module.FirebaseWebGL._util.UTF8ToString(enrollmentId), Module.FirebaseWebGL._util.UTF8ToString(oneTimePassword));
	},
	multiFactor_auth: function(uid) {
		return Module["FirebaseWebGL"]._auth.multiFactor(Module.FirebaseWebGL._util.UTF8ToString(uid));
	},
	MultiFactorUser_enroll_FirebaseWebGL_auth: function(id, multiFactorAssertionId, displayName, callback, taskCompletionSource, autoClear) {
		Module["FirebaseWebGL"]._auth.MultiFactorUser_enroll(id, multiFactorAssertionId, Module.FirebaseWebGL._util.UTF8ToString(displayName), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), autoClear === 0 ? false : true);
	},
	MultiFactorUser_getSession_FirebaseWebGL_auth: function(id, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.MultiFactorUser_getSession(id, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	MultiFactorUser_unenroll_FirebaseWebGL_auth: function(id, option, callback, taskCompletionSource, autoClear) {
		Module["FirebaseWebGL"]._auth.MultiFactorUser_unenroll(id, Module.FirebaseWebGL._util.JSONParse(Module.FirebaseWebGL._util.UTF8ToString(option)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), autoClear === 0 ? false : true);
	},
	MultiFactorUser_getEnrolledFactors_FirebaseWebGL_auth: function(id) {
		return Module["FirebaseWebGL"]._auth.MultiFactorUser_getEnrolledFactors(id);
	},
	MultiFactorUser_clear_FirebaseWebGL_auth: function(id) {
		Module["FirebaseWebGL"]._auth.MultiFactorUser_clear(id);
	},
	getMultiFactorResolver_FirebaseWebGL_auth: function(name, multiFactorError) {
		return Module["FirebaseWebGL"]._auth.getMultiFactorResolver(Module.FirebaseWebGL._util.UTF8ToString(name), JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(multiFactorError)));
	},
	MultiFactorResolver_resolveSignIn_FirebaseWebGL_auth: function(id, multiFactorAssertionId, callback, taskCompletionSource, autoClear) {
		Module["FirebaseWebGL"]._auth.MultiFactorResolver_resolveSignIn(id, multiFactorAssertionId, callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource), autoClear === 0 ? false : true);
	},
	MultiFactorResolver_clear_FirebaseWebGL_auth: function(id) {
		Module["FirebaseWebGL"]._auth.MultiFactorResolver_clear(id);
	},
	TotpMultiFactorGenerator_generateSecret_FirebaseWebGL_auth: function(session, callback, taskCompletionSource) {
		Module["FirebaseWebGL"]._auth.TotpMultiFactorGenerator_generateSecret(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(session)), callback, Module.FirebaseWebGL._util.UTF8ToString(taskCompletionSource));
	},
	TotpSecret_generateQrCodeUrl_FirebaseWebGL_auth: function(secret, accountName, issuer) {
		return Module["FirebaseWebGL"]._auth.TotpSecret_generateQrCodeUrl(JSON.parse(Module.FirebaseWebGL._util.UTF8ToString(secret)), Module.FirebaseWebGL._util.UTF8ToString(accountName), Module.FirebaseWebGL._util.UTF8ToString(issuer));
	}
});