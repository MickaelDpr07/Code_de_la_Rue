using System.Threading.Tasks;
using System.Net;
using UnityEngine;
using TMPro;
using System.IO;

public class ModifNomSérieQuizz : MonoBehaviour
{
    public TextMeshProUGUI TXTNomSerie;
    public TextMeshProUGUI TXTScore;
    public TextMeshProUGUI TXTNouveau; // Si tu as un texte "Nouveau" à afficher
    public string csvURL;

    async void Start()
    {
        await TelechargerEtLireCSV();
    }

    async Task TelechargerEtLireCSV()
    {
        using (WebClient client = new WebClient())
        {
            string csvData = await client.DownloadStringTaskAsync(csvURL);
            string nomSérie = ExtraireNomSérie(csvData);

            TXTNomSerie.text = nomSérie;

            // Mettre à jour l'affichage du score
            string scoreText = ScoreQuizzManager.GetScoreSerieQuizzString(nomSérie);
            TXTScore.text = scoreText;

            // Afficher un message si le quizz n'a pas encore été réalisé
            if (scoreText == "Quizz pas encore réalisé")
            {
                TXTNouveau.gameObject.SetActive(true);
            }
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
