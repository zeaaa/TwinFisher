using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        //int playTimes = ReadFileLines(path) - 1;



        //StreamWriter sw;
        //FileInfo fi = new FileInfo(path);
        //sw = fi.AppendText();
        //sw.WriteLine(GetCurTime() + " " + SceneData.mode + "人");
        //sw.Close();
        //sw.Dispose();
        List<string> list = new List<string>();

        int lines = 0;
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamReader sr = new StreamReader(fs);
        string s;
        //读入文件所有行，存放到List<string>集合中
        while ((s = sr.ReadLine()) != null)
        {
            list.Add(s);
        }
        fs.Close();
        sr.Close();

        StreamWriter sw;
        sw = new StreamWriter(path);
        if(list.Count ==0)
            sw.WriteLine("1人");
        else
            sw.WriteLine("共累计" + list.Count.ToString() + "次游玩");
        for (int i = 1; i < list.Count; i++)
        {
            sw.WriteLine(list[i].Replace("\r\n", ""));
        }
        sw.WriteLine(GetCurTime() + " " + SceneData.mode + "人模式");
        sw.Close();
        sw.Dispose();

        //string s = System.IO.File.ReadAllText(@"e:\xxx\xxx.txt");//有编码注意加上编码，要不默认ansi，和系统默认编码有关
        //string[] arr = s.Replace("\r", "").Split('\n');
        //arr[0] = "xxxxxxxxxxxxxxxx";
        //System.IO.File.WriteAllText(@"e:\xxx\xxx.txt", string.Join("\r\n", arr));

    }

  

    public static byte[] int2ByteArray(int i)
    {
                 byte[] result = new byte[4];
                result[0] = (byte)((i >> 24) & 0xFF);
               result[1] = (byte)((i >> 16) & 0xFF);
               result[2] = (byte)((i >> 8) & 0xFF);
              result[3] = (byte)(i & 0xFF);
               return result;
      }

}
