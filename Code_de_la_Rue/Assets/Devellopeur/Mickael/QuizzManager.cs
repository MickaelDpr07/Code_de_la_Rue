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
    public Button validerButton; // Bouton pour valider les réponses du joueur
    public Text progressionText; // Texte pour afficher "Question X sur Y"

    [Header("Quiz Data")]
    private List<QuizQuestion> quizQuestions = new List<QuizQuestion>(); // Liste des questions
    private int indexQuestionActu = 0;
    private bool[] selectionJoueur; // Sélections du joueur pour la question actuelle

    private int totalScore = 0; // Score total du joueur

    [Header("CSV Source")]
    public string csvURL = "https://docs.google.com/spreadsheets/d/1abcdEfGhijKlMnOpQrStuvWxYZ1234/export?format=csv"; // URL du fichier CSV

    [Header("Fin Quiz")]
    public GameObject finQuizPanel; // Référence au panneau de fin du quiz
    public Text scoreText; // Référence au texte qui affichera le score final

    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] reponses;
        public List<int> reponseCorrectQuestion;
    }

    private QuizQuestion questionActuel; // Variable pour la question actuelle

    void Start()
    {
        StartCoroutine(TelechargementDataQuiz()); // Démarrer le téléchargement du CSV au démarrage
        validerButton.interactable = false; // Désactiver le bouton valider au début
        validerButton.onClick.AddListener(CalculDuScore); // Ajouter l'événement de validation
        finQuizPanel.SetActive(false); // Assurez-vous que le panneau de fin de quiz est désactivé
    }

    IEnumerator TelechargementDataQuiz()
    {
        using (WebClient client = new WebClient())
        {
            string csvData = client.DownloadString(csvURL);
            AnalyseFichierCSV(csvData); // Parser les données après le téléchargement
        }

        yield return null; // Continuer après la fin de la coroutine
    }

    void AnalyseFichierCSV(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        string line;

        reader.ReadLine(); // Ignorer la première ligne

        while ((line = reader.ReadLine()) != null)
        {
            string[] fields = line.Split(',');

            if (fields.Length < 6) continue; // Ligne mal formatée ou incomplète

            QuizQuestion question = new QuizQuestion
            {
                question = fields[0],
                reponses = new string[4]
            };

            for (int i = 0; i < 4; i++)
            {
                question.reponses[i] = fields[i + 1];
            }

            question.reponseCorrectQuestion = new List<int>();
            string[] reponsesCorrect = fields[5].Split(';');
            foreach (string laReponseCorrect in reponsesCorrect)
            {
                if (int.TryParse(laReponseCorrect, out int indexReponse))
                {
                    question.reponseCorrectQuestion.Add(indexReponse - 1); // Convertir en index 0-based
                }
            }

            quizQuestions.Add(question);
        }

        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (indexQuestionActu >= quizQuestions.Count)
        {
            EndQuiz(); // Fin du quiz si toutes les questions ont été posées
            return;
        }

        questionActuel = quizQuestions[indexQuestionActu];

        questionText.text = questionActuel.question;
        selectionJoueur = new bool[questionActuel.reponses.Length];

        for (int i = 0; i < boutonsReponse.Length; i++)
        {
            if (i < questionActuel.reponses.Length)
            {
                boutonsReponse[i].gameObject.SetActive(true);
                boutonsReponse[i].GetComponentInChildren<Text>().text = questionActuel.reponses[i];
                int indexBouton = i; // Stocker l'index (pour le click)
                boutonsReponse[i].onClick.RemoveAllListeners(); // Nettoyer les anciens listeners
                boutonsReponse[i].onClick.AddListener(() => OnAnswerButtonClicked(indexBouton)); // Ajouter d'un listener
                ResetButtonAppearance(i); // Réinitialiser l'apparence
            }
            else
            {
                boutonsReponse[i].gameObject.SetActive(false); // Désactiver les boutons non utilisés
            }
        }

        validerButton.interactable = false; // Désactiver le bouton de validation
        progressionText.text = "Question " + (indexQuestionActu + 1) + " sur " + quizQuestions.Count; // Mise à jour du texte de progression
    }

    void OnAnswerButtonClicked(int index)
    {
        selectionJoueur[index] = !selectionJoueur[index]; // Inverser la sélection
        UpdateButtonAppearance(index); // Mettre à jour l'apparence du bouton
        validerButton.interactable = true; // Activer le bouton "Valider"
    }

    void UpdateButtonAppearance(int index)
    {
        if (selectionJoueur[index])
        {
            boutonsReponse[index].GetComponent<Image>().color = HexToColor("#43B2C1"); // Fond bleu clair
            Text buttonText = boutonsReponse[index].GetComponentInChildren<Text>();
            buttonText.color = Color.white; // Texte blanc
            buttonText.fontSize = 24; // Texte plus grand

            Outline outline = boutonsReponse[index].GetComponent<Outline>();
            if (outline == null)
            {
                outline = boutonsReponse[index].gameObject.AddComponent<Outline>(); // Ajouter l'effet Outline
            }
            outline.effectColor = Color.white; // Bordure blanche
            outline.effectDistance = new Vector2(8, 8); // Augmenter l'épaisseur
        }
        else
        {
            ResetButtonAppearance(index); // Réinitialiser l'apparence du bouton
        }
    }

    void ResetButtonAppearance(int index)
    {
        Color bleuClair = HexToColor("#43B2C1");
        boutonsReponse[index].GetComponent<Image>().color = bleuClair; // Couleur par défaut
        Text buttonText = boutonsReponse[index].GetComponentInChildren<Text>();
        buttonText.color = Color.white; // Texte blanc par défaut
        buttonText.fontSize = 20; // Taille de texte par défaut

        Outline outline = boutonsReponse[index].GetComponent<Outline>();
        if (outline != null)
        {
            Destroy(outline); // Retirer l'effet Outline
        }
    }

    Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        return Color.white;
    }

    public void CalculDuScore()
    {
        for (int i = 0; i < questionActuel.reponses.Length; i++)
        {
            if (selectionJoueur[i] && questionActuel.reponseCorrectQuestion.Contains(i))
            {
                boutonsReponse[i].GetComponent<Image>().color = Color.green; // Vert pour bonne réponse
            }
            else if (selectionJoueur[i] && !questionActuel.reponseCorrectQuestion.Contains(i))
            {
                boutonsReponse[i].GetComponent<Image>().color = Color.red; // Rouge pour mauvaise réponse
            }
        }

        StartCoroutine(DelayBeforeNextQuestion());
    }

    private IEnumerator DelayBeforeNextQuestion()
    {
        yield return new WaitForSeconds(1.5f); // Délai de 2 secondes

        totalScore += GetScoreForCurrentQuestion(); // Calculer le score
        indexQuestionActu++; // Passer à la question suivante

        if (indexQuestionActu >= quizQuestions.Count)
        {
            EndQuiz(); // Fin du quiz si toutes les questions ont été posées
        }
        else
        {
            LoadQuestion(); // Charger la question suivante
        }
    }

    private int GetScoreForCurrentQuestion()
    {
        int questionScore = 0;

        foreach (int correctIndex in questionActuel.reponseCorrectQuestion)
        {
            if (selectionJoueur[correctIndex])
            {
                questionScore++;
            }
        }

        return questionScore;
    }

    public void EndQuiz()
    {
        scoreText.text = "Votre score final est : " + totalScore + " sur " + quizQuestions.Count + " questions. ";

        finQuizPanel.SetActive(true);
    }
}
