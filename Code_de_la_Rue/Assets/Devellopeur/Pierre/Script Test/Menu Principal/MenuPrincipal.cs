using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{

    //[SerializeField] private GameObject panelBoutton;
    [SerializeField] private GameObject panelParametres;
    [SerializeField] private GameObject panelLexique;
    [SerializeField] private GameObject panelSelection;
    [SerializeField] private GameObject panelLieux;
       private void Start()
    {
        //Dans le cas ou le menu selection de niveaux est le "menu principal"
        panelSelection.SetActive(true);
        //panelBoutton.SetActive(false);
        panelParametres.SetActive(false);
        panelLexique.SetActive(false);
        panelLieux.SetActive(false);


        // Pour s'assurer que le menu principal soit toujours ouvert � l'ouverture de la scene ( au cas ou le panel soit desactivez dans l'editeur)
        //panelBoutton.SetActive(true);
        panelParametres.SetActive(false);
    }

    // M�thode pour charger la sc�ne
    public void OpenScene(string Scene)
    {
        // V�rifie que la scene existe
        if (!string.IsNullOrEmpty(Scene))
        {
            // Charge la sc�ne
            SceneManager.LoadScene(Scene);
        }
        else
        {
            Debug.LogWarning("Scene name is not assigned.");
        }
    }

    public void OpenParametres()
    {
        panelParametres.SetActive(true);
        //panelBoutton.SetActive(false);
        panelLexique.SetActive(false);
        panelSelection.SetActive(false);
        panelLieux.SetActive(false);
    }

    public void AppliquerParametres()
    {
        //panelBoutton.SetActive(true);
        panelParametres.SetActive(false);
        panelLexique.SetActive(false);
        panelSelection.SetActive(false);
        panelLieux.SetActive(false);

        // Mettre en place la sauvegarde et l'application des param�tres
    }

    public void OpenLexique()
    {
        panelLexique.SetActive(true);
        //panelBoutton.SetActive(false);
        panelParametres.SetActive(false);
        panelSelection.SetActive(false);
        panelLieux.SetActive(false);
    }

    public void OpenSelection()
    {
        panelSelection.SetActive(true);
        //panelBoutton.SetActive(false);
        panelParametres.SetActive(false);
        panelLexique.SetActive(false);
        panelLieux.SetActive(false);
    }

    public void OpenLieux()
    {
        panelLieux.SetActive(true);
        //panelBoutton.SetActive(false);
        panelParametres.SetActive(false);
        panelLexique.SetActive(false);
        panelSelection.SetActive(false);
    }
}