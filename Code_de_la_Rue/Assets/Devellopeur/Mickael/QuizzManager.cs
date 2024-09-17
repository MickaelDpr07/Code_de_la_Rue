using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text questionText;
    public Button[] boutonsReponse; // Tableau de boutons de réponses (jusqu'à 4)

    [Header("Quiz Data")]
    private List<QuizQuestion> quizQuestions = new List<QuizQuestion>(); // Liste des questions
    private int indexQuestionActu = 0;
    private bool[] selectionJoueur; // Sélections du joueur pour la question actuelle

    private int totalScore = 0; // Score total du joueur

    [Header("CSV Source")]
    public string csvURL = "https://docs.google.com/spreadsheets/d/1abcdEfGhijKlMnOpQrStuvWxYZ1234/export?format=csv"; // URL du fichier CSV

    // Structure représentant une question du quiz
    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] reponses;
        public List<int> reponseCorrectQuestion;
    }

    void Start()
    {
        StartCoroutine(TelechargementDataQuiz()); // Démarrer le téléchargement du CSV au démarrage
    }

    // Téléchargement et lecture du fichier CSV
    IEnumerator TelechargementDataQuiz()
    {
        // Utiliser un WebClient pour télécharger le fichier CSV
        using (WebClient client = new WebClient())
        {
            string csvData = client.DownloadString(csvURL);
            AnalyseFichierCSV(csvData); // Parser les données après le téléchargement
        }

        yield return null; // Continuer après la fin de la coroutine
    }

    // Lecture et analyse du fichier CSV
    void AnalyseFichierCSV(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        string line;

        // Ignorer la première ligne
        reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {
            // Diviser chaque ligne par les virgules (','), ajuster selon votre format
            string[] fields = line.Split(',');

            // Vérifier si la ligne contient suffisamment de colonnes (6 dans votre cas : question + 4 réponses + indices des bonnes réponses)
            if (fields.Length < 6)
            {
                Debug.LogWarning("Ligne mal formatée ou incomplète : " + line);
                continue; // Ignorer cette ligne et passer à la suivante
            }

            QuizQuestion question = new QuizQuestion();
            question.question = fields[0];
            question.reponses = new string[4];

            for (int i = 0; i < 4; i++)
            {
                question.reponses[i] = fields[i + 1];
            }

            // Extraire les bonnes réponses (qui peuvent être multiples)
            question.reponseCorrectQuestion = new List<int>();
            string[] reponsesCorrect = fields[5].Split(',');
            foreach (string laReponseCorrect in reponsesCorrect)
            {
                if (int.TryParse(laReponseCorrect, out int indexReponse))
                {
                    question.reponseCorrectQuestion.Add(indexReponse - 1); // Convertir en index 0-based
                }
            }

            // Ajouter la question à la liste des questions
            quizQuestions.Add(question);
        }

        // Lancer le quiz après avoir chargé les questions
        LoadQuestion();
}

    // Charger une question du quiz
    void LoadQuestion()
    {
        if (indexQuestionActu >= quizQuestions.Count)
        {
            EndQuiz(); // Fin du quiz si toutes les questions ont été posées
            return;
        }

        // Obtenir la question actuelle
        QuizQuestion questionActuel = quizQuestions[indexQuestionActu];

        questionText.text = questionActuel.question;

        // Initialiser les sélections du joueur
        selectionJoueur = new bool[questionActuel.reponses.Length];

        // Afficher les réponses et gestion boutons
        for (int i = 0; i < boutonsReponse.Length; i++)
        {
            if (i < questionActuel.reponses.Length)
            {
                boutonsReponse[i].gameObject.SetActive(true);
                boutonsReponse[i].GetComponentInChildren<Text>().text = questionActuel.reponses[i];
                int indexBouton = i; // Stocker l'index (pour le click)
                boutonsReponse[i].onClick.RemoveAllListeners(); // Nettoyer les anciens listeners
                boutonsReponse[i].onClick.AddListener(() => OnAnswerButtonClicked(indexBouton)); // Ajouter d'un listener
            }
            else
            {
                boutonsReponse[i].gameObject.SetActive(false); // Désactiver les boutons non utilisés
            }
        }
    }

    // Appelée lorsqu'un bouton de réponse est cliqué
    void OnAnswerButtonClicked(int index)
    {
        // Inverser la sélection (sélectionner/désélectionner)
        selectionJoueur[index] = !selectionJoueur[index];

        // Mettre à jour la couleur du bouton pour indiquer la sélection
        UpdateButtonColor(index);
    }

    // Change la couleur du bouton en fonction de la sélection
    void UpdateButtonColor(int index)
    {
        ColorBlock colors = boutonsReponse[index].colors;
        if (selectionJoueur[index])
        {
            colors.normalColor = Color.green;
        }
        else
        {
            colors.normalColor = Color.white;
        }
        boutonsReponse[index].colors = colors;
    }

    // Calcul du score pour 1 question
    public void CalculDuScore()
    {
        QuizQuestion questionActuel = quizQuestions[indexQuestionActu];
        int questionScore = 0;

        for (int i = 0; i < questionActuel.reponses.Length; i++)
        {
            if (selectionJoueur[i] == questionActuel.reponseCorrectQuestion.Contains(i))
            {
                questionScore++; // Ajouter des points si la réponse est correcte
            }
        }

        totalScore += questionScore;
        indexQuestionActu++; // Passer à la question suivante
        LoadQuestion(); // Charger la question suivante
    }

    // Fin du quiz
    public void EndQuiz()
    {
        Debug.Log("Quiz terminé ! Score final : " + totalScore);
        // Ici, vous pouvez afficher le score final ou charger une nouvelle scène
    }
}