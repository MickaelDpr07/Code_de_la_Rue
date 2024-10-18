using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Liste des casse-t�tes � v�rifier
    public GameObject[] puzzles; // Tableau ou liste de casse-t�tes � g�rer
    public int NbrPuzzleR�ussi;

    public GameObject PanelFin;

    public TuToPuzzle tutorielPuzzle;

    // Pour la "sauvegarde" du score au puzzle
    public string nomSerie;

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

        if(NbrPuzzleR�ussi == puzzles.Length && SceneManager.GetActiveScene().name != "TutoPuzzle")
        {
            if (SceneManager.GetActiveScene().name == "C1P1")
            {
                nomSerie = "2. Premiers Puzzles";
                Debug.Log("C1P1");
            }
            else if (SceneManager.GetActiveScene().name == "C1P2")
            {
                nomSerie = "4. Autres Puzzles";
                Debug.Log("C1P1");
            }

            ScoreQuizzManager.SetScorePuzzle(nomSerie, NbrPuzzleR�ussi, puzzles.Length);
            PanelFin.SetActive(true);
            Debug.Log("tout est r�ussi");
        }
   }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MainScene");
    }
}
