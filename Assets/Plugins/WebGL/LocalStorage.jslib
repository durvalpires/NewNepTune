mergeInto(LibraryManager.library, {
    SaveToLocalStorage: function(key, value) {
        console.log("[JS] SaveToLocalStorage: Key =", UTF8ToString(key), "Value Length =", UTF8ToString(value).length, "Value =", UTF8ToString(value));
        localStorage.setItem(UTF8ToString(key), UTF8ToString(value));
        console.log("[JS] Saved Value in LocalStorage:", localStorage.getItem(UTF8ToString(key)));
    },

    LoadFromLocalStorage: function(key, buffer, length) {
        length = Number(length);
        var keyStr = UTF8ToString(key);
        var value = localStorage.getItem(keyStr) || "";
        if (value.length >= length) {
            console.warn("[JS] WARNING: Data size exceeds buffer! Data might be truncated.");
        }
        stringToUTF8(value, buffer, length);
    },

    RemoveFromLocalStorage: function(key) {
        var keyStr = UTF8ToString(key);
        console.log("[JS] RemoveFromLocalStorage: Key =", keyStr);
        localStorage.removeItem(keyStr);
    },

    // Clear all entries in browser localStorage
    ClearAllFromLocalStorage: function() {
        console.log("[JS] ClearAllFromLocalStorage()");
        localStorage.clear();
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
