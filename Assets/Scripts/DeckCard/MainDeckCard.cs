public class MainDeckCard : DeckCard
{
    /// <summary>
    /// Load main card's info
    /// </summary>
    /// <param name="cardInfo">Card data</param>
    /// <param name="onClicked">Action called when the card is clicked. This action should return true or false so the card could be notified about the success of this action.</param>
    /// <param name="loading">If the card will start at a loading state.</param>
    public void Load(CardInfo cardInfo, System.Func<string, bool> onClicked, bool loading = true)
    {
        Load(cardInfo);
        thisButton.onClick.AddListener(delegate
        {
            thisButton.interactable = !onClicked(cardInfo.id);//If it returns true, deactivate the button
        });
        this.loading.SetActive(loading);
    }
    /// <summary>
    /// Make the card clickable or unclickable
    /// </summary>
    /// <param name="value"></param>
    public void EnableCard(bool value)
    {
        thisButton.interactable = value;
    }
}
