using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Extensions;

public class AssetsManager
{
    private static System.Collections.Generic.List<string> downloading = new System.Collections.Generic.List<string>();

    private const string baseImagesUrl = "https://images.pokemontcg.io/";
    private const string localFolder = "CardImages/";

    /// <summary>
    /// Returns a sprite from the given url.
    /// It caches the image once it is downloaded. If there is already a cached image it returns this one.
    /// </summary>
    /// <param name="url">The url of the image.</param>
    /// <param name="cacheImage">Saves the image locally so it won't have to redownload it.</param>
    /// <returns></returns>
    public static async Task<Sprite> GetImageAsSprite(string url, bool cacheImage = true)
    {
        string localPath = Path.Combine(Application.persistentDataPath, localFolder + url.Replace(baseImagesUrl, ""));
        if (downloading.Contains(url))//If sprite is already downloading wait for it
        {
            while (downloading.Contains(url))
                await Task.Yield();
            Sprite sprite = await LoadCachedImage(localPath);
            return sprite;
        }
        else if (!File.Exists(localPath))//If file does not exist download it
        {
            downloading.Add(url);
            Sprite sprite = await DownloadImage(url, localPath, cacheImage);
            downloading.Remove(url);

            return sprite;
        }
        else//If file exist return it
            return await LoadCachedImage(localPath);
    }
    /// <summary>
    /// Download the image.
    /// </summary>
    /// <param name="url">Network url.</param>
    /// <param name="savePath">local path for the image to be saved.</param>
    /// <param name="cacheImage">Saves the image locally so it won't have to redownload it.</param>
    /// <returns></returns>
    private static async Task<Sprite> DownloadImage(string url, string savePath, bool cacheImage)
    {
        Sprite sprite = null;
        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
        await req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            Texture2D tex = ((DownloadHandlerTexture)req.downloadHandler).texture;
            sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            if (cacheImage)
                SaveImageFile(savePath, tex);
        }
        return sprite;
    }


    #region Cache image
    /// <summary>
    /// Loads an image from local storage.
    /// </summary>
    /// <param name="localPath">Path of the image that might be located.</param>
    /// <returns></returns>
    public static async Task<Sprite> LoadCachedImage(string localPath)
    {
        Sprite sprite = null;
        localPath = "file:///" + localPath;

        UnityWebRequest req = UnityWebRequestTexture.GetTexture(localPath);
        await req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            Texture2D tex = ((DownloadHandlerTexture)req.downloadHandler).texture;
            sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        return sprite;
    }
    /// <summary>
    /// Saves the image locally.
    /// </summary>
    /// <param name="localPath">local path for the image to be saved.</param>
    /// <param name="texture">Texture to be saved.</param>
    public static void SaveImageFile(string localPath, Texture2D texture)
    {
        string directoryPath = Path.GetDirectoryName(localPath);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        File.WriteAllBytes(localPath, texture.EncodeToPNG());
    }
    /// <summary>
    /// Deletes all cached images
    /// </summary>
    public static void DeleteCachedImages()
    {
        string localPath = Path.Combine(Application.persistentDataPath, localFolder);
        if (Directory.Exists(localPath))
            Directory.Delete(localPath, true);
    }
    #endregion
}
