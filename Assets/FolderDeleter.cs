using UnityEngine;
using System.IO;

public class FolderDeleter : Singleton<FolderDeleter>
{
    // The path to the folder you want to delete (relative to the Assets folder)
    public string folderPathToDelete = "Path/To/Folder";

    void Start()
    {
      
    }
    public void Delete(TheGame game)
    {
        string stringBeforeLastBackslash = GetStringBeforeLastBackslash(game.path);
        string fullPath = Application.dataPath + "/../Builds/" + stringBeforeLastBackslash;

        if (Directory.Exists(fullPath))
        {
            DeleteFolder(fullPath);
            Debug.Log("Folder deleted: " + fullPath);
       
        }
        else
        {
            Debug.Log("Folder not found: " + fullPath);
        }
    }
    string GetStringBeforeLastBackslash(string fullPath)
    {
        int lastBackslashIndex = fullPath.LastIndexOf("\\");
        if (lastBackslashIndex >= 0)
        {
            // Found a backslash, extract the part before it
            string stringBeforeLastBackslash = fullPath.Substring(0, lastBackslashIndex);
            return stringBeforeLastBackslash;
        }

        // No backslash found, return the full input string as is
        return fullPath;
    }
    void DeleteFolder(string targetPath)
    {
        if (!Directory.Exists(targetPath))
            return;

        string[] files = Directory.GetFiles(targetPath);
        string[] dirs = Directory.GetDirectories(targetPath);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteFolder(dir);
        }

        Directory.Delete(targetPath, false);
    }
}
