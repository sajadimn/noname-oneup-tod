using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TodServerAPI : MonoSingleton<TodServerAPI>
{
    private const string Android = "android";
    private const string Ios = "ios";

    private TodConfig config;

    public void Request(TodServerAPIData todData, Action success, Action failed)
    {
        config ??= Resources.Load<TodConfig>(TodConfig.FileName);
        TodStoreData store = config != null ? TodRequest.GetStore(config.stores, todData.store) : null;
        if (!TodRequest.CanSettle(store))
        {
            Debug.Log($"TodServerAPI - Request - Not Available: {todData.store}");
            failed?.Invoke();
            return;
        }

        string url = TodRequest.BuildUrl(store, todData, GetPlatform());
        StartCoroutine(SendRequest(todData.store, url, success, failed));
    }

    private static string GetPlatform()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer ? Ios : Android;
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
