using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectLink : MonoBehaviour
{
    public string URL;

    public void OpenUrl()
    {
        Application.OpenURL(URL);
    }
}
