using UnityEngine;
using System.Collections;

public class Maps {

    public static Map DefaultMap {
        get {
            Map m = new Map();
            // NOTE that the total number of cells is max + 1
            m.MaxX = 11;
            m.MaxZ = 7;
            // The center 4 squares are treasure rooms
            m.TreasureRooms = new Coordinate[]{new Coordinate(5, 3), new Coordinate(5, 4), new Coordinate(6,3), new Coordinate(6,4)};
            // Spawn points are on the four corners
            m.SpawnPoints = new Coordinate[] {new Coordinate(0,0), new Coordinate(m.MaxX, 0), new Coordinate(0, m.MaxZ), new Coordinate(m.MaxX, m.MaxZ)};
            // This still needs to be defined even if empty
            m.EmptyPoints = new Coordinate[] { };
            return m;
        }
    }
	
}
