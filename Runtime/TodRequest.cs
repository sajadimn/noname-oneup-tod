using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class TodRequest
{
    private const decimal RialPerToman = 10;

    public static TodStoreData GetStore(List<TodStoreData> stores, string store)
    {
        return stores.Find(item => item.store == store);
    }

    public static bool CanSettle(TodStoreData store)
    {
        return store != null && !string.IsNullOrEmpty(store.settleUrl);
    }

    public static string BuildUrl(TodStoreData store, TodServerAPIData data, string platform)
    {
        string url = $"{store.settleUrl}?store={Escape(data.store)}&receipt={Escape(data.receipt)}&pkg={Escape(data.pkg)}" +
                     $"&price={GetPriceText(store, data)}&currency={Escape(data.currency)}&userid={Escape(data.userid)}" +
                     $"&appversion={Escape(data.appversion)}&platform={Escape(platform)}&gamepkg={Escape(data.gamepkg)}" +
                     $"&firebase_app_instance_id={Escape(data.firebase_app_instance_id)}" +
                     $"&appmetricaid={Escape(data.appmetricaid)}";
        return store.isMerchantId ? $"{url}&merchant_id={Escape(data.merchant_id)}" : url;
    }

    public static TodServerAPIResponse ReadResponse(string text)
    {
        if (string.IsNullOrEmpty(text))
            return null;

        try
        {
            return JsonUtility.FromJson<TodServerAPIResponse>(text);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    private static string GetPriceText(TodStoreData store, TodServerAPIData data)
    {
        decimal price = (decimal)data.price;
        return (store.isRialPerToman ? price * RialPerToman : price).ToString(CultureInfo.InvariantCulture);
    }

    private static string Escape(string value)
    {
        return Uri.EscapeDataString(value ?? string.Empty);
    }
}
