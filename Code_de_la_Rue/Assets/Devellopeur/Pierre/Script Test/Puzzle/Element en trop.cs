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
    public string texte1, texte2; // Variable pour les diff�rents textes

    private TMP_Text txt_Contexte; // Texte � afficher

    [SerializeField]
    public GameObject[] ElementsEnTrop; // Variable pour les diff�rents �l�ments en trop
    private GameObject[] ElementsSelectionn�es; // Variable pour stock�s les �l�ments s�l�ctionn�es

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
            // Affiche ou cache le popup en fonction de son �tat actuel
            bool isActive = PopUp.activeSelf; // V�rifie si le popup est actuellement actif
            PopUp.SetActive(!isActive); // Inverse l'�tat
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
                        txtContexte.text = texte1; // Assigner texte1 au TextMeshPro
                 

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

        else
        {
            Debug.LogError("Le prefab n'est pas assign�.");
        }
    }

        public void OnClosePrefabButtonClick()
        {
            PopUp.SetActive(false);
        }

    public void OnClickElement()
    {
        //Si il n'est pas s�l�ctionn�e, l'ajoute � la liste et change la couleur

        // Compare avec la liste elementen trop

        //Si sa correspond, affiche le dialogue r�solu
    }
}
