/// <summary>
/// A struct that holds the information of a card
/// </summary>
public struct CardInfo
{
    public string id { get; }
    public string name { get; }
    public string type { get; }
    public int hp { get; }
    public string rarity { get; }
    public Sprites sprites { get; set; }

    public CardInfo(string id, string name, string type, int hp, string rarity, string imageUrlSmall, string imageUrlLarge)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.hp = hp;
        this.rarity = rarity;
        sprites = new Sprites(imageUrlSmall, imageUrlLarge);
    }
    /// <summary>
    /// A class that holds information about the sprites of a card.
    /// It is a class and not a struct cause Sprite values are not saved at a struct
    /// </summary>
    public class Sprites
    {
        public string UrlSmall { get; private set; }
        public string UrlLarge { get; private set; }
        private UnityEngine.Sprite small;
        private UnityEngine.Sprite large;

        public Sprites(string imageUrlSmall, string imageUrlLarge)
        {
            UrlSmall = imageUrlSmall;
            UrlLarge = imageUrlLarge;
            small = null;
            large = null;
        }
        /// <summary>
        /// Returns card's small size image.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<UnityEngine.Sprite> GetSmallSprite()
        {
            if (small == null)
                small = await AssetsManager.GetImageAsSprite(UrlSmall, GeneralGameInfo.Instance.CacheImages);
            return small;
        }
        /// <summary>
        /// Returns card's large size image.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<UnityEngine.Sprite> GetLargeSprite()
        {
            if (large == null)
                large = await AssetsManager.GetImageAsSprite(UrlLarge, GeneralGameInfo.Instance.CacheImages);
            return large;
        }
    }
}