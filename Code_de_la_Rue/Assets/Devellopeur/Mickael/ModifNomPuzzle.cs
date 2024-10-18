using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;
using System.Net;

public class ModifNomPuzzle : MonoBehaviour
{
    public TextMeshProUGUI TXTNomSerie;
    public TextMeshProUGUI TXTScore;

    public TextMeshProUGUI TXTNouveau;

    void Start()
    {
        string nomSérie = TXTNomSerie.text;
        TXTScore.text = ScoreQuizzManager.GetScorePuzzleString(nomSérie);

        if (ScoreQuizzManager.GetScorePuzzleString(nomSérie) == "Puzzle pas encore réalisé")
        {
            TXTNouveau.gameObject.SetActive(true);
        }
    }
}
