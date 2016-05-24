using UnityEngine;
using System.Collections;

public class DeckCreationTest : MonoBehaviour {

	void Start () {
        BeginTests();
	}
	
    void BeginTests() {
        // Create cards and save the data.
        CreateCards();

        // Reset loaded data. This will test loading new data.
        ResetData();

        CreateDeck();
    }

    void CreateCards() {
        Card c1 = new Card("cardDerp1", "wow", "derp");
        Card c2 = new Card("cardDerp2", "wow", "wut");

        SaveMaster.CurrentDeckMaster().AddCard(c1);
        SaveMaster.CurrentDeckMaster().AddCard(c2);
        SaveMaster.Save();
    }

    void ResetData() {
        SaveMaster.ResetDeckMaster();
    }

    void CreateDeck() {
        

        Deck d1 = new Deck("KingDeck", DeckType.Treasure);
        d1.AddCard(SaveMaster.CurrentDeckMaster().GetCard("cardDerp1"));
        d1.AddCard(SaveMaster.CurrentDeckMaster().GetCard("cardDerp2"));
        SaveMaster.CurrentDeckMaster().AddDeck(d1);
        SaveMaster.CurrentDeckMaster().SaveAllDecks();
        
    }
}
