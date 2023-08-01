using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTitle : MonoBehaviour
{
    private TextMeshProUGUI titleui;

    void Start()
    {
        titleui =  gameObject.GetComponent<TextMeshProUGUI>();
        titleui.text = Application.productName;
      
    }
}
