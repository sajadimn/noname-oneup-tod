using UnityEngine;

public class TodConfig : ScriptableObject
{
    public const string FileName = "TodConfig";

    public string settleUrl = "";
    public string settleHttpUrl = "";
    public int timeoutSeconds = 15;
}
