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
    public GameObject PanelCategorie;
    public GameObject PanelTheme;


    // A Afficher
    public TextMeshProUGUI Categorie;
    public TextMeshProUGUI titre;
    public TextMeshProUGUI contenu;

    public Animator Animation;

    // Start is called before the first frame update
    void Start()
    {
        //Fait en sorte que ce soit bien le panel theme qui apparaisse en premier 
        PanelCategorie.SetActive(false);
        PanelTheme.SetActive(true);
    }

    void Awake()
    {
        //Fait en sorte que ce soit bien le panel theme qui apparaisse en premier 
        PanelCategorie.SetActive(false);
        PanelTheme.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenText(string description)
    {
        //R�cup�re l'objet derni�rement s�l�ctionn� est le stock dans la variable
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;

        //CATEGORIE

        Transform ancestor = clickedButton.transform;

        //R�cup�re l'anc�tre 4 du boutton
        for (int i = 0; i < 4; i++)
        {
            if (ancestor != null)
            {
                ancestor = ancestor.parent;
        
            }
            else
            {
                Debug.LogError("Le parent � la hi�rarchie demand�e n'existe pas.");
                return;
            }
        }
        // Trouver le composant Text dans cet anc�tre
        TextMeshProUGUI textComponent = ancestor.GetComponentInChildren<TextMeshProUGUI>();
        //l'applique au titre de la definition
        Categorie.text = textComponent.text;
        

        //TITRE

        //Je r�cup�re le textmeshpro de l'enfant du gameobject derni�rement s�l�ctionner soit le boutton
        TextMeshProUGUI buttonText = clickedButton.GetComponentInChildren<TextMeshProUGUI>();
        titre.text = buttonText.text;

        //CONTENUE

        contenu.text = description;


        PanelDescription.SetActive(true);

    }

    public void Retour()
    {
        if (Animation != null)
        {
            // Assure que l'animation existe avant de la jouer
            if (Animation.HasState(0, Animator.StringToHash("LexiquePopUp")))
            {
                // Joue l'animation à partir de la fin (1.0f) et la rejoue à l'envers manuellement
                StartCoroutine(PlayAnimationBackward("LexiquePopUp"));
            }
            else
            {
                Debug.LogError("L'état 'Panel_Description' n'a pas été trouvé dans l'Animator.");
            }
        }
        else
        {
            Debug.LogError("L'Animator n'est pas assigné.");
        }
    }

    private IEnumerator PlayAnimationBackward(string animationName)
    {
        AnimatorStateInfo stateInfo = Animation.GetCurrentAnimatorStateInfo(0);
        float playbackTime = 1.0f; // Commence à la fin de l'animation

        // Joue l'animation normalement depuis la fin
        Animation.Play(animationName, 0, playbackTime);

        while (playbackTime > 0f)
        {
            // Diminue progressivement le temps de lecture de l'animation
            playbackTime -= Time.deltaTime / stateInfo.length;

            // Applique le temps de lecture mis à jour
            Animation.Play(animationName, 0, playbackTime);

            yield return null; // Attends la frame suivante
        }

        // Remet l'animation à la première frame (position 0)
        PanelDescription.SetActive(false);
        Animation.Play(animationName, 0, 0.0f);
    }



    public void QuitterLexique()
    {
        PanelButton.SetActive(true);
        PanelLexique.SetActive(false);

    }

    public void OuvrirCategorie()
    {
        PanelCategorie.SetActive(true);
        Animation.Play("Panel_Description",0,0f);
        Animation.speed = 1;
        PanelTheme.SetActive(false);
    }

    public void QuitterCategorie() // Avec le boutton en croix
    {
        PanelCategorie.SetActive(false);
        PanelTheme.SetActive(true);
    }
}
