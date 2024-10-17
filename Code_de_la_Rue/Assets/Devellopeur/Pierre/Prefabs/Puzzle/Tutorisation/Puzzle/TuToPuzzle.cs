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
    private int EtapeTuto = 0;

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
    public GameObject ChoixRéponses;

    // Start is called before the first frame update
    void Start()
    {
        CheckEtape(); // V�rifier l'�tape initiale
    }

    public void EtapeSuivante()
    {
       
            EtapeTuto++;
            CheckEtape(); // Mettre à jour l'�tape
      
    }

    public void CheckEtape()
    {
        switch (EtapeTuto)
        {
            case 0:
                // Actions pour l'�tape 0
                TextExplicatif.text = "Dans les puzzles, vous devrez résoudre différentes situations problématiques que je vais essayer de vous expliquer.";
                Btn_suivant.interactable = true;
                ContextePopUp.SetActive(false);
                break;

            case 1:
                // Actions pour l'�tape 1
                TextExplicatif.text = "Dans cette situation, votre but va être de remettre l'élément au bon endroit en le déplaçant avec votre doigt.";
                Btn_suivant.interactable = false;
                DragandDrop.SetActive(true);
                ElementsEnigme1.SetActive(true);
                break;

            case 2:
                // R�solution de l'�tape 2
                TextExplicatif.text = "Voilà qui est bien mieux ! Ce piétion ne risquera pas de se faire écraser.";
                Btn_suivant.interactable = true;
                ElementsEnigme1.SetActive(false);
                break;

            case 3:
                // Actions pour l'�tape 3
                TextExplicatif.text = "Ici, votre objectif est de sélectionner l'élément qui semble faire tâche ou qui pourrait être en danger.";
                Btn_suivant.interactable = false;
                DragandDrop.SetActive(false);
                ElementEnTrop.SetActive(true);
                ContextePopUp.SetActive(true);
                break;

            case 4:
                // R�solution de l'�tape 3
                TextExplicatif.text = "Au moins, plus personne n'est en danger !";
                Btn_suivant.interactable = true;
                ContextePopUp.SetActive(false);
                break;

            case 5:
                //  Actions pour l'�tape 4
                TextExplicatif.text = "Et pour finir, ici vous devrez retrouver quel élément semble manquer à cet emplacement.";
                Btn_suivant.interactable = false;
                ElementEnTrop.SetActive(false);
                ElementManquant.SetActive(true);
                //ChoixR�ponses.SetActive(true);
                break;

            case 6:
                //  R�solution de l'�tape 4
                TextExplicatif.text = "Je me disais bien qu'il manquait quelque chose ici.";
                Btn_suivant.interactable = true;
                //ElementManquant.SetActive(false);
                ChoixRéponses.SetActive(false);
                break;

            case 7:
                //  R�solution de l'�tape 5
                TextExplicatif.text = "Les situations peuvent être vastes. Pour vous déplacer, il vous suffit de placer deux doigts sur l'écran pour zoomer et d'un seul pour déplacer la scène.";
                Btn_suivant.interactable = true;

                break;

            default:
                // Actions pour les �tapes suppl�mentaires (non d�finies)
                SceneManager.LoadScene("C1P1");
                break;
        }
    }
    public void SkipTuto()
    {
        EtapeTuto = 8;
        CheckEtape();
    }
}