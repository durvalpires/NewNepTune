using UnityEngine;

public static class NeptuneTTS
{
#if UNITY_ANDROID && !UNITY_EDITOR
  private static AndroidJavaObject _tts;
  public static void Speak(string text) {
    if (_tts == null) {
      using var cls = new AndroidJavaClass("com.neptune.UnityTTS");
      _tts = cls.CallStatic<AndroidJavaObject>("getInstance");
    }
    _tts.Call("speak", text);
  }
#else
    public static void Speak(string text) { }
#endif
}