using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class RoomCreationInfo {
    public Coordinate Coords;
    public string SceneName;
    public GameObject HiddenRoomParticlesPrefab;
    public GameObject HallwayParticlesPrefab;

    public RoomCreationInfo(Coordinate spawnLocation, string sceneName)
    {
        Coords = spawnLocation;
        SceneName = sceneName;
    }

    public RoomCreationInfo(Coordinate spawnLocation, string sceneName, GameObject hiddenRoomParticles, GameObject hallwayParticlesPrefab) {
        Coords = spawnLocation;
        SceneName = sceneName;
        HallwayParticlesPrefab = hallwayParticlesPrefab;
        HiddenRoomParticlesPrefab = hiddenRoomParticles;
    }
}
