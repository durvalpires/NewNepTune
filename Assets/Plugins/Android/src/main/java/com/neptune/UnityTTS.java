package com.neptune;

import android.app.Activity;
import android.speech.tts.TextToSpeech;
import android.speech.tts.UtteranceProgressListener;
import com.unity3d.player.UnityPlayer;

import java.util.Locale;

public class UnityTTS {
    private TextToSpeech tts;
    private static UnityTTS instance;

    private UnityTTS() {
        Activity act = UnityPlayer.currentActivity;
        tts = new TextToSpeech(act, status -> {
            if (status == TextToSpeech.SUCCESS) {
                tts.setLanguage(Locale.getDefault());
                tts.setOnUtteranceProgressListener(new UtteranceProgressListener() {
                    @Override
                    public void onStart(String utteranceId) { }
                    @Override
                    public void onError(String utteranceId) { }
                    @Override
                    public void onDone(String utteranceId) {
                        // Notify Unity that this utterance finished:
                        UnityPlayer.UnitySendMessage(
                            "PostureRuleApplier",  // GameObject name
                            "OnTtsDone",          // method in PostureRuleApplier
                            utteranceId           // arbitrary payload, unused
                        );
                    }
                });
            }
        });
    }

    public static UnityTTS getInstance() {
        if (instance == null) instance = new UnityTTS();
        return instance;
    }

    public void speak(String text) {
        // Use same utteranceId as listener expects
        tts.speak(text, TextToSpeech.QUEUE_FLUSH, null, "NeptuneTTS");
    }
}