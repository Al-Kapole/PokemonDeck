using UnityEngine;
using SimpleJSON;
using Extensions;
using System.Collections.Generic;

public class Loading : MonoBehaviour
{
    public int numberOfCards = 50;
    [Tooltip("If true it downloads all the images at the start of the application")]
    public bool FetchImagesOnStart;
    public UnityEngine.Events.UnityEvent onFinished;

    private GeneralGameInfo ggi;

    void Start()
    {
        ApiCall.GetCards(1, numberOfCards, GotCards);
    }
    /// <summary>
    /// Called when the api call returned.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="error"></param>
    private void GotCards(string response, string error)
    {
        CreateStructs(JSON.Parse(response));
    }
    /// <summary>
    /// Parses the given json to create structs for each card.
    /// </summary>
    /// <param name="jsonData">The data in json format</param>
    private void CreateStructs(JSONNode jsonData)
    {
        ggi = GeneralGameInfo.Instance;
        List<string> rarityValues = new List<string>();
        List<string> typeValues = new List<string>();

        JSONNode data = jsonData["data"];
        int length = data.Count;
        ggi.cards = new Dictionary<string, CardInfo>();
        for (int i = 0; i < length; i++)
        {
            string id = data[i]["id"].ToString().TrimSideQuotes();
            CardInfo newCard = new CardInfo
            (
                id,
                data[i]["name"].ToString().TrimSideQuotes(),
                data[i]["types"][0].ToString().TrimSideQuotes(),
                data[i]["hp"].AsInt,
                data[i]["rarity"].ToString().TrimSideQuotes(),
                data[i]["images"]["small"].ToString().TrimSideQuotes(),
                data[i]["images"]["large"].ToString().TrimSideQuotes()
            );
            ggi.cards[id] = newCard;

            //Save rarity values that returned at a list
            if (!rarityValues.Contains(newCard.rarity))
                rarityValues.Add(newCard.rarity);
            //Save type values that returned at a list
            if (!typeValues.Contains(newCard.type))
                typeValues.Add(newCard.type);
        }
        //Keep available type and rarity values to use them for reordering.
        ggi.rarityValues = rarityValues.ToArray();
        ggi.typeValues = typeValues.ToArray();

        if (FetchImagesOnStart)
            FetchImages();
        else
            onFinished?.Invoke();
    }

    /// <summary>
    /// Download all images.
    /// </summary>
    private async void FetchImages()
    {
        Dictionary<string, CardInfo> cards = ggi.cards;
        foreach(CardInfo card in cards.Values)
        {
            await card.sprites.GetSmallSprite();
            Debug.Log("Image ready for: " + card.name);
        }
        onFinished?.Invoke();
    }
}
