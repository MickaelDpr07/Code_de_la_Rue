using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class GoogleSheetsClient : MonoBehaviour, IDataSubject
{
    [SerializeField] private GoogleSheetsConfig config;
    private readonly List<IDataObserver> observers = new List<IDataObserver>();

    private void Start()
    {
        FetchSheetData();
    }

    private async void FetchSheetData()
    {
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{config.sheetId}/values/{Uri.EscapeDataString(config.range)}?key={config.apiKey}";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<CharacterData> characters = CharacterDataJsonParser.ParseJson(json);
                    NotifyObservers(characters);
                }
                else
                {
                    Debug.LogError($"Data fetch failed. Status code: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error encountered during data fetch: {ex.Message}");
        }
    }

    // ;)
}