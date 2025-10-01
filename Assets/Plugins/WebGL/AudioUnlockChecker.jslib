mergeInto(LibraryManager.library, {
  CheckAudioUnlocked: function () {
    return window.neptuneAudioUnlocked ? 1 : 0; // Return int instead of JS boolean
  }
});
