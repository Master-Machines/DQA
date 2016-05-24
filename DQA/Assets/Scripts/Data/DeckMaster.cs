using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
// Handles the saving, loading, exporting, and importing of decks and cards.
public class DeckMaster {
    private const string DeckFolder = "Decks/";
    private const string CardFolder = "Cards/";
    private bool logging = true;
    private List<Deck> decks;
    private List<Card> cards;
    public Dictionary<string, string> defaultDecks;

    public DeckMaster() {
        LoadCards();
        LoadDecks();
    }

    public void LoadCards() {
        cards = new List<Card>();
        FileInfo[] files = SaveMaster.getFiles(CardFolder);
        foreach(FileInfo f in files) {
            
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream file = f.OpenRead();
            Card c = (Card)bf.Deserialize(file);
            Debug.Log("Reading card: " + c.name);
            cards.Add(c);
            file.Close();
        }
    }

    public void LoadDecks() {
        decks = new List<Deck>();
    }

    public void AddDeck(Deck d) {
        decks.Add(d);
    }

    public void AddCard(Card c) {
        cards.Add(c);
    }

    public void SaveAllCards() {
        foreach(Card c in cards) {
            SaveCard(c);
        }
    }

    public void SaveCard(Card c) {
        DebugLog("Starting to save card: " + c.name);
        BinaryFormatter formatter = new BinaryFormatter();

        using (MemoryStream stream = new MemoryStream()) {
            formatter.Serialize(stream, c);
            Byte[] byteArray = stream.ToArray();
            SaveMaster.WriteFile(byteArray, CardFolder, c.name);
            DebugLog("Should have saved!");
        }
    }

    public void SaveAllDecks() {
        DebugLog("Saving all decks");
        // TODO: Can we make decks "dirty" and only save those?
        foreach(Deck d in decks) {
            if(d.name != null && d.name.Length > 0)
                SaveDeck(d);
        }
    }

    public void SaveDeck(Deck d) {
        DebugLog("Starting to save: " + d.name);
        BinaryFormatter formatter = new BinaryFormatter();

        using (MemoryStream stream = new MemoryStream()) {
            formatter.Serialize(stream, d);
            Byte[] byteArray = stream.ToArray();
            SaveMaster.WriteFile(byteArray, DeckFolder, d.name);
            DebugLog("Should have saved!");
        }
    }

    public Card GetCard(string name) {
        foreach(Card c in cards) {
            if(c.name.Equals(name)) {
                return c;
            }
        }
        return null;
    }
    

    private void DebugLog(string message) {
        if(logging) {
            Debug.Log("DeckManager: " + message);
        }
    }
}
