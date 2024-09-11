using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Lexique : MonoBehaviour
{
    public GameObject PanelDescription;
    public GameObject PanelLexique;
    public GameObject PanelButton;
    

    // A Afficher
    public TextMeshProUGUI Categorie;
    public TextMeshProUGUI titre;
    public TextMeshProUGUI contenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenText(string description)
    {
        //Récupère l'objet dernièrement séléctionné est le stock dans la variable
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;

        //CATEGORIE

        Transform ancestor = clickedButton.transform;

        //Récupère l'ancêtre 4 du boutton
        for (int i = 0; i < 4; i++)
        {
            if (ancestor != null)
            {
                ancestor = ancestor.parent;
        
            }
            else
            {
                Debug.LogError("Le parent à la hiérarchie demandée n'existe pas.");
                return;
            }
        }
        // Trouver le composant Text dans cet ancêtre
        TextMeshProUGUI textComponent = ancestor.GetComponentInChildren<TextMeshProUGUI>();
        //l'applique au titre de la definition
        Categorie.text = textComponent.text;
        

        //TITRE

        //Je récupère le textmeshpro de l'enfant du gameobject dernièrement séléctionner soit le boutton
        TextMeshProUGUI buttonText = clickedButton.GetComponentInChildren<TextMeshProUGUI>();
        titre.text = buttonText.text;

        //CONTENUE

        contenu.text = description;


        PanelDescription.SetActive(true);

    }

    public void Retour()
    {
        PanelDescription.SetActive(false);
    }

    public void QuitterLexique()
    {
        PanelButton.SetActive(true);
        PanelLexique.SetActive(false);
        
    }
}
