using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static partial class A {

    ///<summary>Screenshot хийнэ</summary>
    public static void Screenshot(string path, string name) {
        IO.CheckCrtDir(path);
        string fileName = path + "/" + name + ".png";
#if UNITY_5
        Application.CaptureScreenshot (fileName, 1);
#else
        ScreenCapture.CaptureScreenshot(fileName, 1);
#endif
    }
}