using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    private AndroidJavaObject vibrator;

    //longueur de vibration
    private long milliseconds = 500;

    private long[] pattern = { 0, 200, 100, 300 };
    private int repetition = -1;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        }
    }

    //Simple vibration de base en fonction de l'appareil
    public void MakeVibration()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            vibrator.Call("vibrate", milliseconds);
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    //Création d'un pattern de vibration et le nombre de fois quelle se répète
    public void MakePatternVibration()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            vibrator.Call("vibrate", pattern, repetition);
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    //Annulation de la vibration
    public void CancelVibration()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            vibrator.Call("cancel");
        }
    }
}