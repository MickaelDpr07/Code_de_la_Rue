using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;

    void Awake()
    {
        if (isAndroid())
        {
            // Obtenir la classe Vibrator à partir du contexte Android
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        }
    }

    public void Cancel()
    {
        if (isAndroid() && vibrator != null)
            vibrator.Call("cancel");
    }

    public bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }

    public void MakeVibration()
    {
        if (isAndroid() && vibrator != null)
        {
            Debug.Log("Android");
            vibrator.Call("vibrate", 500);
        }
        else
        {
            Debug.Log("pas un android");
            Handheld.Vibrate();
        }
    }
}