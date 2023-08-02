using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGame", menuName = "ScriptableObjects/TheGame")]
public class TheGame : ScriptableObject
{
    public string title;
    public Sprite headerImage;
    public string path;
    public string description;
    public Sprite[] screenshot;
    public string about;
    public string downloadUrl;
}