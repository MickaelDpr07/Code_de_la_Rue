using System.Collections.Generic;
using UnityEngine;

public class ScoreQuizzManager : MonoBehaviour
{
    private static ScoreQuizzManager instance;

    private Dictionary<string, (int scoreActuel, int scoreMaximal)> scores = new Dictionary<string, (int, int)>();

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
        if (!instance.scores.ContainsKey(nomSerie))
        {
            instance.scores[nomSerie] = (0, scoreMaximal);
        }
        instance.scores[nomSerie] = (scoreObtenu, instance.scores[nomSerie].scoreMaximal);
        Debug.Log(GetScoreSerieQuizz(nomSerie));
    }

    public static (int scoreActuel, int scoreMaximal) GetScoreSerieQuizz(string nomSerie)
    {
        if (instance.scores.ContainsKey(nomSerie))
        {
            return instance.scores[nomSerie];
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
        return $"{score.scoreActuel} / {score.scoreMaximal}";
    }
}
