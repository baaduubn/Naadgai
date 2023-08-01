using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
   

    private static GameState _state = GameState.Paused;
    public static GameState State
    {
        get { return _state; }
        set
        {
            _state = value;
           
        }
    }
    
    

    public void SceneStart()
    {
        _state = GameState.Playing;
        ConsoleProDebug.LogToFilter("Үзэгдэл эхэлсэн.","Game manager");
    }
  

    /// <summary>
    /// Үе даваад хожсон бол дуудна
    /// </summary>
    /// <returns>The number</returns>
    /// <param name="time">Дараагын үе нээгдэх хугацаа</param>
    public void LevelComplete(float time)
    {
        if (_state == GameState.Playing) return;
        _state = GameState.LevelCompleted;
        CanvasController.Instance.LevelComplete();
        Invoke("NextLevel", time);
        ConsoleProDebug.LogToFilter("Та хожлоо. "+time+" секундын дараа дараагын үе идэвхжинэ","Game manager");
       
    }
    public void LevelComplete()
    {
        if (_state == GameState.Playing) return;
        _state = GameState.LevelCompleted;
        CanvasController.Instance.LevelComplete();
        ConsoleProDebug.LogToFilter("Та хожлоо. ","Game manager");
    }

    /// <summary>
    /// Хожигдсон бол дуудна
    /// </summary>
    /// <returns>The number</returns>
    /// <param name="time">Үе дахин эхлэх хугацаа</param>
    public void Fail(float time)
    {

        if (_state == GameState.Playing) return;
        _state = GameState.Fail;
        CanvasController.Instance.Fail();
        Invoke("LoadAgain", time);
      
        ConsoleProDebug.LogToFilter("Та хожигдлоо. " + time + " секундын дараа дараагын үе идэвхжинэ","Game manager");
    }
    public void Fail()
    {
        if (_state == GameState.Playing) return;
        _state = GameState.Fail;
        CanvasController.Instance.Fail();
        ConsoleProDebug.LogToFilter("Та хожигдлоо. ","Game manager");
    }

    /// <summary>
    /// Дахин эхлүүлэхдээ дуудна
    /// </summary>
    public void Restart()
    {
        _state = GameState.Fail;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ConsoleProDebug.LogToFilter("Дахин оролдов","Game manager");
        
    }
    

    public void NextLevel()
    {
        int sceneIndex =  SceneManager.GetActiveScene().buildIndex + 1;
        ConsoleProDebug.LogToFilter("Дараагын үе ачааллаж байна","Game manager");
        StartCoroutine(LoadAsynschorounly(sceneIndex));
           

       
    }

    /// <summary>
    /// үе өөрчлөхдөө ачаалал хийнэ
    /// </summary>
    /// <param name="sceneIndex">ачааллах үе</param>
    /// <returns></returns>
    IEnumerator LoadAsynschorounly(int sceneIndex)
    {
       
        int scenelegth = SceneManager.sceneCountInBuildSettings;

        if (scenelegth <= sceneIndex)
        {
            sceneIndex = 0;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);




        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
          
            //Debug.Log(progress);
            if (progress == 1)
            {
                ConsoleProDebug.LogToFilter("Дараагын үе идэвхчлээ","Game manager");
            }
            yield return null;
        }
    }





    private void LoadAgain()
    {
        int scene=    SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadAsynschorounly(scene));
        print("Үе ахин идэвхчлээ");
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
           
        }
    }

}
public enum GameState
{
    Starting,
    Playing,
    Paused,
    LevelCompleted,
    Fail
}