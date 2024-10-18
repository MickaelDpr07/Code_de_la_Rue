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
    public string texte1, texte2; // Variable pour les différents textes

    private TMP_Text txt_Contexte; // Texte à afficher

    [SerializeField]
    public GameObject[] ElementsEnTrop; // Variable pour les différents éléments en trop
    public GameObject[] ElementsBon; // Variable pour les différents éléments bon
    private Toggle selectedToggle;

    public Button BouttonContexte;
    public GameObject BTNValider;

    public bool completed;

    public GameManager GameManager;

    
    private List<GameObject> ElementsSelectionnées = new List<GameObject>(); // liste pour gérer la sélection dynamiquement

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

    // Méthode pour sélectionner une réponse
    public void SelectButton(Toggle button)
    {
        GameObject selectedElement = button.gameObject;

        if (button.isOn)
        {
            // Ajoute l'élément à la liste ElementsSelectionnées
            AddElementToSelection(selectedElement);

            SVGImage svgImage = selectedElement.GetComponent<SVGImage>();
            if (svgImage != null)
            {
                svgImage.color = Color.red;
            }
            else
            {
                Debug.LogWarning("Le composant SVGImage n'a pas été trouvé sur l'élément sélectionné.");
            }
        }
        else
        {
            // Retire l'élément de la sélection
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
        // Ajoute l'élément uniquement s'il n'est pas déjà dans la liste
        if (!ElementsSelectionnées.Contains(element))
        {
            ElementsSelectionnées.Add(element);
            Debug.Log("Élément ajouté à la sélection.");
        }
        else
        {
            Debug.LogWarning("L'élément est déjà dans la sélection.");
        }
    }

    private void RemoveElementFromSelection(GameObject element)
    {
        if (ElementsSelectionnées.Contains(element))
        {
            ElementsSelectionnées.Remove(element);
            Debug.Log("Élément retiré de la sélection.");
        }
        else
        {
            Debug.LogWarning("L'élément n'a pas été trouvé dans la sélection.");
        }
    }

    // Méthode pour vérifier si la réponse est correcte
    public void CheckReponse()
    {
        bool reponseCorrecte = true;

        // Vérifier que le nombre d'éléments sélectionnés est égal au nombre d'éléments en trop
        if (ElementsSelectionnées.Count != ElementsEnTrop.Length)
        {
            reponseCorrecte = false;
        }
        else
        {
            // Parcourt chaque élément sélectionné et vérifie qu'il fait partie des éléments en trop
            foreach (GameObject elementSelectionné in ElementsSelectionnées)
            {
                if (!System.Array.Exists(ElementsEnTrop, element => element == elementSelectionné))
                {
                    reponseCorrecte = false;
                    break;
                }
            }
        }

        // Affiche le résultat dans le texte contextuel ou dans la console
        if (reponseCorrecte)
        {
            txt_Contexte.text = texte2;
            if(BTNValider !=null)
            {
                BTNValider.SetActive(false);
            }
            GameManager.NbrPuzzleRéussi++;
            GameManager.CheckPuzzle();
            //On detruit les elements en trop
            foreach (GameObject element in ElementsEnTrop)
            {
                if (element != null) // Vérifie que l'élément n'est pas null avant de le détruire
                {
                    Destroy(element);
                    Debug.Log(element.name + " a été détruit.");
                }
            }

            //Empêche de reselectionnées les éléments
            foreach(GameObject elementbon in ElementsBon)
            {
                if (elementbon != null) // Vérifie que l'élément n'est pas null avant de le détruire
                {
                    elementbon.GetComponent<Toggle>().interactable = false;
                }
            }

            BouttonContexte.interactable = false;
        }
        else
        {
            txt_Contexte.text = "Je ne suis pas sur que ce soit ça..";
        }
    }
}