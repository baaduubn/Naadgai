using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelIndex : MonoBehaviour
{

    private TextMeshProUGUI levelText;

    [Tooltip("тухайн левэл дээр нэмэгдэх тоо")]
    public int number;
    [Tooltip("тухайн левел урдаа текст байх эсэх")]
    public bool isHasText;

    // Start is called before the first frame update
    void Start()
    {
        levelText = gameObject.GetComponent<TextMeshProUGUI>();
        if (isHasText)
        {
            levelText.text = "Level: " + (SceneManager.GetActiveScene().buildIndex + number);
        }
        else
        {
        levelText.text = SceneManager.GetActiveScene().buildIndex + number+"";

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
