using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Singleton<Player>
{

    private void Start()
    {
        
        
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.LevelComplete();
        }
    }


}
