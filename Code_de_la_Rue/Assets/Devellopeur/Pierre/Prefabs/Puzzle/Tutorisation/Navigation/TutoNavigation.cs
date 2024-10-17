using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoNavigation : MonoBehaviour
{
    [Header("Global")]
    [SerializeField]
    public GameObject Btn_Continuer;
    public GameObject BTN_Skip;
    private int EtapeTuto = 0;
    private int indexPanel =0;

    [SerializeField]
    [Header("Panels")]
    public GameObject[] Panels;

    void Start()
    {
        CheckEtape();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EtapeSuivante()
    {
        EtapeTuto++;
        CheckEtape(); // Mettre à jour l'étape
    }

    public void CheckEtape()
    {
        switch (EtapeTuto)
        {
            case 0:
                // Actions pour l'étape 0
                UnActivePanels();
                Panels[indexPanel].SetActive(true);
                Btn_Continuer.SetActive(false);
                BTN_Skip.SetActive(false);
                break;

            case 1:
                // Actions pour l'étape 1
                AffichagePanelSuivant();
                Btn_Continuer.SetActive(true);
                BTN_Skip.SetActive(true);
                break;

            case 2:
                // Résolution de l'étape 2
                AffichagePanelSuivant();
                
                break;

            case 3:
                // Actions pour l'étape 3
                AffichagePanelSuivant();

                break;

            case 4:
                // Actions pour l'étape 4
                AffichagePanelSuivant();

                break;

            case 5:
                //  Actions pour l'étape 5
                AffichagePanelSuivant();
                break;

            case 6:
                //  Résolution de l'étape 6
                AffichagePanelSuivant();

                break;

            case 7:
                //  Résolution de l'étape 7
                AffichagePanelSuivant();

                break;

            case 8:
                //  Résolution de l'étape 8
                AffichagePanelSuivant();
                
                break;

            default:
                // Actions pour les étapes supplémentaires (non définies)
                SceneManager.LoadScene("MainScene");
                break;
        }
    }

    public void SkipTuto()
    {
        EtapeTuto = 9;
        CheckEtape();
    }


    //Sert à desactivez tout les panels
    public void UnActivePanels()
    {
        foreach(GameObject panels in Panels)
        {
            panels.SetActive(false);
        }
    }

    //Affiche le panel suivant
    public void AffichagePanelSuivant()
    {
        UnActivePanels();
        indexPanel++;
        Panels[indexPanel].SetActive(true);
    }
}
