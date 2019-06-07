using UnityEngine;
using UnityEditor;

public class DataClear : MonoBehaviour
{

    [MenuItem("Tools/删除数据/清除全部")]
    public static void ClearAll()
    {
        ClearMeetFish();
        ClearMeetFishCount();
        ClearRank();
        ClearFishCount();
        ClearFarthest();
        ClearTotalFishType();
    }

    [MenuItem("Tools/删除数据/删除是否遇到鱼")]
    public static void ClearMeetFish()
    {

        bool[] array = PlayerPrefsX.GetBoolArray("FishType", false, 17);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = false;
        }
        PlayerPrefsX.SetBoolArray("FishType", array);
        Debug.Log("Complete! ");
    }

    [MenuItem("Tools/删除数据/删除遇到鱼次数")]
    public static void ClearMeetFishCount()
    {

        int[] arrayInt = PlayerPrefsX.GetIntArray("FishCountArray", 0, 17);
        for (int i = 0; i < arrayInt.Length; i++)
        {
            arrayInt[i] = 0;
        }
        PlayerPrefsX.SetIntArray("FishCountArray", arrayInt);
        Debug.Log("Complete! ");
    }
 

    [MenuItem("Tools/删除数据/删除名次")]
    public static void ClearRank()
    {
        float[] data = PlayerPrefsX.GetFloatArray("Rank", 0, 10);
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = 0f;
        }
        PlayerPrefsX.SetFloatArray("Rank", data);
        Debug.Log("Complete! ");
    }

    [MenuItem("Tools/删除数据/抓获的总鱼数")]
    public static void ClearFishCount()
    {
        PlayerPrefs.SetInt("FishCount", 0);
        Debug.Log("Complete! ");
    }

    [MenuItem("Tools/删除数据/最远距离")]
    public static void ClearFarthest()
    {
        PlayerPrefs.SetFloat("Farthest", 0f);
        Debug.Log("Complete! ");
    }

    [MenuItem("Tools/删除数据/遇到鱼的种数")]
    public static void ClearTotalFishType()
    {
        PlayerPrefs.SetInt("TotalFishType", 0);
        Debug.Log("Complete! ");
    }


}