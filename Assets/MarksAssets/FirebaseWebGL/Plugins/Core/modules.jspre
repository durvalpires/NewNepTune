(
	function() {
		var currentVersion = '10.7.1';//https://firebase.google.com/support/release-notes/js
		window["FirebaseWebGL"] = {
			root: `https://www.gstatic.com/firebasejs/${currentVersion}`,
			enableProducts : {
				analytics:     true,
				appCheck:      false,
				auth:		   true,
				database:      true,
				firestore:	   false,
				firestoreLite: false,
				functions:     false,
				installations: false,
				messaging:     false,
				performance:   false,
				remoteConfig:  false,
				storage:       false,
			}
		}
})();