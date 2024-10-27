public class MyDeckCard : DeckCard
{
    internal bool IsEmpty = true;

    /// <summary>
    /// Load my card's info
    /// </summary>
    /// <param name="cardInfo">Card data</param>
    /// <param name="onClicked">Action called when the card is clicked.</param>
    /// <param name="loading">If the card will start at a loading state.</param>
    public void Load(CardInfo cardInfo, System.Action<string> onClicked, bool loading = true)
    {
        IsEmpty = false;
        Load(cardInfo);
        thisButton.onClick.AddListener(ClearCard);
        thisButton.onClick.AddListener(delegate { onClicked(cardInfo.id); });
        this.loading.SetActive(loading);
    }
    /// <summary>
    /// Clear card's UI and current actions.
    /// </summary>
    public void ClearCard()
    {
        IsEmpty = true;
        nameLabel.text = "";
        cardImg.sprite = null;
        cardImg.gameObject.SetActive(false);
        thisButton.interactable = true;
        thisButton.onClick.RemoveAllListeners();
    }
}
