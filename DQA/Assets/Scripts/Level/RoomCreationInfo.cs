using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class RoomCreationInfo {
    public Coordinate Coords;
    public string SceneName;
    public GameObject HiddenRoomParticlesPrefab;

    public RoomCreationInfo(Coordinate spawnLocation, string sceneName)
    {
        Coords = spawnLocation;
        SceneName = sceneName;
    }

    public RoomCreationInfo(Coordinate spawnLocation, string sceneName, GameObject hiddenRoomParticles) {
        Coords = spawnLocation;
        SceneName = sceneName;
        HiddenRoomParticlesPrefab = hiddenRoomParticles;
    }
}
