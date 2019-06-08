using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetTime
{
    public static bool IsDay() {
        if (System.DateTime.Now.Hour > 19 || System.DateTime.Now.Hour < 6) {
            return false;
        }
        return true;
    }

    public static string GetCurTime() {
        return System.DateTime.Now.ToString("MM-dd  HH:mm:ss");
    }


    public static void LogPlay(string path)
    {         
        StreamWriter sw;
        FileInfo fi = new FileInfo(path);
        sw = fi.AppendText();
        sw.WriteLine(GetCurTime() + " " + SceneData.mode + "人");
        sw.Close();
        sw.Dispose();
    }

}
