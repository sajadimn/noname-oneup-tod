using System.Collections.Generic;
using UnityEngine;

public class TodConfig : ScriptableObject
{
    public const string FileName = "TodConfig";

    public List<TodStoreData> stores = new List<TodStoreData>
    {
        new TodStoreData { store = "Cafebazaar", isRialPerToman = true },
        new TodStoreData { store = "Myket", isRialPerToman = true },
        new TodStoreData { store = "GooglePlay" },
        new TodStoreData { store = "Zarinpal", isRialPerToman = true, isMerchantId = true },
    };
    public int timeoutSeconds = 15;
}
