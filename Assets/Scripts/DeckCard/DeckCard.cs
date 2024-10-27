using UnityEngine;
using UnityEngine.UI;

public class DeckCard : MonoBehaviour
{
    public string CardId { get { return cardInfo.id; } }
    public string CardType { get { return cardInfo.type; } }
    public int CardHP { get { return cardInfo.hp; } }
    public string CardRarity { get { return cardInfo.rarity; } }
    public int OrderNum { set { transform.SetSiblingIndex(value); } }


    [SerializeField]
    protected Text nameLabel;
    [SerializeField]
    protected GameObject loading;
    [SerializeField]
    protected Image cardImg;

    protected Button thisButton;
    private CardInfo cardInfo;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
    }
    /// <summary>
    /// Base info load for UI
    /// </summary>
    /// <param name="cardInfo">Card data</param>
    protected void Load(CardInfo cardInfo)
    {
        this.cardInfo = cardInfo;
        nameLabel.text = cardInfo.name;
        thisButton.interactable = true;
    }
    /// <summary>
    /// Set the Image of the card.
    /// </summary>
    /// <returns></returns>
    public async System.Threading.Tasks.Task LoadImage()
    {
        cardImg.sprite = await GeneralGameInfo.Instance.cards[cardInfo.id].sprites.GetSmallSprite();
        FadeTransition.FadeIn(cardImg, 0.5f, ImageReady);
    }
    /// <summary>
    /// It is called when the iamge is ready and being displayed.
    /// </summary>
    private void ImageReady()
    {
        loading.SetActive(false);
    }

}
