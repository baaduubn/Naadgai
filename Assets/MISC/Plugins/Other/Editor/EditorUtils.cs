using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;
public static class EditorUtils {
    private static EditorWindow _mouseOverWindow;

    [MenuItem("Tools/Screenshot")]
    public static void Screenshot() {
        A.Screenshot("Screenshots/", "Screenshot" + System.DateTime.Now.ToString("_yyyy-MM-dd_hh-mm-ss"));
    }

    [MenuItem("Tools/Google Drive",false,100)]
    public static void GoDrive()
    {
        Application.OpenURL("https://drive.google.com/drive/folders/1F3ATW8MJxXMYXoYloUvKCH8DbirenV7R");
    }


    [MenuItem("Tools/DeletePlayerPrefs")]
    public static void DeletePlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Toggle Lock %l")]
    static void ToggleInspectorLock() {
        if (_mouseOverWindow == null) {
            if (!EditorPrefs.HasKey("LockableInspectorIndex"))
                EditorPrefs.SetInt("LockableInspectorIndex", 0);
            int i = EditorPrefs.GetInt("LockableInspectorIndex");

            Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
            Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
            _mouseOverWindow = (EditorWindow)findObjectsOfTypeAll[i];
        }

        if (_mouseOverWindow != null && _mouseOverWindow.GetType().Name == "InspectorWindow") {
            Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
            PropertyInfo propertyInfo = type.GetProperty("isLocked");
            bool value = (bool)propertyInfo.GetValue(_mouseOverWindow, null);
            propertyInfo.SetValue(_mouseOverWindow, !value, null);
            _mouseOverWindow.Repaint();
        }
    }
}