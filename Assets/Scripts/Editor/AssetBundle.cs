using System.IO;
using UnityEditor;
using UnityEngine;
public class AssetBundle
{
    [MenuItem("AssetBundle/Package (Default)")]
    private static void PackageBuddle()
    {
        Debug.Log("Packaging AssetBundle...");
#if UNITY_STANDALONE_WIN
        /*
        string packagePath = UnityEditor.EditorUtility.OpenFolderPanel("Select Package Path", "C:/Users/PVer/TwinFisher/", "");
        if (packagePath.Length <= 0 || !Directory.Exists(packagePath))
            return;
        Debug.Log("Output Path: " + packagePath);
        */
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
#endif

#if UNITY_ANDROID
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
#endif
#if UNITY_STANDALONE_OSX
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);
#endif
        AssetDatabase.Refresh();

    }

    
}
