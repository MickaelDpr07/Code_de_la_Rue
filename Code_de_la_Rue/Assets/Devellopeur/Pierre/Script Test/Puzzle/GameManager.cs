using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Liste des casse-têtes à vérifier
    public GameObject[] puzzles; // Tableau ou liste de casse-têtes à gérer
    public int NbrPuzzleRéussi;

    public GameObject PanelFin;

    public TuToPuzzle tutorielPuzzle;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void CheckPuzzle()
   {
        if (SceneManager.GetActiveScene().name == "TutoPuzzle")
        {
            Debug.Log("tuto case +1");
            tutorielPuzzle.EtapeSuivante();
        }

        if(NbrPuzzleRéussi == puzzles.Length && SceneManager.GetActiveScene().name != "TutoPuzzle")
        {
            PanelFin.SetActive(true);
            Debug.Log("tout est réussi");
        }
   }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MainScene");
    }
}
