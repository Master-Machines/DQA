using UnityEngine;
using System.Collections;

public class RoomController : MonoBehaviour {
    public GameObject DisableInGame;
    public GameObject TestingPrefab;

    public RoomConnection.RoomConnectionType PosXConnection;
    public RoomConnection.RoomConnectionType NegXConnection;
    public RoomConnection.RoomConnectionType PosZConnection;
    public RoomConnection.RoomConnectionType NegZConnection;

    public Direction Entrance;
    public RoomType RoomType;
    public Coordinate GridPosition;

    private GameObject hiddenFogParticles;

	void Start () {
        if (GridMaster.Instance == null) {
            ConfigureForTesting();
        } else {
            DisableInGame.SetActive(false);
            RoomCreationInfo info = GridMaster.Instance.AddToGrid(this);
            Setup(info);
        }
	}

    void Setup(RoomCreationInfo info) {
        GridPosition = info.Coords;
        if (RoomType == RoomType.Standard)
        {
            gameObject.SetActive(false);
            hiddenFogParticles = (GameObject)Instantiate(info.HiddenRoomParticlesPrefab, GridPosition.GetWorldPosition(), Quaternion.identity);
        }
        
        transform.position = GridPosition.GetWorldPosition();
    }

    void ConfigureForTesting() {
        GameObject tester = (GameObject)Instantiate(TestingPrefab, transform.position + Vector3.up, Quaternion.identity);
    }

    public void Reveal() {
        hiddenFogParticles.GetComponent<ParticleSystem>().enableEmission = false;
    }
}

public enum RoomType {
    Standard,
    Treasure,
    Spawn
}
