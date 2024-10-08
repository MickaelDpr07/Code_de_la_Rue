using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Ajoutez ceci pour g�rer les sc�nes
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public Text questionText;
    public Text explicationText;
    public Button[] boutonsReponse;
    public Button validerButton;
    public Text progressionText;
    public GameObject finQuizPanel;
    public Text scoreText;
    public Button quitterButton;
    public TextMeshProUGUI fichierText;

    public string csvURL = "https://docs.google.com/spreadsheets/d/1abcdEfGhijKlMnOpQrStuvWxYZ1234/export?format=csv";

    [SerializeField] private Color colorFondSelection; // Couleur du fond du bouton quand s�lectionn�
    [SerializeField] private Color colorFondNonSelection; // Couleur du fond du bouton quand non s�lectionn�
    [SerializeField] private Color colorTXTSelection; // Couleur du texte du bouton quand s�lectionn�
    [SerializeField] private Color colorTXTNonSelection; // Couleur du texte du bouton quand non s�lectionn�
    [SerializeField] private Color couleurBonneReponse = Color.green; // Couleur pour une bonne r�ponse
    [SerializeField] private Color couleurMauvaiseReponse = Color.red; // Couleur pour une mauvaise r�ponse
    [SerializeField] private Color contourBlanc = Color.white;
    [SerializeField] private float largeurContour = 5f;
    [SerializeField] private float tempsAvantProchaine = 2f;

    [SerializeField] private string scoreTextFormat = "Votre score final est :";

    // Champ pour la taille de s�lection
    [SerializeField] private int tailleSelection = 10; // Augmente la taille de la police lors de la s�lection
    [SerializeField] private int taillePolice = 40;

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

        // Assurez-vous que le bouton quitter est d�sactiv� tant que le quiz n'est pas termin�
        quitterButton.gameObject.SetActive(false);
        quitterButton.onClick.AddListener(QuitterQuiz); // Ajouter le comportement au bouton Quitter
    }

    // M�thode pour quitter et retourner � la sc�ne principale
    public void QuitterQuiz()
    {
        SceneManager.LoadScene("MainScene");
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

        // Lire la premi�re ligne pour obtenir le nom de la feuille
        if ((line = reader.ReadLine()) != null)
        {
            string[] fields = line.Split(',');

            if (fields.Length > 0)
            {
                string nomFeuille = fields[0].Trim(); // R�cup�rer le nom de la feuille
                fichierText.text = nomFeuille; // Afficher le nom dans le UI
            }
        }

        // Continuez � lire les lignes suivantes pour les questions
        while ((line = reader.ReadLine()) != null)
        {
            string[] fields = line.Split(',');

            if (fields.Length < 7)
            {
                Debug.LogWarning("Ligne mal format�e ou incompl�te : " + line);
                continue;
            }

            QuizQuestion question = new QuizQuestion();
            question.question = fields[1]; // Changez ceci pour prendre la question dans la bonne colonne
            question.reponses = new string[4];

            for (int i = 0; i < 4; i++)
            {
                question.reponses[i] = fields[i + 2]; // Ajustez l'index pour les r�ponses
            }

            question.reponseCorrectQuestion = new List<int>();
            string[] reponsesCorrect = fields[6].Split(';');
            foreach (string laReponseCorrect in reponsesCorrect)
            {
                if (int.TryParse(laReponseCorrect, out int indexReponse))
                {
                    question.reponseCorrectQuestion.Add(indexReponse - 1);
                }
            }

            question.explication = fields[7]; // Ajustez l'index pour l'explication
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
        selectionJoueur = new bool[questionActuel.reponses.Length];
        explicationText.gameObject.SetActive(false);

        for (int i = 0; i < boutonsReponse.Length; i++)
        {
            if (i < questionActuel.reponses.Length && !string.IsNullOrEmpty(questionActuel.reponses[i]))
            {
                boutonsReponse[i].gameObject.SetActive(true);
                boutonsReponse[i].GetComponentInChildren<Text>().text = questionActuel.reponses[i];
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
            boutonsReponse[index].GetComponent<Image>().color = colorFondSelection; // Utiliser la couleur de fond s�lectionn�e
            Text buttonText = boutonsReponse[index].GetComponentInChildren<Text>();

            // Changer la couleur du texte lorsque le bouton est s�lectionn�
            buttonText.color = colorTXTSelection;

            // Augmenter la taille de la police
            buttonText.fontSize += tailleSelection;

            Outline outline = boutonsReponse[index].GetComponent<Outline>();
            if (outline == null)
            {
                outline = boutonsReponse[index].gameObject.AddComponent<Outline>();
            }
            outline.effectColor = Color.white; // Garder le contour blanc
            outline.effectDistance = new Vector2(largeurContour, largeurContour);
        }
        else
        {
            ResetButtonAppearance(index);
        }
    }

    void ResetButtonAppearance(int index)
    {
        boutonsReponse[index].GetComponent<Image>().color = colorFondNonSelection; // Utiliser la couleur de fond non s�lectionn�e
        Text buttonText = boutonsReponse[index].GetComponentInChildren<Text>();

        // R�initialiser la couleur du texte
        buttonText.color = colorTXTNonSelection;

        // R�initialiser la taille de la police � sa taille d'origine
        buttonText.fontSize = taillePolice; // Remplacez par la taille d'origine

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
                Color targetColor = questionActuel.reponseCorrectQuestion.Contains(i) ? couleurBonneReponse : couleurMauvaiseReponse;
                boutonsReponse[i].GetComponent<Image>().color = targetColor; // Changer la couleur du fond
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
        finQuizPanel.SetActive(true);
        quitterButton.gameObject.SetActive(true);
        scoreText.text = scoreTextFormat + " " + totalScore + "/" + quizQuestions.Count;
    }
}
