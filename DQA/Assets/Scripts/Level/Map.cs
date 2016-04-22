using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class Map {
    public int MaxX;
    public int MaxZ;
    public Coordinate[] TreasureRooms;
    public Coordinate[] SpawnPoints;
    public Coordinate[] EmptyPoints;

    public Vector3 GridCenter { get {
            float maxX = (float)MaxX;
            float maxZ = (float)MaxZ;
            float height = maxX * 3f + maxZ * 10f;
            return new Vector3((maxX / 2f) * GameplayConstants.Instance.RoomSize, height, (maxZ / 2f) * GameplayConstants.Instance.RoomSize);
    } }
    

    public List<Coordinate> GetStandardRooms() {
        List<Coordinate> coords = new List<Coordinate>();
        for (int x = 0; x < MaxX + 1; x++) {
            for (int z = 0; z < MaxZ + 1; z++) {
                Coordinate c = new Coordinate(x, z);
                if (!TreasureRooms.Any(coord1 => coord1.Equals(c))
                    && !SpawnPoints.Any(coord2 => coord2.Equals(c))
                    && !EmptyPoints.Any(coord3 => coord3.Equals(c))) {

                    coords.Add(c);
                }
                    
            }
        }
        return coords;
    }

    public List<Coordinate> GetBorderCoords() {
        List<Coordinate> border = new List<Coordinate>();
        // Left and Right Sides
        for (int i = -1; i < MaxZ + 1; i++) {
            border.Add(new Coordinate(-1, i));
            border.Add(new Coordinate(MaxX + 1, i));
        }

        // Top and Bottom Sides
        for (int i = -1; i < MaxX + 1; i++)
        {
            border.Add(new Coordinate(i, -1));
            border.Add(new Coordinate(i, MaxZ + 1));
        }
        border.Add(new Coordinate(MaxX + 1, MaxZ + 1));
        return border;
    }
    
}
