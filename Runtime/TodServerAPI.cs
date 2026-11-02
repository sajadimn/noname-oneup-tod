using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TodServerAPI : MonoSingleton<TodServerAPI>
{
    private const string Android = "android";
    private const string Ios = "ios";
    private const string ZarinPal = "Zarinpal";

    private TodConfig config;

    public void Request(TodServerAPIData todData, Action success, Action failed)
    {
        config ??= Resources.Load<TodConfig>(TodConfig.FileName);
        if (config == null || !TodRequest.IsSupported(todData.store))
        {
            Debug.Log($"TodServerAPI - Request - Not Available: {todData.store}");
            failed?.Invoke();
            return;
        }

        string url = TodRequest.BuildUrl(config.settleUrl, config.settleHttpUrl, todData, GetPlatform(todData.store));
        StartCoroutine(SendRequest(todData.store, url, success, failed));
    }

    private static string GetPlatform(string store)
    {
        return store != ZarinPal || Application.platform == RuntimePlatform.Android ? Android : Ios;
    }

    private IEnumerator SendRequest(string store, string url, Action success, Action failed)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = config.timeoutSeconds;
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"TodServerAPI - {store} - Response - Error: {request.error}");
                failed?.Invoke();
                yield break;
            }

            TodServerAPIResponse response = TodRequest.ReadResponse(request.downloadHandler.text);
            Debug.Log($"TodServerAPI - {store} - Response - Status: {response?.status} error: {response?.error}");
            if (response != null && response.status)
                success?.Invoke();
            else
                failed?.Invoke();
        }
    }
}
