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
        if(NbrPuzzleR�ussi == puzzles.Length)
        {
            PanelFin.SetActive(true);
            Debug.Log("tout est r�ussi");
        }
   }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MainScene");
    }
}
