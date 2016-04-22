using UnityEngine;
using System.Collections;

public class Player {
    public int PlayerNumber;
    public PlayerController PlayerController;
    public bool Initiated;
    public RoomController CurrentRoom;

    public void LinkPlayerToController(PlayerController p) {
        PlayerController = p;
        p.player = this;
    }
}
