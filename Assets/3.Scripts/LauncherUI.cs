using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class LauncherUI : MonoBehaviour
{
    public GameObject header;
    public TheGame game,game2;
    public Transform contentTran;
    private Process process;
    [SerializeField]
    private GameObject gameHome;
    private void Start()
    {
       ShowGame(game);
        ShowGame(game2);
    }

    private void ShowGame(TheGame game)
    {
        var a = Instantiate(header, transform.position, transform.rotation, contentTran);
        a.GetComponentInChildren<Header>().Set(game);
        var b = a.AddComponent<Button>();
        b.onClick.AddListener(() => AddAndStartProcess(game));
    }

    public void AddAndStartProcess(TheGame game)
    {
        // Assuming the gameHome object is already assigned in the Inspector.
        gameHome.SetActive(true);

        // Assuming the GameHome script has a Set method that takes the game object as an argument.
        GameHome.Instance.Set(game);
    }


}

