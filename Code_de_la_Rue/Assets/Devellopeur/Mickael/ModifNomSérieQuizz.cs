using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;
using System.Net;

public class ModifNomSérieQuizz : MonoBehaviour
{
    public TextMeshProUGUI TXTNomSerie;
    public TextMeshProUGUI TXTScore;
    public string csvURL;

    public TextMeshProUGUI TXTNouveau;

    void Start()
    {
        StartCoroutine(TelechargerEtLireCSV());
    }

    IEnumerator TelechargerEtLireCSV()
    {
        using (WebClient client = new WebClient())
        {
            string csvData = client.DownloadString(csvURL);
            string nomSérie = ExtraireNomSérie(csvData);

            TXTNomSerie.text = nomSérie;

            // Maj affichage du score
            TXTScore.text = ScoreQuizzManager.GetScoreSerieQuizzString(nomSérie);

            if (ScoreQuizzManager.GetScoreSerieQuizzString(nomSérie) == "Quizz pas encore réalisé")
            {
                TXTNouveau.gameObject.SetActive(true);
            }

            yield return null;
        }
    }

    string ExtraireNomSérie(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        string firstLine = reader.ReadLine();

        if (firstLine != null)
        {
            string[] fields = firstLine.Split(',');
            if (fields.Length > 0)
            {
                return fields[0].Trim();
            }
        }

        return "Nom de série introuvable";
    }
}
