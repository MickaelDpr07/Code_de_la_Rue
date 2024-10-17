using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TuToPuzzle : MonoBehaviour
{
    [SerializeField]
    public GameManager GameManager;

    [Header("Global")]
    [SerializeField]
    public Button Btn_suivant;
    public TextMeshProUGUI TextExplicatif;
    public int EtapeTuto = 0;

    [SerializeField]
    [Header("Enigme 1")]
    public GameObject DragandDrop;
    public GameObject ElementsEnigme1;

    [SerializeField]
    [Header("Enigme 2")]
    public GameObject ElementEnTrop;
    public GameObject ContextePopUp;

    [SerializeField]
    [Header("Enigme 3")]
    public GameObject ElementManquant;
    public GameObject ChoixR�ponses;

    // Start is called before the first frame update
    void Start()
    {
        CheckEtape(); // V�rifier l'�tape initiale
    }

    public void EtapeSuivante()
    {
       
            EtapeTuto++;
            CheckEtape(); // Mettre � jour l'�tape
      
    }

    public void CheckEtape()
    {
        switch (EtapeTuto)
        {
            case 0:
                // Actions pour l'�tape 0
                TextExplicatif.text = "Dans les puzzles, vous devrez r�soudre di�rentes situations probl�matiques que je vais essayer de vous expliquer.";
                Btn_suivant.interactable = true;
                ContextePopUp.SetActive(false);
                break;

            case 1:
                // Actions pour l'�tape 1
                TextExplicatif.text = "Dans cette situation, votre but va �tre de remettre l��l�ment au bon endroit en le d�pla�ant avec votre doigt.";
                Btn_suivant.interactable = false;
                DragandDrop.SetActive(true);
                ElementsEnigme1.SetActive(true);
                break;

            case 2:
                // R�solution de l'�tape 2
                TextExplicatif.text = "Voil� qui est bien mieux ce pi�tion ne risquera pas de se faire �craser.";
                Btn_suivant.interactable = true;
                ElementsEnigme1.SetActive(false);
                break;

            case 3:
                // Actions pour l'�tape 3
                TextExplicatif.text = "Ici, votre objectif est de s�l�ctionner l��l�ment qui semble faire t�che ou qui pourrait �tre en danger.";
                Btn_suivant.interactable = false;
                DragandDrop.SetActive(false);
                ElementEnTrop.SetActive(true);
                ContextePopUp.SetActive(true);
                break;

            case 4:
                // R�solution de l'�tape 3
                TextExplicatif.text = "Au moins, personne n�est en danger !";
                Btn_suivant.interactable = true;
                ContextePopUp.SetActive(false);
                break;

            case 5:
                //  Actions pour l'�tape 4
                TextExplicatif.text = "Et pour finir, ici vous devrez retrouver quel �l�ment semble manquer � cet emplacement.";
                Btn_suivant.interactable = false;
                ElementEnTrop.SetActive(false);
                ElementManquant.SetActive(true);
                //ChoixR�ponses.SetActive(true);
                break;

            case 6:
                //  R�solution de l'�tape 4
                TextExplicatif.text = "Je me disais bien qu�il manquait quelque chose ici.";
                Btn_suivant.interactable = true;
                //ElementManquant.SetActive(false);
                ChoixR�ponses.SetActive(false);
                break;

            case 7:
                //  R�solution de l'�tape 5
                TextExplicatif.text = "Les situations peuvent �tre vastes pour vous d�placer il vous suffit de placer deux doigts sur l��cran pour d�zoomer et d�un seul pour d�placer la sc�ne.";
                Btn_suivant.interactable = true;

                break;

            default:
                // Actions pour les �tapes suppl�mentaires (non d�finies)
                SceneManager.LoadScene("C1P1");
                break;
        }
    }
}