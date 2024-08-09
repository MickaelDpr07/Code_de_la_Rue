using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ChangementPage : MonoBehaviour
{

    // Méthode pour charger la scène
    public void OpenScene(string Scene)
    {
        // Vérifie que la scene existe
        if (!string.IsNullOrEmpty(Scene))
        {
            // Charge la scène
            SceneManager.LoadScene(Scene);
        }
        else
        {
            Debug.LogWarning("Scene name is not assigned.");
        }
    }
}