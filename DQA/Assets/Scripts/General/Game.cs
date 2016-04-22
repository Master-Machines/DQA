using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game {

    public static Game CurrentGame;

    public Player[] Players;

    public int CurrentPlayerIndex;
    public int CurrentRound;
    public Player CurrentPlayer;

    public Game(int playerCount) {
        Players = new Player[playerCount];
        for(int i = 0; i < Players.Length; i++) {
            Players[i] = new Player();
            Players[i].PlayerNumber = i;
        }
        CurrentPlayerIndex = -1;
        CurrentRound = 0;
        CurrentGame = this;
        IncrementRound();
    }

    public void IncrementRound() {
        if (CurrentPlayerIndex == -1)
            CurrentPlayerIndex = Random.Range(0, Players.Length);
        CurrentRound++;
    }

    public void IncrementPlayer() {
        CurrentPlayerIndex++;
        if (CurrentPlayerIndex == Players.Length)
            CurrentPlayerIndex = 0;
        CurrentPlayer = Players[CurrentPlayerIndex];
    }

	
}
