using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{

    [SerializeField] private GameObject panelBoutton;
    [SerializeField] private GameObject panelParametres;
    [SerializeField] private GameObject panelLexique;
    [SerializeField] private GameObject panelSelection;

    private void Start()
    {
        // Pour s'assurer que le menu principal soit toujours ouvert à l'ouverture de la scene ( au cas ou le panel soit desactivez dans l'editeur)
        panelBoutton.SetActive(true);
        panelParametres.SetActive(false);
    }

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

    public void OpenParamètres()
    {
        panelBoutton.SetActive(false);
        panelParametres.SetActive(true);
    }

    public void AppliquerParametres()
    {
        panelBoutton.SetActive(true);
        panelParametres.SetActive(false);

        // Mettre en place la sauvegarde et l'application des paramètres
    }

    public void OpenLexique()
    {
        panelBoutton.SetActive(false);
        panelLexique.SetActive(true);
    }

    public void OpenSelection()
    {
        panelBoutton.SetActive(false);
        panelSelection.SetActive(true);
    }
}