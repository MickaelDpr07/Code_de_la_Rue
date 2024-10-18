using System.Collections.Generic;
using UnityEngine;

public class ScoreQuizzManager : MonoBehaviour
{
    private static ScoreQuizzManager instance;

    private Dictionary<string, (int scoreActuel, int scoreMaximal)> scoresQuizz = new Dictionary<string, (int, int)>();
    private Dictionary<string, (int scoreActuel, int scoreMaximal)> scoresPuzzle = new Dictionary<string, (int, int)>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SetScoreSerieQuizz(string nomSerie, int scoreObtenu, int scoreMaximal)
    {
        if (!instance.scoresQuizz.ContainsKey(nomSerie))
        {
            instance.scoresQuizz[nomSerie] = (0, scoreMaximal);
        }
        instance.scoresQuizz[nomSerie] = (scoreObtenu, instance.scoresQuizz[nomSerie].scoreMaximal);
        Debug.Log(GetScoreSerieQuizz(nomSerie));
    }

    public static (int scoreActuel, int scoreMaximal) GetScoreSerieQuizz(string nomSerie)
    {
        if (instance.scoresQuizz.ContainsKey(nomSerie))
        {
            return instance.scoresQuizz[nomSerie];
        }
        else
        {
            Debug.LogWarning("La série " + nomSerie + " n'existe pas.");
            return (0, 0);
        }
    }

    public static string GetScoreSerieQuizzString(string nomSerie)
    {
        var score = GetScoreSerieQuizz(nomSerie);
        if (score.scoreActuel == 0 && score.scoreMaximal == 0)
        {
            return "Quizz pas encore réalisé";
        }
        return $"Votre score : {score.scoreActuel} / {score.scoreMaximal}";
    }

    // Méthodes pour les puzzles
    public static void SetScorePuzzle(string nomPuzzle, int scoreObtenu, int scoreMaximal)
    {
        if (!instance.scoresPuzzle.ContainsKey(nomPuzzle))
        {
            instance.scoresPuzzle[nomPuzzle] = (0, scoreMaximal);
        }
        instance.scoresPuzzle[nomPuzzle] = (scoreObtenu, instance.scoresPuzzle[nomPuzzle].scoreMaximal);
        Debug.Log(GetScorePuzzle(nomPuzzle));
    }

    public static (int scoreActuel, int scoreMaximal) GetScorePuzzle(string nomPuzzle)
    {
        if (instance.scoresPuzzle.ContainsKey(nomPuzzle))
        {
            return instance.scoresPuzzle[nomPuzzle];
        }
        else
        {
            Debug.LogWarning("Le puzzle " + nomPuzzle + " n'existe pas.");
            return (0, 0);
        }
    }

    public static string GetScorePuzzleString(string nomPuzzle)
    {
        var score = GetScorePuzzle(nomPuzzle);
        if (score.scoreActuel == 0 && score.scoreMaximal == 0)
        {
            return "Puzzle pas encore réalisé";
        }
        return $"Votre score : {score.scoreActuel} / {score.scoreMaximal}";
    }
}
