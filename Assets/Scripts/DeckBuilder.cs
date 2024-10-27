using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckBuilder : MonoBehaviour
{
    public enum CardOrder { NONE, HP, TYPE, RARITY}
    private CardOrder currentOrder;

    [SerializeField]
    private Button orderBtn;
    [SerializeField]
    private Text orderLabel;

    [SerializeField]
    private Transform cardsHolder;
    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private MyDecks myDecks;

    private CanvasGroup cardsHolderCG;
    private MainDeckCard[] deckCards;
    private GeneralGameInfo ggi;

    private int lastImageLoadedIndex;
    private bool imagesCompleted;
    private bool pauseImageLoading;

    void OnEnable()
    {
        if (!imagesCompleted && pauseImageLoading)
        {
            pauseImageLoading = false;
            LoadImages();
        }
    }
    void OnDisable()
    {
        if (!imagesCompleted)
            pauseImageLoading = true;
    }

    private async void Start()
    {
        ggi = GeneralGameInfo.Instance;

        currentOrder = CardOrder.NONE;
        orderLabel.text = currentOrder.ToString();
        cardsHolderCG = cardsHolder.GetComponent<CanvasGroup>();

        int index = 0;
        int length = ggi.cards.Count;
        deckCards = new MainDeckCard[length];
        foreach(CardInfo card in ggi.cards.Values)
        {
            deckCards[index] = Instantiate(cardPrefab, cardsHolder, false).GetComponent<MainDeckCard>();
            deckCards[index].Load(card, AddToMyDeck);
            index++;
            await System.Threading.Tasks.Task.Yield();
        }
        lastImageLoadedIndex = 0;
        LoadImages();

        myDecks.Load(EnableDeckCard);
    }

    /// <summary>
    /// Load images asynchronously by order, one after an other.
    /// </summary>
    private async void LoadImages()
    {
        int length = deckCards.Length;
        for (int i = lastImageLoadedIndex; i < length; i++)
        {
            await deckCards[i].LoadImage();
            lastImageLoadedIndex++;
            if (pauseImageLoading)
                break;
        }

        if (lastImageLoadedIndex == length)
            imagesCompleted = true;
    }
    /// <summary>
    /// Add a card to my deck
    /// </summary>
    /// <param name="id">card's id</param>
    /// <returns>If card added succefully returns true.</returns>
    public bool AddToMyDeck(string id)
    {
        return myDecks.AddCard(id);
    }
    /// <summary>
    /// Enable or disable a card from the main deck where you pick cards from.
    /// </summary>
    /// <param name="id">card's id</param>
    /// <param name="value">True to enable, false to disable the card</param>
    private void EnableDeckCard(string id, bool value)
    {
        int length = deckCards.Length;
        foreach(MainDeckCard dCard in deckCards)//Could be better
        {
            if(dCard.CardId == id)
            {
                dCard.EnableCard(value);
                break;
            }
        }
    }


    #region Choosing order
    /// <summary>
    /// Change the order of the cards by the next order option.
    /// </summary>
    public void ReorderCards()
    {
        orderBtn.interactable = false;
        switch (currentOrder)
        {
            case CardOrder.TYPE://Then change to HP
                currentOrder = CardOrder.HP;
                OrderByHP();
                break;
            case CardOrder.HP://Then change to RARITY
                currentOrder = CardOrder.RARITY;
                OrderBy(currentOrder);
                break;
            case CardOrder.RARITY://Then change to TYPE
            case CardOrder.NONE:
                currentOrder = CardOrder.TYPE;
                OrderBy(currentOrder);
                break;
        }
        orderLabel.text = currentOrder.ToString();
    }

    /// <summary>
    /// Order the cards by the given option.
    /// Only works with TYPE and RARITY
    /// </summary>
    /// <param name="order">The option by which the order should be done</param>
    private async void OrderBy(CardOrder order)
    {
        Dictionary<string, List<DeckCard>> dict = new Dictionary<string, List<DeckCard>>();
        #region by type
        if (order == CardOrder.TYPE)
        {
            foreach (string type in ggi.typeValues)
                dict.Add(type, new List<DeckCard>());
            foreach (DeckCard card in deckCards)
                dict[card.CardType].Add(card);
        }
        #endregion
        #region by rarity
        else //If by rarity
        {
            foreach (string rarity in ggi.rarityValues)
                dict.Add(rarity, new List<DeckCard>());
            foreach (DeckCard card in deckCards)
                dict[card.CardRarity].Add(card);
        }
        #endregion
        await FadeTransition.FadeCanvas(cardsHolderCG, FadeTransition.Fade.OUT,0.3f);

        int index = 0;
        foreach(string key in dict.Keys)
        {
            List<DeckCard> cards = dict[key];
            foreach(DeckCard card in cards)
            {
                card.OrderNum = index;
                index++;
            }
        }
        await FadeTransition.FadeCanvas(cardsHolderCG, FadeTransition.Fade.IN, 0.3f);
        orderBtn.interactable = true;
    }

    /// <summary>
    /// Order the cards by HP
    /// </summary>
    private async void OrderByHP()
    {
        MainDeckCard temp;
        int length = deckCards.Length;
        for (int i = 0; i < length - 1; i++)
        {
            for (int j = i + 1; j < length; j++)
            {
                if (deckCards[i].CardHP < deckCards[j].CardHP)
                {
                    temp = deckCards[i];
                    deckCards[i] = deckCards[j];
                    deckCards[j] = temp;
                }
            }
        }

        await FadeTransition.FadeCanvas(cardsHolderCG, FadeTransition.Fade.OUT, 0.3f);
        for (int i = 0; i < length; i++)
            deckCards[i].OrderNum = i;
        await FadeTransition.FadeCanvas(cardsHolderCG, FadeTransition.Fade.IN, 0.3f);
        orderBtn.interactable = true;
    }
    #endregion
}
