mergeInto(LibraryManager.library, {
    
	SaveToLocalStorage: function(key, value) {

		console.log("[JS] SaveToLocalStorage: Key =", UTF8ToString(key), "Value Length =", UTF8ToString(value).length, "Value =", UTF8ToString(value));
		localStorage.setItem(UTF8ToString(key), UTF8ToString(value));
		console.log("[JS] Saved Value in LocalStorage:", localStorage.getItem(UTF8ToString(key)));
    	},
    
	LoadFromLocalStorage: function(key, buffer, length) {
		console.log("[JS] BEFORE Corrected bufferLength log...");
		length = Number(length);  // Ensure it's read as a full integer
		setTimeout(() => console.log("[JS] Corrected bufferLength:", length), 0);
		console.log("[JS] AFTER Corrected bufferLength log...");
		var keyStr = UTF8ToString(key);
		var value = localStorage.getItem(keyStr) || "";
	
		console.log("[JS] LoadFromLocalStorage: Key =", keyStr, "Stored Data Length =", value.length, "Buffer Length =", length);

		if (value.length >= length) {
			console.warn("[JS] WARNING: Data size exceeds buffer! Data might be truncated.");
		}

		console.log("[JS] Writing data to buffer:", value);

		// Write value to Unity buffer
		stringToUTF8(value, buffer, length);

		console.log("[JS] Buffer after writing:", HEAPU8.subarray(buffer, buffer + value.length));
    },

	RemoveFromLocalStorage: function(key) {

		var keyStr = UTF8ToString(key);
		console.log("[JS] RemoveFromLocalStorage: Key =", keyStr);
		localStorage.removeItem(keyStr);
	},

	// MEMORY CLEANUP FUNCTION
	ClearMemory: function() {

		console.log("[JS] Clearing WebGL Memory...");
		if (Module && Module.destroy) {
				Module.destroy();
		}
		if (Module && Module.memory) {
				Module.memory.grow(0);
		}
	}
});