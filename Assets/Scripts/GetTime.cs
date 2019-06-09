using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetTime
{
    
    public static bool filp = false;

    public static bool IsDay() {
        bool value = true; 
        if (System.DateTime.Now.Hour > 18 || System.DateTime.Now.Hour < 6) {
            value = false;
        }
        if (filp)
            value = !value;
        return value;
    }

    public static string GetCurTime() {
        return System.DateTime.Now.ToString("MM-dd  HH:mm:ss");
    }


    public static void LogPlay(string path)
    {
        //Debug.Log(System.DateTime.Now.Hour);
        StreamWriter sw;
        FileInfo fi = new FileInfo(path);
        sw = fi.AppendText();
        sw.WriteLine(GetCurTime() + " " + SceneData.mode + "人");
        sw.Close();
        sw.Dispose();
    }

}
