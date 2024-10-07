using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VectorGraphics;

public class Contexte : MonoBehaviour
{
    [SerializeField]
    private GameObject PopUp, Reponse; // Le prefab à ouvrir

    [SerializeField]
    public string texte1, texte2; // Variable pour les différents textes

    [SerializeField]
    public Button BTN_Réponse; // Bouton de réponse correct

    private Button selectedButton; // Bouton actuellement sélectionné

    private TMP_Text txt_Contexte; // Texte à afficher

    //Element relatif à l'élément manquant
    public GameObject Résolution;
    public SVGImage Img_Résolution;
    private bool Elementmanquantreussi = false;
    public SVGImage img_afficher;

    void Start()
    {
        OnOpenPrefabButtonClick();
    }

    // Fonction appelée lorsque le bouton est cliqué
    public void OnOpenPrefabButtonClick()
    {
        Debug.Log("oui");
        if (PopUp != null)
        {
            // Affiche ou cache le popup en fonction de son état actuel
            bool isActive = PopUp.activeSelf; // Vérifie si le popup est actuellement actif
            PopUp.SetActive(!isActive); // Inverse l'état

            if (PopUp.activeSelf) // Si le popup est maintenant actif
            {
                // Cherche l'enfant avec le nom "Txt_contexte"
                Transform txtContexteTransform = PopUp.transform.Find("Txt_contexte");

                // Vérifie si l'élément existe et applique le texte
                if (txtContexteTransform != null)
                {
                    // Récupérer le composant TMP_Text et appliquer le texte
                    TMP_Text txtContexte = txtContexteTransform.GetComponent<TMP_Text>();
                    txt_Contexte = txtContexte;

                    if (txtContexte != null)
                    {
                        if(!Elementmanquantreussi)
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
                        Debug.LogError("Le composant TMP_Text n'a pas été trouvé sur 'Txt_contexte'.");
                    }
                }
                else
                {
                    Debug.LogError("L'élément 'Txt_contexte' n'a pas été trouvé dans le prefab.");
                }
            }
        }
        else
        {
            Debug.LogError("Le prefab n'est pas assigné.");
        }
    }

    public void OnClosePrefabButtonClick()
    {
        PopUp.SetActive(false);
    }

    public void OpenRéponse()
    {
        Reponse.SetActive(true);
    }

    // Méthode pour sélectionner une réponse, à appliquer sur les évêments btnclick des choix de réponses
    public void SelectButton(Button button)
    {
        selectedButton = button; // Enregistrer le bouton sélectionné
    }

    // Méthode pour valider la réponse
    public void ValiderRéponse()
    {
        if (selectedButton == BTN_Réponse)
        {
            Debug.Log("Réponse correcte !" + selectedButton);
            Reponse.SetActive(false);
            PopUp.SetActive(true);
            txt_Contexte.text = texte2;
            Résolution.SetActive(false);
            Elementmanquantreussi = true;
            img_afficher.sprite = Img_Résolution.sprite;
        }
        else
        {
            Debug.Log("Réponse incorrecte !");
            Reponse.SetActive(false);
            PopUp.SetActive(true);
            txt_Contexte.text = "Je ne suis pas sur que ça m'aide dans cette situation..";
        }
    }
}