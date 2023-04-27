// https://gist.githubusercontent.com/ruzrobert/d98220a3b7f71ccc90403e041967c46b/raw/2e5cbd94f581d2b9b00b9003ca92a17a8c2cd8c7/Vibration.cs

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Kumi.Device.Android
{
    /// <summary>
    /// Class for controlling Vibration. Automatically initializes before scene is loaded.
    /// </summary>
    public static class Vibration
    {
        // Component Parameters
        public const LOGLevel LogLevel = LOGLevel.Disabled;

        // Vibrator References
        static AndroidJavaObject vibrator;
        static AndroidJavaClass vibrationEffectClass;
        static int defaultAmplitude = 255;

        // Api Level
        static int apiLevel = 1;
        static bool DoesSupportVibrationEffect () => apiLevel >= 26;    // available only from Api >= 26
        static bool DoesSupportPredefinedEffect () => apiLevel >= 29;   // available only from Api >= 29

        #region Initialization

        static bool isInitialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        [SuppressMessage("Code quality", "IDE0051", Justification = "Called on scene load")]
        static void Initialize ()
        {
            // Add APP VIBRATION PERMISSION to the Manifest
            #if UNITY_ANDROID
            if (Application.isConsolePlatform) { Handheld.Vibrate(); }
            #endif

            switch (isInitialized)
            {
                // load references safely
                case false when Application.platform == RuntimePlatform.Android:
                {
                    // Get Api Level
                    using (AndroidJavaClass androidVersionClass = new("android.os.Build$VERSION")) {
                        apiLevel = androidVersionClass.GetStatic<int>("SDK_INT");
                    }

                    // Get UnityPlayer and CurrentActivity
                    using (AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer"))
                    using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                        if (currentActivity != null) {
                            vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

                            // if device supports vibration effects, get corresponding class
                            if (DoesSupportVibrationEffect()) {
                                vibrationEffectClass = new("android.os.VibrationEffect");
                                defaultAmplitude = Mathf.Clamp(vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE"), 1, 255);
                            }

                            // if device supports predefined effects, get their IDs
                            if (DoesSupportPredefinedEffect()) {
                                PredefinedEffect.EffectClick = vibrationEffectClass.GetStatic<int>("EFFECT_CLICK");
                                PredefinedEffect.EffectDoubleClick = vibrationEffectClass.GetStatic<int>("EFFECT_DOUBLE_CLICK");
                                PredefinedEffect.EffectHeavyClick = vibrationEffectClass.GetStatic<int>("EFFECT_HEAVY_CLICK");
                                PredefinedEffect.EffectTick = vibrationEffectClass.GetStatic<int>("EFFECT_TICK");
                            }
                        }
                    }

                    LOGAuto("Vibration component initialized", LOGLevel.Info);
                    isInitialized = true;
                    break;
                }
            }
        }
        #endregion

        #region Vibrate Public
        /// <summary>
        /// Vibrate for Milliseconds, with Amplitude (if available).
        /// If amplitude is -1, amplitude is Disabled. If -1, device DefaultAmplitude is used. Otherwise, values between 1-255 are allowed.
        /// If 'cancel' is true, Cancel() will be called automatically.
        /// </summary>
        public static void Vibrate (long milliseconds, int amplitude = -1, bool cancel = false)
        {
            string FuncToStr () => $"Vibrate ({milliseconds}, {amplitude}, {cancel})";

            Initialize(); // make sure script is initialized
            if (isInitialized == false) {
                LOGAuto(FuncToStr() + ": Not initialized", LOGLevel.Warning);
            }
            else if (HasVibrator() == false) {
                LOGAuto(FuncToStr() + ": Device doesn't have Vibrator", LOGLevel.Warning);
            }
            else {
                if (cancel) Cancel();
                if (DoesSupportVibrationEffect()) {
                    // validate amplitude
                    amplitude = Mathf.Clamp(amplitude, -1, 255);
                    if (amplitude == -1) amplitude = 255; // if -1, disable amplitude (use maximum amplitude)
                    if (amplitude != 255 && HasAmplitudeControl() == false) { // if amplitude was set, but not supported, notify developer
                        LOGAuto(FuncToStr() + ": Device doesn't have Amplitude Control, but Amplitude was set", LOGLevel.Warning);
                    }
                    if (amplitude == 0) amplitude = defaultAmplitude; // if 0, use device DefaultAmplitude

                    // if amplitude is not supported, use 255; if amplitude is -1, use systems DefaultAmplitude. Otherwise use user-defined value.
                    amplitude = HasAmplitudeControl() == false ? 255 : amplitude;
                    VibrateEffect(milliseconds, amplitude);
                    LOGAuto(FuncToStr() + ": Effect called", LOGLevel.Info);
                }
                else {
                    VibrateLegacy(milliseconds);
                    LOGAuto(FuncToStr() + ": Legacy called", LOGLevel.Info);
                }
            }
        }
        /// <summary>
        /// Vibrate Pattern (pattern of durations, with format Off-On-Off-On and so on).
        /// Amplitudes can be Null (for default) or array of Pattern array length with values between 1-255.
        /// To cause the pattern to repeat, pass the index into the pattern array at which to start the repeat, or -1 to disable repeating.
        /// If 'cancel' is true, Cancel() will be called automatically.
        /// </summary>
        public static void Vibrate (long[] pattern, int[] amplitudes = null, int repeat = -1, bool cancel = false)
        {
            string FuncToStr () => $"Vibrate (({ArrToStr(pattern)}), ({ArrToStr(amplitudes)}), {repeat}, {cancel})";

            Initialize(); // make sure script is initialized
            if (isInitialized == false) {
                LOGAuto(FuncToStr() + ": Not initialized", LOGLevel.Warning);
            }
            else if (HasVibrator() == false) {
                LOGAuto(FuncToStr() + ": Device doesn't have Vibrator", LOGLevel.Warning);
            }
            else {
                // check Amplitudes array length
                if (amplitudes != null && amplitudes.Length != pattern.Length) {
                    LOGAuto(FuncToStr() + ": Length of Amplitudes array is not equal to Pattern array. Amplitudes will be ignored.", LOGLevel.Warning);
                    amplitudes = null;
                }
                // limit amplitudes between 1 and 255
                if (amplitudes != null) {
                    ClampAmplitudesArray(amplitudes);
                }

                // vibrate
                if (cancel) Cancel();
                if (DoesSupportVibrationEffect()) {
                    if (amplitudes != null && HasAmplitudeControl() == false) {
                        LOGAuto(FuncToStr() + ": Device doesn't have Amplitude Control, but Amplitudes was set", LOGLevel.Warning);
                        amplitudes = null;
                    }
                    if (amplitudes != null) {
                        VibrateEffect(pattern, amplitudes, repeat);
                        LOGAuto(FuncToStr() + ": Effect with amplitudes called", LOGLevel.Info);
                    }
                    else {
                        VibrateEffect(pattern, repeat);
                        LOGAuto(FuncToStr() + ": Effect called", LOGLevel.Info);
                    }
                }
                else {
                    VibrateLegacy(pattern, repeat);
                    LOGAuto(FuncToStr() + ": Legacy called", LOGLevel.Info);
                }
            }
        }

        /// <summary>
        /// Vibrate predefined effect (described in Vibration.PredefinedEffect). Available from Api Level >= 29.
        /// If 'cancel' is true, Cancel() will be called automatically.
        /// </summary>
        public static void VibratePredefined (int effectId, bool cancel = false)
        {
            string FuncToStr () => $"VibratePredefined ({effectId})";

            Initialize(); // make sure script is initialized
            if (isInitialized == false) {
                LOGAuto(FuncToStr() + ": Not initialized", LOGLevel.Warning);
            }
            else if (HasVibrator() == false) {
                LOGAuto(FuncToStr() + ": Device doesn't have Vibrator", LOGLevel.Warning);
            }
            else if (DoesSupportPredefinedEffect() == false) {
                LOGAuto(FuncToStr() + ": Device doesn't support Predefined Effects (Api Level >= 29)", LOGLevel.Warning);
            }
            else {
                if (cancel) Cancel();
                VibrateEffectPredefined(effectId);
                LOGAuto(FuncToStr() + ": Predefined effect called", LOGLevel.Info);
            }
        }
        #endregion

        #region Public Properties & Controls
        public static long[] ParsePattern (string pattern)
        {
            if (pattern == null) return Array.Empty<long>();
            pattern = pattern.Trim();
            string[] split = pattern.Split(',');

            long[] timings = new long[split.Length];
            for (int i = 0; i < split.Length; i++) {
                if (int.TryParse(split[i].Trim(), out int duration)) {
                    timings[i] = duration < 0 ? 0 : duration;
                }
                else {
                    timings[i] = 0;
                }
            }

            return timings;
        }

        /// <summary>
        /// Returns Android Api Level
        /// </summary>
        public static int GetApiLevel () => apiLevel;
        /// <summary>
        /// Returns Default Amplitude of device, or 0.
        /// </summary>
        public static int GetDefaultAmplitude () => defaultAmplitude;

        /// <summary>
        /// Returns true if device has vibrator
        /// </summary>
        public static bool HasVibrator ()
        {
            return vibrator != null && vibrator.Call<bool>("hasVibrator");
        }
        /// <summary>
        /// Return true if device supports amplitude control
        /// </summary>
        public static bool HasAmplitudeControl ()
        {
            if (HasVibrator() && DoesSupportVibrationEffect()) {
                return vibrator.Call<bool>("hasAmplitudeControl"); // API 26+ specific
            }

            return false; // no amplitude control below API level 26
        }

        /// <summary>
        /// Tries to cancel current vibration
        /// </summary>
        public static void Cancel ()
        {
            if (!HasVibrator()) return;
            vibrator.Call("cancel");
            LOGAuto("Cancel (): Called", LOGLevel.Info);
        }
        #endregion

        #region Vibrate Internal
        #region Vibration Callers

        static void VibrateEffect (long milliseconds, int amplitude)
        {
            using AndroidJavaObject effect = createEffect_OneShot(milliseconds, amplitude);
            vibrator.Call("vibrate", effect);
        }

        static void VibrateLegacy (long milliseconds)
        {
            vibrator.Call("vibrate", milliseconds);
        }

        static void VibrateEffect (long[] pattern, int repeat)
        {
            using AndroidJavaObject effect = createEffect_Waveform(pattern, repeat);
            vibrator.Call("vibrate", effect);
        }

        static void VibrateLegacy (long[] pattern, int repeat)
        {
            vibrator.Call("vibrate", pattern, repeat);
        }

        static void VibrateEffect (long[] pattern, int[] amplitudes, int repeat)
        {
            using AndroidJavaObject effect = createEffect_Waveform(pattern, amplitudes, repeat);
            vibrator.Call("vibrate", effect);
        }

        static void VibrateEffectPredefined (int effectId)
        {
            using AndroidJavaObject effect = createEffect_Predefined(effectId);
            vibrator.Call("vibrate", effect);
        }
        #endregion

        #region Vibration Effect
        /// <summary>
        /// Wrapper for public static VibrationEffect createOneShot (long milliseconds, int amplitude). API >= 26
        /// </summary>
        static AndroidJavaObject createEffect_OneShot (long milliseconds, int amplitude)
        {
            return vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude);
        }
        /// <summary>
        /// Wrapper for public static VibrationEffect createPredefined (int effectId). API >= 29
        /// </summary>
        static AndroidJavaObject createEffect_Predefined (int effectId)
        {
            return vibrationEffectClass.CallStatic<AndroidJavaObject>("createPredefined", effectId);
        }
        /// <summary>
        /// Wrapper for public static VibrationEffect createWaveform (long[] timings, int[] amplitudes, int repeat). API >= 26
        /// </summary>
        static AndroidJavaObject createEffect_Waveform (long[] timings, int[] amplitudes, int repeat)
        {
            return vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", timings, amplitudes, repeat);
        }
        /// <summary>
        /// Wrapper for public static VibrationEffect createWaveform (long[] timings, int repeat). API >= 26
        /// </summary>
        static AndroidJavaObject createEffect_Waveform (long[] timings, int repeat)
        {
            return vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", timings, repeat);
        }
        #endregion
        #endregion

        #region Internal

        static void LOGAuto (string text, LOGLevel level)
        {
            if (level == LOGLevel.Disabled) level = LOGLevel.Info;
            if (text == null) return;
            switch (level)
            {
                case LOGLevel.Warning when LogLevel == LOGLevel.Warning:
                    Debug.LogWarning(text);
                    break;
                case LOGLevel.Info when LogLevel >= LOGLevel.Info:
                    Debug.Log(text);
                    break;
            }
        }

        static string ArrToStr (long[] array) => array == null ? "null" : string.Join(", ", array);
        static string ArrToStr (int[] array) => array == null ? "null" : string.Join(", ", array);

        static void ClampAmplitudesArray (int[] amplitudes)
        {
            for (int i = 0; i < amplitudes.Length; i++) {
                amplitudes[i] = Mathf.Clamp(amplitudes[i], 1, 255);
            }
        }
        #endregion

        public static class PredefinedEffect
        {
            public static int EffectClick;         // public static final int EFFECT_CLICK
            public static int EffectDoubleClick;  // public static final int EFFECT_DOUBLE_CLICK
            public static int EffectHeavyClick;   // public static final int EFFECT_HEAVY_CLICK
            public static int EffectTick;          // public static final int EFFECT_TICK
        }
        public enum LOGLevel
        {
            Disabled,
            Info,
            Warning
        }
    }
}