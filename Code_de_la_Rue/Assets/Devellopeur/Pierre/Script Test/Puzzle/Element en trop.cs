using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VectorGraphics;

public class Elemententrop : MonoBehaviour
{
    [SerializeField]
    private GameObject PopUp;

    [SerializeField]
    public string texte1, texte2; // Variable pour les différents textes

    private TMP_Text txt_Contexte; // Texte à afficher

    [SerializeField]
    public GameObject[] ElementsEnTrop; // Variable pour les différents éléments en trop
    private GameObject[] ElementsSelectionnées; // Variable pour stockés les éléments séléctionnées

    void Start()
    {
        OnOpenPrefabButtonClick();
    }

    void Update()
    {

    }
    public void OnOpenPrefabButtonClick()
    {
        Debug.Log("oui");
        if (PopUp != null)
        {
            // Affiche ou cache le popup en fonction de son état actuel
            bool isActive = PopUp.activeSelf; // Vérifie si le popup est actuellement actif
            PopUp.SetActive(!isActive); // Inverse l'état
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
                        txtContexte.text = texte1; // Assigner texte1 au TextMeshPro
                 

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

        else
        {
            Debug.LogError("Le prefab n'est pas assigné.");
        }
    }

        public void OnClosePrefabButtonClick()
        {
            PopUp.SetActive(false);
        }

    public void OnClickElement()
    {
        //Si il n'est pas séléctionnée, l'ajoute à la liste et change la couleur

        // Compare avec la liste elementen trop

        //Si sa correspond, affiche le dialogue résolu
    }
}
