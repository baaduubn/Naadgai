using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameHome : Singleton<GameHome>
{
    public TextMeshProUGUI description, aboutGame;
    public Image headerImage;
    public Button playButton;
    public Image playBtnIcon;
    public Sprite playIcon, downloadIcon, installIcon; 
    private TextMeshProUGUI playBtnString;
    private Process process;
    private bool versionCorrect=false;
    private void Awake()
    {
   
        playBtnString = playButton.transform.GetComponentInChildren<TextMeshProUGUI>();
    }
   
    public void Set(TheGame game)
    {
        description.text = game.description;
        aboutGame.text = game.about;
        headerImage.sprite = game.headerImage;
        CheckGame(game);
        // playButton.onClick.AddListener(AddAndStartProcess);

    }
    public void StartGame(TheGame game)
    {
        string path = Application.dataPath + "/../Builds/" + game.path;
        CheckVersion(game);
        if (versionCorrect)
        {
            try
            {
                process = Process.Start(path);
                playBtnString.text = "lauching ";
                // Optionally, you can do something with the process here if needed.
            }
            catch (Exception ex)
            {
                playBtnString.text = "down ";
                DownloadGame(game);
            }
        }
        else
        {
            playBtnString.text = "Update";
        }
       
    }
    public void DownloadGame(TheGame game)
    {
        playBtnString.text = "downloading";
        var zipdowloader = ZipDownloader.Instance;
        zipdowloader.StartDowloadGame(game);

    }
    public void UpdateGame(TheGame game)
    {
        FolderDeleter.Instance.Delete(game);
        DownloadGame(game);
    }
    private void Update()
    {
        if (process == null) return;
        if (process.HasExited)
        {
            process = null;
            playBtnString.text = "Play ";
        }
    }

    public void CheckGame(TheGame game)
    {
        FolderDeleter.Instance.Delete(game);
        playButton.onClick.RemoveAllListeners();
        if (game == null)
        {
            print("The game parameter is null.");
            return;
        }
        CheckVersion(game);
        string path = Application.dataPath + "/../Builds/" + game.path;

        if (File.Exists(path))
        {
            if (versionCorrect)
            {
                playBtnIcon.enabled = true;
                playBtnIcon.type = Image.Type.Simple;
                playButton.onClick.AddListener(() => StartGame(game));
                playBtnIcon.sprite = playIcon;
                playBtnString.text = "Play";
            }
            else
            {
                playBtnIcon.enabled = true;
                playBtnIcon.type = Image.Type.Simple;
                playButton.onClick.AddListener(() => UpdateGame(game));
                playBtnIcon.sprite = installIcon;
                playBtnString.text = "Install";
            }
           
           
        }
        else
        {
            // Game executable doesn't exist, set the play button text to "Install"
            playBtnIcon.enabled = true;
            playBtnIcon.type = Image.Type.Simple;
            playButton.onClick.AddListener(() => DownloadGame(game));
            playBtnIcon.sprite = installIcon;
            playBtnString.text = "Install";
        }
    }

    public void BackButton()
    {
        this.gameObject.SetActive(false);
        playButton.onClick.RemoveAllListeners();
    }
   
    public void CheckVersion(TheGame game)
    {
      
    }

  

}
