using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;

public class VersionManager : Singleton<VersionManager>
{
    // Define a list to hold the game information
    private List<GameInfo> games;

    // Method to find the version of a game by its name
    public string GetVersionByName(string gameName)
    {
        foreach (GameInfo game in games)
        {
            if (game.Name == gameName)
            {
                return game.Version;
            }
        }

        return "Game not found";
    }

    // Create a structure to hold game information
    [Serializable]
    public struct GameInfo
    {
        public string Name;
        public string Version;
    }

    // Example usage
    private void Start()
    {
        // Start the coroutine to fetch the JSON file from the internet URL
        StartCoroutine(LoadJsonFromURL("https://drive.usercontent.google.com/download?id=1msL_5t1Q8jYQwHlbqIYoQotnwLYkSf5U&export=download&authuser=0&confirm=t&uuid=d29dea70-50bb-4a84-80f1-11d13f657ad8&at=AC2mKKRrlpSER9H3FqGDd7RzChKr:1690977494651"));
    }

    // Coroutine to fetch JSON data from an internet URL
    private IEnumerator LoadJsonFromURL(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // Save the downloaded JSON data locally
                string localPath = Application.persistentDataPath + "/version.json";
                File.WriteAllText(localPath, www.downloadHandler.text);

                // Parse the JSON data and populate the 'games' list
                games = JsonUtility.FromJson<VersionData>(www.downloadHandler.text).games;

                // Example usage of GetVersionByName method
                string gameName = "CSGO";
                string version = GetVersionByName(gameName);
                Debug.Log($"Version of {gameName}: {version}");
            }
        }
    }

    // Method to load JSON data from a local file
    private void LoadJsonFromFile(string filePath)
    {
        string jsonData = File.ReadAllText(filePath);
        games = JsonUtility.FromJson<VersionData>(jsonData).games;
    }

    // Create a class to hold the version data (to match the JSON structure)
    [Serializable]
    public class VersionData
    {
        public List<GameInfo> games;
    }
}
