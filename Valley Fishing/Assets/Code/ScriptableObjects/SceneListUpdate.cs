using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEditor;

[InitializeOnLoad]
//A quirky little class that keeps the Scene Names enum up to date.
public static class SceneListUpdate
{
    static SceneListUpdate()
    {
        EditorBuildSettings.sceneListChanged += Generate;
        EditorApplication.delayCall += Generate;
    }

    private static void Generate()
    {
        List<string> sceneNames = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled)
                continue;

            sceneNames.Add(Regex.Replace(scene.path.Split('/').Last().Split('.').First(), @"[\s_]|^0(?=\d)", ""));
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public enum SceneNames");
        sb.AppendLine("{");

        foreach (var name in sceneNames)
        {
            sb.AppendLine($"    {replaceNumbersWithEnglishWords(name)},");
        }

        sb.AppendLine("}");
        File.WriteAllText("Assets/Code/ScriptableObjects/Enums/SceneNames.cs", sb.ToString());

        AssetDatabase.Refresh();
    }

    private static string replaceNumbersWithEnglishWords(string input)
    {
        Dictionary<string, string> numberWords = new Dictionary<string, string>
        {
            { "0", "Zeroth" },
            { "1", "First" },
            { "2", "Second" },
            { "3", "Third" },
            { "4", "Fourth" },
            { "5", "Fifth" },
            { "6", "Sixth" },
            { "7", "Seventh" },
            { "8", "Eighth" },
            { "9", "Ninth" }
        };
        foreach (var item in numberWords)
        {
            input = input.Replace(item.Key, item.Value);
        }
        return input;
    }
}