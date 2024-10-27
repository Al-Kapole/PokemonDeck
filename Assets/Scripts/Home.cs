using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Toggle cacheToggle;
    private GeneralGameInfo ggi;
    private void Start()
    {
        ggi = GeneralGameInfo.Instance;
        ggi.CacheImages = cacheToggle.isOn;
    }

    /// <summary>
    /// Toggle the option to save images locally or not.
    /// </summary>
    /// <param name="value"></param>
    public void CacheToggle(bool value)
    {
        ggi.CacheImages = value;
    }

    /// <summary>
    /// Delete all saved images.
    /// </summary>
    public void DeleteCachedImages()
    {
        AssetsManager.DeleteCachedImages();
    }
}
