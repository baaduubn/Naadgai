using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Header : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshProUGUI;
    [SerializeField]
    private Image m_Image;
    public TheGame game;
    private void Awake()
    {
        m_TextMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        
    }
    public void Set(TheGame _header)
    {
        m_TextMeshProUGUI.text = _header.title;
        m_Image.sprite = _header.headerImage;
    }
}
