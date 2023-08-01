using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class IO {

    ///<summary>folder байгааг шалгана</summary>
    public static bool IsDirExists(string path) {
        return Directory.Exists(path);
    }

    ///<summary>folder-уудын замыг авна</summary>
    public static string[] GetDirs(string path) {
        return Directory.GetDirectories(path);
    }

    ///<summary>folder үүсгэнэ</summary>
    public static void CrtDir(string path) {
        Directory.CreateDirectory(path);
    }

    ///<summary>шалгаад folder үүсгэнэ</summary>
    public static void CheckCrtDir(string path) {
        if (!IsDirExists(path))
            CrtDir(path);
    }

    ///<summary>file байгааг шалгана</summary>
    public static bool IsFileExists(string path) {
        return File.Exists(path);
    }

    ///<summary>file-уудын замыг авна</summary>
    public static string[] GetFiles(string path) {
        return Directory.GetFiles(path);
    }

    ///<summary>file-уудын замыг авна</summary>
    public static string[] GetFiles(string path, params string[] exts) {
        return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(s => exts.Contains(Path.GetExtension(s))).ToArray();
    }

    ///<summary>file үүсгэнэ</summary>
    public static void CrtFile(string path, string data) {
        File.WriteAllText(path, data);
    }

    ///<summary>file уншина</summary>
    public static string ReadFile(string path) {
        try {
            return File.ReadAllText(path);
        } catch (FileNotFoundException) {
            return "";
        }
    }

    ///<summary>одоогийн замыг авна</summary>
    public static string GetCurPath() {
        return Directory.GetCurrentDirectory() + "/";
    }

    ///<summary>assets-н замыг авна</summary>
    public static string GetAssPath() {
        return GetCurPath() + "Assets/";
    }

    ///<summary>ProjectSettings-н замыг авна</summary>
    public static string GetPsPath() {
        return GetCurPath() + "ProjectSettings/";
    }

    ///<summary>Extensions-н замыг авна</summary>
    public static string GetExtPath() {
        return GetAssPath() + "Scripts/Other/Extentions/";
    }

    ///<summary>Data-н замыг авна</summary>
    public static string GetDataPath() {
        return GetAssPath() + "Scripts/Other/Data/";
    }
}