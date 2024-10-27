using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GeneralGameInfo
{
    private static GeneralGameInfo instance;
    public static GeneralGameInfo Instance
    {
        get
        {
            if (instance == null)
                instance = new GeneralGameInfo();
            return instance;
        }
    }

    public Dictionary<string, CardInfo> cards { get; set; }
    public bool CacheImages;
    public string[] rarityValues;
    public string[] typeValues;

    private DeckPosAndId[][] myDecks;
    public DeckPosAndId[][] MyDecks
    {
        get
        {
            if (myDecks == null)
            {
                myDecks = new DeckPosAndId[3][] 
                {
                    LoadDeck(0),
                    LoadDeck(1),
                    LoadDeck(2)
                };
            }
            return myDecks;
        }
        set { myDecks = value; }
    }

    public void SaveDeck(int num)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        //string path = "file:///" + Application.persistentDataPath + "/Deck.data";
        string path = UnityEngine.Application.persistentDataPath + "/Deck" + num + ".data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, myDecks[num]);
        stream.Close();
    }
    private DeckPosAndId[] LoadDeck(int deckNum)
    {
        string path = UnityEngine.Application.persistentDataPath + "/Deck" + deckNum + ".data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DeckPosAndId[] ids = formatter.Deserialize(stream) as DeckPosAndId[];
            stream.Close();

            return ids;
        }
        else
            return new DeckPosAndId[0];
    }
    [System.Serializable]
    public struct DeckPosAndId
    {
        public string Id;
        public int DeckPosition;
        public DeckPosAndId(string id, int deckPosition)
        {
            Id = id;
            DeckPosition = deckPosition;
        }
    }
}
