using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public static class CharacterDataJsonParser
{
    public static List<CharacterData> ParseJson(string json)
    {
        List<CharacterData> characters = new List<CharacterData>();

        try
        {
            JObject data = JObject.Parse(json);
            JArray entries = (JArray)data["values"];

            for (int i = 1; i < entries.Count; i++)
            {
                var entry = entries[i].ToObject<List<string>>();
                CharacterData character = new CharacterData
                {
                    Name = entry[0],
                    Level = int.Parse(entry[1]),
                    Type = entry[2]
                };
                characters.Add(character);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Error parsing JSON: " + ex.Message);
        }

        return characters;
    }
}