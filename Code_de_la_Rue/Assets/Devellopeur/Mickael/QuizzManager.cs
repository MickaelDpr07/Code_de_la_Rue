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
    public Button[] boutonsReponse; // Tableau de boutons de r�ponses (jusqu'� 4)
    public Button validerButton; // Bouton pour valider les r�ponses du joueur
    public Text progressionText; // Texte pour afficher "Question X sur Y"

    [Header("Quiz Data")]
    private List<QuizQuestion> quizQuestions = new List<QuizQuestion>(); // Liste des questions
    private int indexQuestionActu = 0;
    private bool[] selectionJoueur; // S�lections du joueur pour la question actuelle

    private int totalScore = 0; // Score total du joueur

    [Header("CSV Source")]
    public string csvURL = "https://docs.google.com/spreadsheets/d/1abcdEfGhijKlMnOpQrStuvWxYZ1234/export?format=csv"; // URL du fichier CSV

    [Header("Fin Quiz")]
    public GameObject finQuizPanel; // R�f�rence au panneau de fin du quiz
    public Text scoreText; // R�f�rence au texte qui affichera le score final

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
        StartCoroutine(TelechargementDataQuiz()); // D�marrer le t�l�chargement du CSV au d�marrage
        validerButton.interactable = false; // D�sactiver le bouton valider au d�but
        validerButton.onClick.AddListener(CalculDuScore); // Ajouter l'�v�nement de validation
        finQuizPanel.SetActive(false); // Assurez-vous que le panneau de fin de quiz est d�sactiv�
    }

    IEnumerator TelechargementDataQuiz()
    {
        using (WebClient client = new WebClient())
        {
            string csvData = client.DownloadString(csvURL);
            AnalyseFichierCSV(csvData); // Parser les donn�es apr�s le t�l�chargement
        }

        yield return null; // Continuer apr�s la fin de la coroutine
    }

    void AnalyseFichierCSV(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        string line;

        reader.ReadLine(); // Ignorer la premi�re ligne

        while ((line = reader.ReadLine()) != null)
        {
            string[] fields = line.Split(',');

            if (fields.Length < 6) continue; // Ligne mal format�e ou incompl�te

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
            EndQuiz(); // Fin du quiz si toutes les questions ont �t� pos�es
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
                ResetButtonAppearance(i); // R�initialiser l'apparence
            }
            else
            {
                boutonsReponse[i].gameObject.SetActive(false); // D�sactiver les boutons non utilis�s
            }
        }

        validerButton.interactable = false; // D�sactiver le bouton de validation
        progressionText.text = "Question " + (indexQuestionActu + 1) + " sur " + quizQuestions.Count; // Mise � jour du texte de progression
    }

    void OnAnswerButtonClicked(int index)
    {
        selectionJoueur[index] = !selectionJoueur[index]; // Inverser la s�lection
        UpdateButtonAppearance(index); // Mettre � jour l'apparence du bouton
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
            outline.effectDistance = new Vector2(8, 8); // Augmenter l'�paisseur
        }
        else
        {
            ResetButtonAppearance(index); // R�initialiser l'apparence du bouton
        }
    }

    void ResetButtonAppearance(int index)
    {
        Color bleuClair = HexToColor("#43B2C1");
        boutonsReponse[index].GetComponent<Image>().color = bleuClair; // Couleur par d�faut
        Text buttonText = boutonsReponse[index].GetComponentInChildren<Text>();
        buttonText.color = Color.white; // Texte blanc par d�faut
        buttonText.fontSize = 20; // Taille de texte par d�faut

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
                boutonsReponse[i].GetComponent<Image>().color = Color.green; // Vert pour bonne r�ponse
            }
            else if (selectionJoueur[i] && !questionActuel.reponseCorrectQuestion.Contains(i))
            {
                boutonsReponse[i].GetComponent<Image>().color = Color.red; // Rouge pour mauvaise r�ponse
            }
        }

        StartCoroutine(DelayBeforeNextQuestion());
    }

    private IEnumerator DelayBeforeNextQuestion()
    {
        yield return new WaitForSeconds(1.5f); // D�lai de 2 secondes

        totalScore += GetScoreForCurrentQuestion(); // Calculer le score
        indexQuestionActu++; // Passer � la question suivante

        if (indexQuestionActu >= quizQuestions.Count)
        {
            EndQuiz(); // Fin du quiz si toutes les questions ont �t� pos�es
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
