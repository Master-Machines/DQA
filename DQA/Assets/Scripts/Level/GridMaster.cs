﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

// Used for creating and managing rooms in the level
public class GridMaster : MonoBehaviour {
    public static GridMaster Instance;

    public GameObject HiddenRoomParticlesPrefab;
    public GameObject BorderPrefab;
    public GameObject PlayerPrefab;

    private Queue<RoomCreationInfo> roomCreationQueue;

    void Awake() {
        Instance = this;
    }

    public Map CurrentMap { get; internal set; }

    private List<RoomController> grid;
    private List<RoomController> spawnPoints;
    private List<GameObject> border;
    private bool spawningRoom = false;
    private bool levelLoaded;

    void Start() {
        SetupMap(Maps.DefaultMap);
    }

    public void SetupMap(Map map) {
        CurrentMap = map;
        grid = new List<RoomController>();
        roomCreationQueue = new Queue<RoomCreationInfo>();
        setupSpawnRooms();
        setupTreasureRooms();
        createBorder();
    }

    public RoomCreationInfo AddToGrid(RoomController room) {
        spawningRoom = false;
        RoomCreationInfo infoToPass = roomCreationQueue.Dequeue();
        checkQueue();
        grid.Add(room);
        if (room.RoomType == RoomType.Spawn)
            spawnPoints.Add(room);
        return infoToPass;
    }

    private void createBorder() {
        border = new List<GameObject>();
        List<Coordinate> borderCoords = CurrentMap.GetBorderCoords();
        foreach (Coordinate c in borderCoords) {
            border.Add((GameObject)Instantiate(BorderPrefab, c.GetWorldPosition(), Quaternion.identity));
        }
    }

    private void setupSpawnRooms() {
        spawnPoints = new List<RoomController>();
        for (int i = 0; i < CurrentMap.SpawnPoints.Length; i++) {
            createRoom(new RoomCreationInfo(CurrentMap.SpawnPoints[i], GameplayConstants.Instance.spawnRoomName));
        }
    }

    private void setupTreasureRooms() {
        for (int i = 0; i < CurrentMap.TreasureRooms.Length; i++) {
            createRoom(new RoomCreationInfo(CurrentMap.TreasureRooms[i], GameplayConstants.Instance.treasureRoomName));
        }
    }

    private void createRoom(RoomCreationInfo creationInfo) {
        roomCreationQueue.Enqueue(creationInfo);
        checkQueue();
    }

    private void checkQueue() {
        if (!spawningRoom && roomCreationQueue.Count > 0) {
            SceneManager.LoadSceneAsync(roomCreationQueue.Peek().SceneName, LoadSceneMode.Additive);
            spawningRoom = true;
        } else if (!levelLoaded && roomCreationQueue.Count == 0) {
            levelLoaded = true;
            NotificationManager.PostNotification(Notifications.NOTIFICATION_LEVEL_LOADED, null);
            loadAllRooms();
        }
    }

    private void loadAllRooms() {
        List<Coordinate> standardRoomCoords = CurrentMap.GetStandardRooms();
        foreach (Coordinate coord in standardRoomCoords){
            loadStandardRoom(coord);
        }
    }

    private void loadStandardRoom(Coordinate coord) {
        int selectedRoom = UnityEngine.Random.Range(1, GameplayConstants.Instance.totalNumberOfStandardRooms + 1);
        string sceneName = GameplayConstants.Instance.standardRoomNamePrefix + selectedRoom;
        createRoom(new RoomCreationInfo(coord, sceneName, HiddenRoomParticlesPrefab));
    }

    /// <summary>
    ///  Determines if the room is occupied at the given coordinate. NOTE: Does not check queued rooms!
    /// </summary>
    public bool isRoomOccupied(Coordinate coord) {
        foreach (RoomController room in grid) {
            if (room.GridPosition.Equals(coord))
                return true;
        }
        return false;
    }

    public void SpawnPlayer(Player p) {
        GameObject player = (GameObject)Instantiate(PlayerPrefab, spawnPoints[p.PlayerNumber].GridPosition.GetWorldPosition(), Quaternion.identity);
        p.LinkPlayerToController(player.GetComponent<PlayerController>());
        p.CurrentRoom = spawnPoints[p.PlayerNumber];
    }

    public List<RoomController> GetAdjacentRooms(RoomController room) {
        List<RoomController> rooms = new List<RoomController>();
        foreach (RoomController r in grid) {
            if(r.GridPosition.Distance(room.GridPosition) == 1) {
                rooms.Add(r);
            }
        }
        return rooms;
    }

}