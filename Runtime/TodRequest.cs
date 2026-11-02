using System;
using System.Globalization;
using UnityEngine;

public static class TodRequest
{
    private const string CafeBazaar = "Cafebazaar";
    private const string Myket = "Myket";
    private const string GooglePlay = "GooglePlay";
    private const string ZarinPal = "Zarinpal";
    private const decimal RialPerToman = 10;

    public static bool IsSupported(string store)
    {
        return store == CafeBazaar || store == Myket || store == GooglePlay || store == ZarinPal;
    }

    public static string BuildUrl(string settleUrl, string settleHttpUrl, TodServerAPIData data, string platform)
    {
        string baseUrl = data.store == CafeBazaar || data.store == Myket ? settleHttpUrl : settleUrl;
        string url = $"{baseUrl}?store={Escape(data.store)}&receipt={Escape(data.receipt)}&pkg={Escape(data.pkg)}" +
                     $"&price={GetPriceText(data)}&currency={Escape(data.currency)}&userid={Escape(data.userid)}" +
                     $"&appversion={Escape(data.appversion)}&platform={Escape(platform)}&gamepkg={Escape(data.gamepkg)}" +
                     $"&firebase_app_instance_id={Escape(data.firebase_app_instance_id)}" +
                     $"&appmetricaid={Escape(data.appmetricaid)}";
        return data.store == ZarinPal ? $"{url}&merchant_id={Escape(data.merchant_id)}" : url;
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

    private static string GetPriceText(TodServerAPIData data)
    {
        decimal price = (decimal)data.price;
        return (data.store == GooglePlay ? price : price * RialPerToman).ToString(CultureInfo.InvariantCulture);
    }

    private static string Escape(string value)
    {
        return Uri.EscapeDataString(value ?? string.Empty);
    }
}
