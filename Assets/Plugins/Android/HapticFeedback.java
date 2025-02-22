package com.yourcompany.haptics;

import android.content.Context;
import android.os.VibrationEffect;
import android.os.Vibrator;
import com.unity3d.player.UnityPlayer;

public class HapticFeedback {
    private static Vibrator vibrator = (Vibrator) UnityPlayer.currentActivity.getSystemService(Context.VIBRATOR_SERVICE);

    public static void Vibrate(long milliseconds) {
        if (vibrator != null) {
            vibrator.vibrate(VibrationEffect.createOneShot(milliseconds, VibrationEffect.DEFAULT_AMPLITUDE));
        }
    }

    public static void TapVibration() {
        Vibrate(50); // Short vibration for tap
    }

    public static void DoubleTapVibration() {
        long[] pattern = { 0, 50, 50, 50 }; // Short vibration pattern for double tap
        vibrator.vibrate(VibrationEffect.createWaveform(pattern, -1)); // No repeat
    }
}
