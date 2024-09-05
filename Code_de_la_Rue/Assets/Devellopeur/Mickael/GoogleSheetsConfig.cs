using UnityEngine;

[CreateAssetMenu(fileName = "GoogleSheetsConfig", menuName = "ScriptableObjects/GoogleSheetsConfig", order = 1)]
public class GoogleSheetsConfig : ScriptableObject
{
    public string sheetId; // Identifiant de feuille (ex : 1H5Vu-POGv1lc2nLZuydIP44XOsSAGDJXXJAqBV9p0m4) 
    public string apiKey; // clé API ou dans ce cas l'URL de déploiement de l'APP Script (ex : AIzsSyBveA2zMSycioD2TJfAO1ON2hV1Igd9goX) 
    public string range; // La plage de récupération des données (ex : Character!A2:Y1000)
}