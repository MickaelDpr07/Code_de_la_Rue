using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public Text questionText;
    public Text explicationText;
    public GameObject fondExplication;
    public Button[] boutonsReponse;
    public Button validerButton;
    public Text progressionText;
    public GameObject finQuizPanel;
    public Text scoreText;
    public Button quitterButton;
    public TextMeshProUGUI fichierText;

    public string csvURL = "https://docs.google.com/spreadsheets/d/1abcdEfGhijKlMnOpQrStuvWxYZ1234/export?format=csv";

    [SerializeField] private Color colorFondSelection;
    [SerializeField] private Color colorFondNonSelection;
    [SerializeField] private Color colorTXTSelection;
    [SerializeField] private Color colorTXTNonSelection;
    [SerializeField] private Color couleurBonneReponse = Color.green;
    [SerializeField] private Color couleurMauvaiseReponse = Color.red;
    [SerializeField] private Color contourBlanc = Color.white;
    [SerializeField] private float largeurContour = 5f;

    [SerializeField] private string scoreTextFormat = "Votre score final est :";

    [SerializeField] private int tailleSelection = 5; // Valeur de +5 lorsque sélectionné

    private List<QuizQuestion> quizQuestions = new List<QuizQuestion>();
    private int indexQuestionActu = 0;
    private bool[] selectionJoueur;
    public int totalScore = 0;
    private QuizQuestion questionActuel;
    private bool questionValidee = false;

    public int taillePoliceInitiale = 40; // Taille de police initiale à stocker

    // Ajout de nouvelles variables pour la personnalisation du bouton "SUITE"
    public Vector3 suiteButtonScale = new Vector3(1.2f, 1.2f, 1.2f); // Scale quand c'est "SUITE"
    public Color suiteButtonColor = Color.yellow; // Couleur quand c'est "SUITE"

    // Variables pour stocker l'apparence initiale du bouton "Valider"
    private Color couleurInitialeBouton;
    private Vector3 scaleInitialBouton;

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

        // Stockage de l'apparence initiale du bouton "Valider"
        couleurInitialeBouton = validerButton.GetComponent<Image>().color;
        scaleInitialBouton = validerButton.transform.localScale;

        validerButton.interactable = false;
        validerButton.onClick.AddListener(OnValiderButtonClicked);

        fondExplication.SetActive(false);

        quitterButton.gameObject.SetActive(false);
        quitterButton.onClick.AddListener(QuitterQuiz);
    }

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

        if ((line = reader.ReadLine()) != null)
        {
            string[] fields = line.Split(',');

            if (fields.Length > 0)
            {
                string nomFeuille = fields[0].Trim();
                fichierText.text = nomFeuille;
            }
        }

        while ((line = reader.ReadLine()) != null)
        {
            string[] fields = line.Split(',');

            if (fields.Length < 7)
            {
                Debug.LogWarning("Ligne mal formatée ou incomplète : " + line);
                continue;
            }

            QuizQuestion question = new QuizQuestion();
            question.question = fields[1];
            question.reponses = new string[4];

            for (int i = 0; i < 4; i++)
            {
                question.reponses[i] = fields[i + 2];
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

            question.explication = fields[7];
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

        fondExplication.SetActive(false);

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
                boutonsReponse[i].interactable = true; // Réactiver les boutons réponses
            }
            else
            {
                boutonsReponse[i].gameObject.SetActive(false);
            }
        }

        validerButton.interactable = false;
        progressionText.text = "Question " + (indexQuestionActu + 1) + " sur " + quizQuestions.Count;
        questionValidee = false;
    }

    void OnAnswerButtonClicked(int index)
    {
        selectionJoueur[index] = !selectionJoueur[index];
        UpdateButtonAppearance(index);
        validerButton.interactable = true;
    }

    void UpdateButtonAppearance(int index)
    {
        Text buttonText = boutonsReponse[index].GetComponentInChildren<Text>();

        if (selectionJoueur[index])
        {
            boutonsReponse[index].GetComponent<Image>().color = colorFondSelection;
            buttonText.color = colorTXTSelection;
            buttonText.fontSize = taillePoliceInitiale + tailleSelection; // Augmente de +5 lors de la sélection

            Outline outline = boutonsReponse[index].GetComponent<Outline>();
            if (outline == null)
            {
                outline = boutonsReponse[index].gameObject.AddComponent<Outline>();
            }
            outline.effectColor = Color.white;
            outline.effectDistance = new Vector2(largeurContour, largeurContour);
        }
        else
        {
            ResetButtonAppearance(index);
        }
    }

    void ResetButtonAppearance(int index)
    {
        boutonsReponse[index].GetComponent<Image>().color = colorFondNonSelection;
        Text buttonText = boutonsReponse[index].GetComponentInChildren<Text>();

        buttonText.color = colorTXTNonSelection;
        buttonText.fontSize = taillePoliceInitiale; // Retour à la taille initiale lorsque non sélectionné

        Outline outline = boutonsReponse[index].GetComponent<Outline>();
        if (outline != null)
        {
            Destroy(outline);
        }
    }

    void OnValiderButtonClicked()
    {
        if (!questionValidee)
        {
            CalculDuScore();
        }
        else
        {
            indexQuestionActu++;
            LoadQuestion();
            ResetValiderButtonAppearance();  // On restaure l'apparence initiale ici
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
            StartCoroutine(AfficherExplicationEtChangerBouton(questionActuel.explication));
        }
        else
        {
            ColorerBoutonsSelectionnes();
            ModifierBoutonPourSuite(); // On modifie l'apparence ici quand c'est "SUITE"
            questionValidee = true;
        }

        DisableButtonsWithoutFilter();
    }

    void DisableButtonsWithoutFilter()
    {
        foreach (Button bouton in boutonsReponse)
        {
            // Sauvegarde des couleurs actuelles
            ColorBlock colorBlock = bouton.colors;

            // Appliquer les couleurs actuelles comme couleur de l'état désactivé
            colorBlock.disabledColor = bouton.colors.normalColor; // Garde la couleur normale quand désactivé

            bouton.colors = colorBlock;
            bouton.interactable = false; // Désactive les interactions
        }
    }

    IEnumerator AfficherExplicationEtChangerBouton(string explication)
    {
        fondExplication.SetActive(true);
        explicationText.text = explication;

        ColorerBoutonsSelectionnes();

        ModifierBoutonPourSuite(); // Change l'apparence du bouton pour "SUITE"
        questionValidee = true;

        yield return null;
    }

    void ModifierBoutonPourSuite()
    {
        validerButton.GetComponentInChildren<Text>().text = "SUITE";
        validerButton.GetComponent<Image>().color = suiteButtonColor; // Change la couleur
        validerButton.transform.localScale = suiteButtonScale; // Change la taille (scale)
        validerButton.interactable = true; // Le bouton "SUITE" doit rester interactif
    }

    void ResetValiderButtonAppearance()
    {
        validerButton.GetComponentInChildren<Text>().text = "Valider";
        validerButton.GetComponent<Image>().color = couleurInitialeBouton; // Restaure la couleur
        validerButton.transform.localScale = scaleInitialBouton; // Restaure la taille (scale)
        validerButton.interactable = false; // Désactiver le bouton jusqu'à ce que le joueur fasse un choix
    }

    void ColorerBoutonsSelectionnes()
    {
        for (int i = 0; i < boutonsReponse.Length; i++)
        {
            if (selectionJoueur[i])
            {
                if (questionActuel.reponseCorrectQuestion.Contains(i))
                {
                    boutonsReponse[i].GetComponent<Image>().color = couleurBonneReponse;
                }
                else
                {
                    boutonsReponse[i].GetComponent<Image>().color = couleurMauvaiseReponse;
                }
            }
        }
    }

    void EndQuiz()
    {
        questionText.text = "Fin du quiz !";
        finQuizPanel.SetActive(true);
        scoreText.text = scoreTextFormat + " " + totalScore + " / " + quizQuestions.Count;
        quitterButton.gameObject.SetActive(true);

        foreach (Button bouton in boutonsReponse)
        {
            bouton.gameObject.SetActive(false);
        }

        validerButton.gameObject.SetActive(false);
    }
}
