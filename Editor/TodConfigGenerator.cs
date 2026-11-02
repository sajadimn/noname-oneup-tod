using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class TodConfigGenerator
{
    public const string ConfigPath = "Assets/Settings/TOD/Resources/" + TodConfig.FileName + ".asset";

    static TodConfigGenerator()
    {
        EditorApplication.delayCall += Generate;
    }

    [MenuItem("OneUp/TOD/Create Missing Config")]
    public static void Generate()
    {
        CreateFolder("Assets", "Settings");
        CreateFolder("Assets/Settings", "TOD");
        CreateFolder("Assets/Settings/TOD", "Resources");

        if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(ConfigPath, AssetPathToGUIDOptions.OnlyExistingAssets)))
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TodConfig>(), ConfigPath);
    }

    [MenuItem("OneUp/TOD/Config")]
    private static void OpenConfig()
    {
        Generate();
        Object asset = AssetDatabase.LoadMainAssetAtPath(ConfigPath);
        Selection.activeObject = asset;
        EditorGUIUtility.PingObject(asset);
    }

    private static void CreateFolder(string parent, string name)
    {
        if (!AssetDatabase.IsValidFolder($"{parent}/{name}"))
            AssetDatabase.CreateFolder(parent, name);
    }
}
