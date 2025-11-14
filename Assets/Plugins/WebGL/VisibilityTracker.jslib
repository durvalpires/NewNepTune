mergeInto(LibraryManager.library, {
    RegisterVisibilityCallback: function(gameObjectNamePtr) {
        var gameObjectName = UTF8ToString(gameObjectNamePtr);
        console.log("[VisibilityTracker] Registering callbacks for GameObject:", gameObjectName);

        // Page Visibility API - Detect tab visibility changes
        document.addEventListener('visibilitychange', function() {
            if (document.hidden) {
                console.log("[VisibilityTracker] Tab is now HIDDEN");
                try {
                    SendMessage(gameObjectName, 'OnBrowserTabHidden');
                } catch (e) {
                    console.error("[VisibilityTracker] Error sending OnBrowserTabHidden:", e);
                }
            } else {
                console.log("[VisibilityTracker] Tab is now VISIBLE");
                try {
                    SendMessage(gameObjectName, 'OnBrowserTabVisible');
                } catch (e) {
                    console.error("[VisibilityTracker] Error sending OnBrowserTabVisible:", e);
                }
            }
        });

        // beforeunload - Detect browser/tab closing
        window.addEventListener('beforeunload', function(event) {
            console.log("[VisibilityTracker] Browser/tab is CLOSING");
            try {
                SendMessage(gameObjectName, 'OnBrowserClosing');
            } catch (e) {
                console.error("[VisibilityTracker] Error sending OnBrowserClosing:", e);
            }
        });

        // pagehide - Additional safety for mobile browsers and navigation
        window.addEventListener('pagehide', function(event) {
            console.log("[VisibilityTracker] Page is HIDING (navigation or close)");
            try {
                SendMessage(gameObjectName, 'OnBrowserClosing');
            } catch (e) {
                console.error("[VisibilityTracker] Error sending OnBrowserClosing (pagehide):", e);
            }
        });

        console.log("[VisibilityTracker] All callbacks registered successfully");
    }
});
