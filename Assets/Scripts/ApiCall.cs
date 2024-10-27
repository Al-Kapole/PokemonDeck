using System.Collections.Generic;
using UnityEngine.Networking;
using Extensions;

public class ApiCall
{
    private const string getCardsUrl = "https://api.pokemontcg.io/v2/cards?page=_PAGE_&pageSize=_PAGESIZE_";
    private static Dictionary<string, string> headers = new Dictionary<string, string>();

    /// <summary>
    /// Ads a header or replaces it if already exists.
    /// It seems that it was not usefull for this project.
    /// </summary>
    public static void AddHeader(string key, string value)
    {
        if (headers.ContainsKey(key))
            headers[key] = value;
        else
            headers.Add(key, value);
    }
    /// <summary>
    /// API Call to retrive cards.
    /// </summary>
    /// <param name="page">The page of cards to access.</param>
    /// <param name="pageSize">The maximum amount of cards to return.</param>
    /// <param name="callback">Callback that is called once the call is completed.</param>
    public static async void GetCards(int page, int pageSize, System.Action<string, string> callback)
    {
        string url = getCardsUrl.Replace("_PAGE_", page.ToString()).Replace("_PAGESIZE_", pageSize.ToString());
        using (UnityWebRequest uwr = new UnityWebRequest(url, "GET"))
        {
            uwr.downloadHandler = new DownloadHandlerBuffer();
            foreach (KeyValuePair<string, string> header in headers)
                uwr.SetRequestHeader(header.Key, header.Value);

            UnityEngine.Debug.Log("call sent");
            await uwr.SendWebRequest();
            UnityEngine.Debug.Log("call returned");
            callback.Invoke(uwr.downloadHandler.text, uwr.error);
        }
    }
}
