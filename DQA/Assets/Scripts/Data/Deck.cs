using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Deck {
    [NonSerialized]
    private List<Card> cards;

    private List<string> cardPointers;
    public string name;
    public DeckType type;

    public Deck(string Name, DeckType Type) {
        name = Name;
        type = Type;
        cardPointers = new List<string>();
        cards = new List<Card>();
    }

    public void AddCard(Card c) {
        cardPointers.Add(c.name);
        cards.Add(c);
    }




    public bool Equals(Deck other) {
        // Seperating these if tests to make the logic readable
        if( (cardPointers == null) != (other.cardPointers == null)) {
            return false;
        }

        if(name.Equals(other.name) == false || type != other.type) {
            return false;
        }
        
        if(cardPointers == null && other.cardPointers == null) {
            // Name and type are the same, and both decks have null card lists
            return true;
        }
        
        if(cardPointers.Count != other.cardPointers.Count) {
            return false;
        }

        if(cardPointers.Count == 0 && other.cardPointers.Count == 0) {
            return true;
        }

        for(int i = 0; i < cardPointers.Count; i++) {
            if(cardPointers[i].Equals(other.cardPointers[i]) == false) {
                return false;
            } 
        }

        return true;
    }
}

[Serializable]
public enum DeckType {
    Treasure
}
