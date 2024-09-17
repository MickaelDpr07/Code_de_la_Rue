using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text questionText; // Le texte pour la question
    public Button[] answerButtons; // Tableau de boutons de réponses (jusqu'à 4)

    [Header("Quiz Data")]
    private List<QuizQuestion> quizQuestions = new List<QuizQuestion>(); // Liste des questions
    private int currentQuestionIndex = 0;
    private bool[] playerSelections; // Sélections du joueur pour la question actuelle

    private int totalScore = 0; // Score total du joueur

    [Header("CSV Source")]
    public string csvURL = "https://docs.google.com/spreadsheets/d/1abcdEfGhijKlMnOpQrStuvWxYZ1234/export?format=csv"; // URL du fichier CSV ou chemin local

    // Structure représentant une question du quiz
    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] answers;
        public List<int> correctAnswerIndices;
    }

    void Start()
    {
        StartCoroutine(DownloadQuizData()); // Démarrer le téléchargement du CSV au démarrage du jeu
    }

    // Téléchargement et lecture du fichier CSV
    IEnumerator DownloadQuizData()
    {
        // Utiliser un WebClient pour télécharger le fichier CSV
        using (WebClient client = new WebClient())
        {
            string csvData = client.DownloadString(csvURL);
            ParseCSV(csvData); // Parser les données après le téléchargement
        }

        yield return null; // Continuer après la fin de la coroutine
    }

    // Lecture et parsing du fichier CSV
    void ParseCSV(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        string line;

        // Ignorer la première ligne (header)
        reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {
            // Diviser chaque ligne par les virgules (','), ajuster selon votre format
            string[] fields = line.Split(',');

            QuizQuestion question = new QuizQuestion();
            question.question = fields[0];
            question.answers = new string[4];

            for (int i = 0; i < 4; i++)
            {
                question.answers[i] = fields[i + 1];
            }

            // Extraire les bonnes réponses (qui peuvent être multiples)
            question.correctAnswerIndices = new List<int>();
            string[] correctAnswers = fields[5].Split(',');
            foreach (string correctAnswer in correctAnswers)
            {
                question.correctAnswerIndices.Add(int.Parse(correctAnswer) - 1); // Convertir en index 0-based
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
        if (currentQuestionIndex >= quizQuestions.Count)
        {
            EndQuiz(); // Fin du quiz si toutes les questions ont été posées
            return;
        }

        // Obtenir la question actuelle
        QuizQuestion currentQuestion = quizQuestions[currentQuestionIndex];

        // Afficher la question
        questionText.text = currentQuestion.question;

        // Initialiser les sélections du joueur
        playerSelections = new bool[currentQuestion.answers.Length];

        // Afficher les réponses et gérer les boutons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];
                int buttonIndex = i; // Stocker l'index pour le onClick
                answerButtons[i].onClick.RemoveAllListeners(); // Nettoyer les anciens listeners
                answerButtons[i].onClick.AddListener(() => OnAnswerButtonClicked(buttonIndex)); // Ajouter le listener
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false); // Désactiver les boutons non utilisés
            }
        }
    }

    // Appelée lorsqu'un bouton de réponse est cliqué
    void OnAnswerButtonClicked(int index)
    {
        // Inverser la sélection (sélectionner/désélectionner)
        playerSelections[index] = !playerSelections[index];

        // Mettre à jour la couleur du bouton pour indiquer la sélection
        UpdateButtonColor(index);
    }

    // Change la couleur du bouton en fonction de la sélection
    void UpdateButtonColor(int index)
    {
        ColorBlock colors = answerButtons[index].colors;
        if (playerSelections[index])
        {
            colors.normalColor = Color.green; // Sélectionné
        }
        else
        {
            colors.normalColor = Color.white; // Non sélectionné
        }
        answerButtons[index].colors = colors;
    }

    // Calcul du score pour une question
    public void CalculateScore()
    {
        QuizQuestion currentQuestion = quizQuestions[currentQuestionIndex];
        int questionScore = 0;

        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            if (playerSelections[i] == currentQuestion.correctAnswerIndices.Contains(i))
            {
                questionScore++; // Ajouter des points si la réponse est correcte
            }
        }

        totalScore += questionScore;
        currentQuestionIndex++; // Passer à la question suivante
        LoadQuestion(); // Charger la question suivante
    }

    // Fin du quiz
    public void EndQuiz()
    {
        Debug.Log("Quiz terminé ! Score final : " + totalScore);
        // Ici, vous pouvez afficher le score final ou charger une nouvelle scène
    }
}