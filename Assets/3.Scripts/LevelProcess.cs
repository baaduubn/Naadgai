using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProcess : MonoBehaviour
{
    private Slider levelProcess;
   
   
    // Start is called before the first frame update
    void Start()
    {
        levelProcess = gameObject.GetComponent<Slider>();
        levelProcess.maxValue = LevelManager.Instance.Goal;
        levelProcess.minValue = LevelManager.Instance.LevelProcessMin;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        levelProcess.value = LevelManager.Instance.LevelProcess;
    }
}
