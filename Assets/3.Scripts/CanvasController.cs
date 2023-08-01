using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController :Singleton<CanvasController>
{

    private bool isStarted;
    [SerializeField]
    private GameObject _menu, _gamePlay,_winui,_gameOverUi;
 

    private void Start()
    {
        _gamePlay.SetActive(false);
    }

    public void GameStart()
    {
        if (isStarted) return;
        _menu.SetActive(false);
        _gamePlay.SetActive(true);
        GameManager.Instance.SceneStart();
        isStarted = true;
    }
    public void LevelComplete()
    {
        _gamePlay.SetActive(false);
        Camera.main.gameObject.GetComponentInChildren<ParticleSystem>().Play();
        _winui.SetActive(true);
    }

    public void Fail()
    {
        _gamePlay.SetActive(false);
        _gameOverUi.SetActive(true);
    }

}
