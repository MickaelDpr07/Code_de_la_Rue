using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

public class Elemententrop : MonoBehaviour
{
    [SerializeField]
    private GameObject PopUp;

    [SerializeField]
    public string texte1, texte2; // Variable pour les diff�rents textes

    private TMP_Text txt_Contexte; // Texte � afficher

    [SerializeField]
    public GameObject[] ElementsEnTrop; // Variable pour les diff�rents �l�ments en trop
    public GameObject[] ElementsBon; // Variable pour les diff�rents �l�ments bon
    private Toggle selectedToggle;

    public Button BouttonContexte;
    public GameObject BTNValider;

    public bool completed;

    public GameManager GameManager;

    
    private List<GameObject> ElementsSelectionn�es = new List<GameObject>(); // liste pour g�rer la s�lection dynamiquement

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
           
           bool isActive = PopUp.activeSelf;

            if(SceneManager.GetActiveScene().name != "TutoPuzzle")
            {
                PopUp.SetActive(!isActive);
            }
           
            Transform txtContexteTransform = PopUp.transform.Find("Txt_contexte");

            if (txtContexteTransform != null)
            {
                TMP_Text txtContexte = txtContexteTransform.GetComponent<TMP_Text>();
                txt_Contexte = txtContexte;

                if (txtContexte != null)
                {
                    txtContexte.text = texte1;
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

    // M�thode pour s�lectionner une r�ponse
    public void SelectButton(Toggle button)
    {
        GameObject selectedElement = button.gameObject;

        if (button.isOn)
        {
            // Ajoute l'�l�ment � la liste ElementsSelectionn�es
            AddElementToSelection(selectedElement);

            SVGImage svgImage = selectedElement.GetComponent<SVGImage>();
            if (svgImage != null)
            {
                svgImage.color = Color.red;
            }
            else
            {
                Debug.LogWarning("Le composant SVGImage n'a pas �t� trouv� sur l'�l�ment s�lectionn�.");
            }
        }
        else
        {
            // Retire l'�l�ment de la s�lection
            RemoveElementFromSelection(selectedElement);

            SVGImage svgImage = selectedElement.GetComponent<SVGImage>();
            if (svgImage != null)
            {
                svgImage.color = Color.white;
            }
        }
    }

    private void AddElementToSelection(GameObject element)
    {
        // Ajoute l'�l�ment uniquement s'il n'est pas d�j� dans la liste
        if (!ElementsSelectionn�es.Contains(element))
        {
            ElementsSelectionn�es.Add(element);
            Debug.Log("�l�ment ajout� � la s�lection.");
        }
        else
        {
            Debug.LogWarning("L'�l�ment est d�j� dans la s�lection.");
        }
    }

    private void RemoveElementFromSelection(GameObject element)
    {
        if (ElementsSelectionn�es.Contains(element))
        {
            ElementsSelectionn�es.Remove(element);
            Debug.Log("�l�ment retir� de la s�lection.");
        }
        else
        {
            Debug.LogWarning("L'�l�ment n'a pas �t� trouv� dans la s�lection.");
        }
    }

    // M�thode pour v�rifier si la r�ponse est correcte
    public void CheckReponse()
    {
        bool reponseCorrecte = true;

        // V�rifier que le nombre d'�l�ments s�lectionn�s est �gal au nombre d'�l�ments en trop
        if (ElementsSelectionn�es.Count != ElementsEnTrop.Length)
        {
            reponseCorrecte = false;
        }
        else
        {
            // Parcourt chaque �l�ment s�lectionn� et v�rifie qu'il fait partie des �l�ments en trop
            foreach (GameObject elementSelectionn� in ElementsSelectionn�es)
            {
                if (!System.Array.Exists(ElementsEnTrop, element => element == elementSelectionn�))
                {
                    reponseCorrecte = false;
                    break;
                }
            }
        }

        // Affiche le r�sultat dans le texte contextuel ou dans la console
        if (reponseCorrecte)
        {
            txt_Contexte.text = texte2;
            if(BTNValider !=null)
            {
                BTNValider.SetActive(false);
            }
            GameManager.NbrPuzzleR�ussi++;
            GameManager.CheckPuzzle();
            //On detruit les elements en trop
            foreach (GameObject element in ElementsEnTrop)
            {
                if (element != null) // V�rifie que l'�l�ment n'est pas null avant de le d�truire
                {
                    Destroy(element);
                    Debug.Log(element.name + " a �t� d�truit.");
                }
            }

            //Emp�che de reselectionn�es les �l�ments
            foreach(GameObject elementbon in ElementsBon)
            {
                if (elementbon != null) // V�rifie que l'�l�ment n'est pas null avant de le d�truire
                {
                    elementbon.GetComponent<Toggle>().interactable = false;
                }
            }

            BouttonContexte.interactable = false;
        }
        else
        {
            txt_Contexte.text = "Je ne suis pas sur que ce soit �a..";
        }
    }
}