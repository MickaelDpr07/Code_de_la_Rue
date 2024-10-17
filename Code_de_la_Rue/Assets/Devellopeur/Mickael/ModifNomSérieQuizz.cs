using UnityEngine;
using TMPro; // Importation de la bibliothèque TextMesh Pro
using System.Collections;
using System.IO;
using System.Net;

public class ModifNomSérieQuizz : MonoBehaviour
{
    public TextMeshProUGUI serieText; // Champ de texte TextMesh Pro pour afficher le nom de la série
    public string csvURL;  // Lien vers le fichier CSV

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

            // Mettre à jour le champ de texte avec le nom de la série
            serieText.text = nomSérie;

            yield return null; // Nécessaire pour permettre l'attente dans une coroutine
        }
    }

    string ExtraireNomSérie(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        string firstLine = reader.ReadLine(); // Lire la première ligne

        if (firstLine != null)
        {
            string[] fields = firstLine.Split(',');
            if (fields.Length > 0)
            {
                return fields[0].Trim(); // Retourner le premier champ qui est supposé être le nom de la série
            }
        }

        return "Nom de série introuvable"; // Retourne un message par défaut si le nom n'est pas trouvé
    }
}
