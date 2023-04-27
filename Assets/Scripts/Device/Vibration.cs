namespace Kumi.Device
{
    public static class Vibration
    {
        public static bool Enabled = true;

        public static void Vibrate(long milliseconds, int amplitude = -1, bool cancel = false)
        {
            if (Enabled == false) return;
            #if UNITY_ANDROID
            Android.Vibration.Vibrate(milliseconds, amplitude, cancel);
            #endif
        }
    }
}
