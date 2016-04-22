using UnityEngine;
using System.Collections;

public class GameplayConstants
{
    private static GameplayConstants instance;
    public static GameplayConstants Instance
    {
        get
        {
            if (instance == null)
                instance = new GameplayConstants();
            return instance;
        }
    }

    public float RoomSize = 10f;


    public string treasureRoomName = "treasureRoom";
    public string spawnRoomName = "spawnRoom";
    public int totalNumberOfStandardRooms = 1;
    public string standardRoomNamePrefix = "room";
}
