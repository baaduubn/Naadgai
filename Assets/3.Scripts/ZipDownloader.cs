using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ZipDownloader : Singleton<ZipDownloader>
{
    public string url;
    private string zipFilePath; // The path to the zip file
    private string extractPath; // The path where you want to extract the contents
    public TextMeshProUGUI debug;
    private Image downloadingUI;
    public TextMeshProUGUI downloadSpeedTMP;
    // Start is called before the first frame update
    public void StartDowloadGame(TheGame game)
    {
        url = game.downloadUrl;
        downloadingUI = GameHome.Instance.playBtnIcon;
        downloadingUI.sprite = GameHome.Instance.downloadIcon;
        downloadingUI.type = Image.Type.Filled;
        downloadingUI.fillMethod = Image.FillMethod.Radial360;
        downloadingUI.fillOrigin = (int)Image.Origin360.Top;
        downloadingUI.fillAmount = 0;
        string fullPath = game.path;
        string stringBeforeLastBackslash = GetStringBeforeLastBackslash(fullPath);
        zipFilePath = Application.persistentDataPath + "/zip_file_name.zip";
        extractPath = Application.dataPath + "/../Builds/" + stringBeforeLastBackslash;
        CreateDirectoryIfNotExists(extractPath);
        StartCoroutine(DownloadAndExtractZipFile(game));
    }

    void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Created directory: " + path);
        }
    }
    string GetStringBeforeLastBackslash(string fullPath)
    {
        int lastBackslashIndex = fullPath.LastIndexOf("\\");
        if (lastBackslashIndex >= 0)
        {
            // Found a backslash, extract the part before it
            string stringBeforeLastBackslash = fullPath.Substring(0, lastBackslashIndex);
            return stringBeforeLastBackslash;
        }

        // No backslash found, return the full input string as is
        return fullPath;
    }

    private IEnumerator DownloadAndExtractZipFile(TheGame game)
    {
        // Download the zip file
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            float startTime = Time.time;
            www.SendWebRequest();

            while (!www.isDone)
            {
                float progress = Mathf.Clamp01(www.downloadProgress);
                int percent = Mathf.RoundToInt(progress * 100);
                float currentTime = Time.time;
                float elapsedTime = currentTime - startTime;
                float downloadSpeed = www.downloadedBytes / elapsedTime / (1024 * 1024); // Speed in KB/s
                Debug.Log("Downloading: " + percent + "%");
                Debug.Log("Download Speed: " + downloadSpeed.ToString("F2") + " KB/s");
              
               
                downloadingUI.fillAmount = progress;
                downloadSpeedTMP.text = downloadSpeed.ToString("F2") + " MB/s";
                yield return null;

            }

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                debug.text = www.error;
                Debug.LogError("Error while downloading: " + www.error);
                yield break;
            }

            File.WriteAllBytes(zipFilePath, www.downloadHandler.data);
            Debug.Log("Download complete. Saved at: " + zipFilePath);
            downloadingUI.fillAmount = 0;
            downloadSpeedTMP.text = "";
         
            // Extract the zip file
            if (!File.Exists(zipFilePath))
            {
                Debug.LogError("Zip file not found: " + zipFilePath);
                yield break;
            }

            if (!Directory.Exists(extractPath))
            {
                Debug.LogError("Extract path does not exist: " + extractPath);
                yield break;
            }

            try
            {
                ZipFile.ExtractToDirectory(zipFilePath, extractPath);

                GetComponent<GameHome>().CheckGame(game);
                Debug.Log("Zip file extracted successfully!");

            }
            catch (IOException ex)
            {
                Debug.LogError("Error extracting zip file: " + ex.Message);
            }
        }
    }
}
