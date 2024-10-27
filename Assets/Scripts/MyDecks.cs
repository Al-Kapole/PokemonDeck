using UnityEngine;
using System.Collections.Generic;
using static GeneralGameInfo;

public class MyDecks : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup myDeckCG;
    [SerializeField]
    private Transform cardsHolder;

    private MyDeckCard[] dCards;
    private System.Action<string, bool> enableCardFromMainDeck;
    private GeneralGameInfo ggi;
    private int currentDeck;

    /// <summary>
    /// Load My Deck.
    /// Get important components and load locally saved decks data.
    /// </summary>
    /// <param name="enableCardFromMainDeck">The method to be called when a card from the main deck should be enabled or disabled.</param>
    public void Load(System.Action<string, bool> enableCardFromMainDeck)
    {
        ggi = GeneralGameInfo.Instance;
        this.enableCardFromMainDeck = enableCardFromMainDeck;
        dCards = cardsHolder.GetComponentsInChildren<MyDeckCard>();

        ShowLoadedDeck(ggi.MyDecks[0]);
    }

    /// <summary>
    /// Add a card at my deck at the next available position.
    /// </summary>
    /// <param name="id">The id of the card.</param>
    /// <returns>Returnes true if the card was added succefully.</returns>
    public bool AddCard(string id)
    {
        int length = dCards.Length;
        for (int i = 0; i < length; i++)
        {
            if(dCards[i].IsEmpty)
            {
                dCards[i].Load(ggi.cards[id], delegate { enableCardFromMainDeck(id, true); SaveDeck(); }, false);
                dCards[i].LoadImage();
                SaveDeck();
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Add a card at my deck at a specific position.
    /// </summary>
    /// <param name="pos">The number of the position.</param>
    /// <param name="id">The id of the card.</param>
    private void AddCard(int pos, string id)
    {
        enableCardFromMainDeck.Invoke(id, false);
        dCards[pos].Load(ggi.cards[id], delegate { enableCardFromMainDeck(id, true); SaveDeck(); }, false);
        dCards[pos].LoadImage();
    }
    
    /// <summary>
    /// This is called by a toggle once it is on.
    /// </summary>
    /// <param name="value">The value passed by the toggle. Representing the number of the deck.</param>
    public void ChangeDeck(int value)
    {
        if (currentDeck == value)
            return;
        SwitchDeck(value);
    }
    
    /// <summary>
    /// Switching from current deck to the selected one.
    /// </summary>
    /// <param name="value">Number of the selected deck.</param>
    public async void SwitchDeck(int value)
    {
        await FadeTransition.FadeCanvas(myDeckCG, FadeTransition.Fade.OUT, 0.3f);
        ClearCurrentDeck();
        currentDeck = value;
        ShowLoadedDeck(ggi.MyDecks[currentDeck]);
        await FadeTransition.FadeCanvas(myDeckCG, FadeTransition.Fade.IN, 0.3f);
    }

    /// <summary>
    /// Clears all all cards values at my deck.
    /// </summary>
    private void ClearCurrentDeck()
    {
        foreach (MyDeckCard dCard in dCards)
        {
            if (!dCard.IsEmpty)
            {
                enableCardFromMainDeck.Invoke(dCard.CardId, true);
                dCard.ClearCard();
            }
        }
    }

    /// <summary>
    /// Shows a saved deck at my deck.
    /// </summary>
    /// <param name="dpais">The saved values of ids and their positions.</param>
    private void ShowLoadedDeck(DeckPosAndId[] dpais)
    {
        foreach (DeckPosAndId dpai in dpais)
            AddCard(dpai.DeckPosition, dpai.Id);
    }

    /// <summary>
    /// Saving a deck.
    /// </summary>
    private void SaveDeck()
    {
        int length = dCards.Length;
        List<DeckPosAndId> ids = new List<DeckPosAndId>();
        for (int i = 0; i < length; i++)
        {
            if (!dCards[i].IsEmpty)
                ids.Add(new DeckPosAndId(dCards[i].CardId, i));
        }
        ggi.MyDecks[currentDeck] = ids.ToArray();
        ggi.SaveDeck(currentDeck);
    }
    

}
