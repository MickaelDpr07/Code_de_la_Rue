using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

public class QuizManager : MonoBehaviour
{
    public Text questionText;
    public Text explicationText;
    public Button[] boutonsReponse;
    public Button validerButton;
    public Text progressionText;

    public GameObject finQuizPanel;
    public Text scoreText;

    public string csvURL = "https://docs.google.com/spreadsheets/d/1abcdEfGhijKlMnOpQrStuvWxYZ1234/export?format=csv";

    [SerializeField] private Color couleurNonSelectionnee = Color.cyan;
    [SerializeField] private Color couleurSelectionnee = Color.white;
    [SerializeField] private Color contourBlanc = Color.white;
    [SerializeField] private int taillePoliceNonSelectionnee = 20;
    [SerializeField] private int taillePoliceSelectionnee = 24;
    [SerializeField] private Font policeTexte;
    [SerializeField] private float largeurContour = 5f;
    [SerializeField] private float tempsAvantProchaine = 2f;

    [SerializeField] private string scoreTextFormat = "Votre score final est :";

    private List<QuizQuestion> quizQuestions = new List<QuizQuestion>();
    private int indexQuestionActu = 0;
    private bool[] selectionJoueur;
    private int totalScore = 0;
    private QuizQuestion questionActuel;

    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] reponses;
        public List<int> reponseCorrectQuestion;
        public string explication;
    }

    void Start()
    {
        StartCoroutine(TelechargementDataQuiz());
        validerButton.interactable = false;
        validerButton.onClick.AddListener(CalculDuScore);
        explicationText.gameObject.SetActive(false);
    }

    IEnumerator TelechargementDataQuiz()
    {
        using (WebClient client = new WebClient())
        {
            string csvData = client.DownloadString(csvURL);
            AnalyseFichierCSV(csvData);
        }
        yield return null;
    }

    void AnalyseFichierCSV(string csvData)
    {
        StringReader reader = new StringReader(csvData);
        string line;

        reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {
            string[] fields = line.Split(',');

            if (fields.Length < 7)
            {
                Debug.LogWarning("Ligne mal formatée ou incomplète : " + line);
                continue;
            }

            QuizQuestion question = new QuizQuestion();
            question.question = fields[0];
            question.reponses = new string[4];

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
                    question.reponseCorrectQuestion.Add(indexReponse - 1);
                }
            }

            question.explication = fields[6];
            quizQuestions.Add(question);
        }

        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (indexQuestionActu >= quizQuestions.Count)
        {
            EndQuiz();
            return;
        }

        questionActuel = quizQuestions[indexQuestionActu];
        questionText.text = questionActuel.question;
        questionText.font = policeTexte;
        selectionJoueur = new bool[questionActuel.reponses.Length];
        explicationText.gameObject.SetActive(false);

        for (int i = 0; i < boutonsReponse.Length; i++)
        {
            if (i < questionActuel.reponses.Length && !string.IsNullOrEmpty(questionActuel.reponses[i]))
            {
                boutonsReponse[i].gameObject.SetActive(true);
                boutonsReponse[i].GetComponentInChildren<Text>().text = questionActuel.reponses[i];
                boutonsReponse[i].GetComponentInChildren<Text>().font = policeTexte;
                boutonsReponse[i].onClick.RemoveAllListeners();
                int indexBouton = i;
                boutonsReponse[i].onClick.AddListener(() => OnAnswerButtonClicked(indexBouton));
                ResetButtonAppearance(i);
            }
            else
            {
                boutonsReponse[i].gameObject.SetActive(false);
            }
        }

        validerButton.interactable = false;
        progressionText.text = "Question " + (indexQuestionActu + 1) + " sur " + quizQuestions.Count;
    }

    void OnAnswerButtonClicked(int index)
    {
        selectionJoueur[index] = !selectionJoueur[index];
        UpdateButtonAppearance(index);
        validerButton.interactable = true;
    }

    void UpdateButtonAppearance(int index)
    {
        if (selectionJoueur[index])
        {
            boutonsReponse[index].GetComponent<Image>().color = couleurSelectionnee;
            Text buttonText = boutonsReponse[index].GetComponentInChildren<Text>();
            buttonText.color = contourBlanc;
            buttonText.fontSize = taillePoliceSelectionnee;

            Outline outline = boutonsReponse[index].GetComponent<Outline>();
            if (outline == null)
            {
                outline = boutonsReponse[index].gameObject.AddComponent<Outline>();
            }
            outline.effectColor = contourBlanc;
            outline.effectDistance = new Vector2(largeurContour, largeurContour);
        }
        else
        {
            ResetButtonAppearance(index);
        }
    }

    void ResetButtonAppearance(int index)
    {
        boutonsReponse[index].GetComponent<Image>().color = couleurNonSelectionnee;
        Text buttonText = boutonsReponse[index].GetComponentInChildren<Text>();
        buttonText.color = Color.white;
        buttonText.fontSize = taillePoliceNonSelectionnee;

        Outline outline = boutonsReponse[index].GetComponent<Outline>();
        if (outline != null)
        {
            Destroy(outline);
        }
    }

    public void CalculDuScore()
    {
        bool correct = true;

        for (int i = 0; i < questionActuel.reponses.Length; i++)
        {
            if (selectionJoueur[i] && !questionActuel.reponseCorrectQuestion.Contains(i))
            {
                correct = false;
            }
            else if (!selectionJoueur[i] && questionActuel.reponseCorrectQuestion.Contains(i))
            {
                correct = false;
            }
        }

        if (correct)
        {
            totalScore++;
            StartCoroutine(AfficherExplicationEtCouleur(questionActuel.explication));
        }
        else
        {
            ColorerBoutonsSelectionnes();
            StartCoroutine(DelayBeforeNextQuestion());
        }
    }

    IEnumerator AfficherExplicationEtCouleur(string explication)
    {
        ColorerBoutonsSelectionnes();
        explicationText.text = explication;
        explicationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(tempsAvantProchaine);
        indexQuestionActu++;
        LoadQuestion();
    }

    void ColorerBoutonsSelectionnes()
    {
        for (int i = 0; i < boutonsReponse.Length; i++)
        {
            if (i < questionActuel.reponses.Length && selectionJoueur[i])
            {
                Color targetColor = questionActuel.reponseCorrectQuestion.Contains(i) ? Color.green : Color.red;
                boutonsReponse[i].GetComponent<Image>().color = targetColor;
            }
        }
    }

    IEnumerator DelayBeforeNextQuestion()
    {
        yield return new WaitForSeconds(tempsAvantProchaine);
        indexQuestionActu++;
        LoadQuestion();
    }

    void EndQuiz()
    {
        scoreText.text = scoreTextFormat + " " + totalScore + " / " + quizQuestions.Count;
        finQuizPanel.SetActive(true);
    }
}
