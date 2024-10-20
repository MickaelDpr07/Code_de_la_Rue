using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VectorGraphics;

public class Contexte : MonoBehaviour
{
    [SerializeField]
    private GameObject PopUp, Reponse; // Le prefab � ouvrir

    [SerializeField]
    public string texte1, texte2; // Variable pour les diff�rents textes

    [SerializeField]
    public Button BTN_R�ponse; // Bouton de r�ponse correct

    private Button selectedButton; // Bouton actuellement s�lectionn�

    private TMP_Text txt_Contexte; // Texte � afficher

    //Element relatif � l'�l�ment manquant
    public GameObject R�solution;
    public SVGImage Img_R�solution;
    public bool completed = false;
    public SVGImage img_afficher;

    public GameManager GameManager;

    void Start()
    {
        OpenPopUp();
    }

    // Fonction appel�e lorsque le bouton est cliqu�
    public void OpenPopUp()
    {
        Debug.Log("oui");
        if (PopUp != null)
        {
            // Affiche ou cache le popup en fonction de son �tat actuel
            bool isActive = PopUp.activeSelf; // V�rifie si le popup est actuellement actif
            PopUp.SetActive(!isActive); // Inverse l'�tat

            if (PopUp.activeSelf) // Si le popup est maintenant actif
            {
                // Cherche l'enfant avec le nom "Txt_contexte"
                Transform txtContexteTransform = PopUp.transform.Find("Txt_contexte");

                // V�rifie si l'�l�ment existe et applique le texte
                if (txtContexteTransform != null)
                {
                    // R�cup�rer le composant TMP_Text et appliquer le texte
                    TMP_Text txtContexte = txtContexteTransform.GetComponent<TMP_Text>();
                    txt_Contexte = txtContexte;

                    if (txtContexte != null)
                    {
                        if (!completed)
                        {
                            txtContexte.text = texte1; // Assigner texte1 au TextMeshPro
                        }
                        else
                        {
                            txtContexte.text = texte2; // Assigner texte1 au TextMeshPro
                        }

                    }
                    else
                    {
                        Debug.LogError("Le composant TMP_Text n'a pas �t� trouv� sur 'Txt_contexte'.");
                    }
                }
                else
                {
                    Debug.LogError("L'�l�ment 'Txt_contexte' n'a pas �t� trouv� dans le prefab.");
                }
            }
        }
        else
        {
            Debug.LogError("Le prefab n'est pas assign�.");
        }
    }

    public void ClosePopUp()
    {
        PopUp.SetActive(false);
    }

    public void OpenR�ponse()
    {
        Reponse.SetActive(true);
    }

    // M�thode pour s�lectionner une r�ponse, � appliquer sur les �v�ments btnclick des choix de r�ponses
    public void SelectButton(Button button)
    {
        selectedButton = button; // Enregistrer le bouton s�lectionn�

    }

    // M�thode pour valider la r�ponse
    public void ValiderR�ponse()
    {
        if (selectedButton == BTN_R�ponse)
        {
            // Cherche l'enfant avec le nom "Txt_contexte"
            Transform txtContexteTransform = PopUp.transform.Find("Txt_contexte");

            // V�rifie si l'�l�ment existe et applique le texte
            if (txtContexteTransform != null)
            {
                // R�cup�rer le composant TMP_Text et appliquer le texte
                TMP_Text txtContexte = txtContexteTransform.GetComponent<TMP_Text>();
                Debug.Log("R�ponse correcte !" + selectedButton);

                GameManager.NbrPuzzleR�ussi++;
                GameManager.CheckPuzzle();
                Reponse.SetActive(false);
                PopUp.SetActive(true);
                txtContexte.text = texte2;
                R�solution.SetActive(false);
                completed = true;
                img_afficher.sprite = Img_R�solution.sprite;
            }
        }
        else
        {
            Debug.Log("R�ponse incorrecte !"); 
            Reponse.SetActive(false);
            PopUp.SetActive(true);

            // Afficher un message d'erreur dans le texte contextuel
            if (txt_Contexte != null)
            {
                txt_Contexte.text = "Je ne suis pas s�r que �a m'aide dans cette situation..";
            }
            else
            {
                Debug.LogError("Le texte contextuel est nul lors de la r�ponse incorrecte.");
            }
        }
    }
}