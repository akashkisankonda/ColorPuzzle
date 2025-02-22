using UnityEngine;

public class HapticManager : MonoBehaviour
{
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject currentActivity;
    private AndroidJavaClass hapticClass;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                hapticClass = new AndroidJavaClass("com.yourcompany.haptics.HapticFeedback");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to initialize AndroidJavaClass or get currentActivity: " + e.Message);
            }
        }
    }

    public void TapVibration()
    {
        if (Application.isEditor)
        {
            Debug.Log("Tap vibration simulated.");
        }
        else
        {
            try
            {
                hapticClass.CallStatic("TapVibration");
            }
            catch (System.Exception e)
            {
                Debug.LogError("TapVibration error: " + e.Message);
            }
        }
    }

    public void DoubleTapVibration()
    {
        if (Application.isEditor)
        {
            Debug.Log("Double-tap vibration simulated.");
        }
        else
        {
            try
            {
                hapticClass.CallStatic("DoubleTapVibration");
            }
            catch (System.Exception e)
            {
                Debug.LogError("DoubleTapVibration error: " + e.Message);
            }
        }
    }
}
