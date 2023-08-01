using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }

    [Tooltip("Зорилго")]
    public float Goal = 1f;



    [HideInInspector]
    public bool isLevelComplete;



    [HideInInspector]
    public float   LevelProcessMin = 0f, LevelProcess;

    [HideInInspector]
    public int score;



    void Awake()
    {
    
        Instance = this;
    }

    void Update()
    {
    
        if (Goal <= LevelProcess&&!isLevelComplete)
        {
            GameManager.Instance.LevelComplete(3);
            isLevelComplete = true;
        }
    }















}
