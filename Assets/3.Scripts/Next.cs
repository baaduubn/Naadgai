using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next : MonoBehaviour
{
    public void NextButton()
    {
        GameManager.Instance.NextLevel();
    }
}
