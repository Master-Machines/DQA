using UnityEngine;
using System.Collections;

public class RoomController : MonoBehaviour {
    public GameObject DisableInGame;
    public GameObject TestingPrefab;

    public RoomConnection.RoomConnectionType PosXConnection;
    public RoomConnection.RoomConnectionType NegXConnection;
    public RoomConnection.RoomConnectionType PosZConnection;
    public RoomConnection.RoomConnectionType NegZConnection;

    public RoomType RoomType;
    public Coordinate GridPosition;
    public bool Revealed { get; internal set; }

    private GameObject[] createdHallwayParticles;
    private GameObject hiddenFogParticles;
    private GameObject hallwayParticles;

	void Start () {
        createdHallwayParticles = new GameObject[4];
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
        } else {
            Revealed = true;
        }
        hallwayParticles = info.HallwayParticlesPrefab;
        transform.position = GridPosition.GetWorldPosition();
    }

    void ConfigureForTesting() {
        GameObject tester = (GameObject)Instantiate(TestingPrefab, transform.position + Vector3.up, Quaternion.identity);
    }

    public void Reveal(RoomController fromRoom) {
        if(!Revealed) {
            gameObject.SetActive(true);
            Vector3 lookTarget = fromRoom.transform.position;
            lookTarget.y = transform.position.y;
            transform.LookAt(lookTarget);

            Edge edge = GetDirectionFromOtherRoom(fromRoom);
            if (edge == Edge.Left)
                Rotate(-1);
            else if (edge == Edge.Bottom)
                Rotate(-2);
            else if (edge == Edge.Right)
                Rotate(1);

            Revealed = true;
            hiddenFogParticles.GetComponentInChildren<ParticleSystem>().enableEmission = false;
            foreach(MeshRenderer r in hiddenFogParticles.GetComponentsInChildren<MeshRenderer>()) {
                r.enabled = false;
            }
            UpdateHallwayParticles();
            foreach(RoomController r in GridMaster.Instance.GetAdjacentRooms(this)) {
                if (r != null)
                    r.UpdateHallwayParticles();
            }
        }
    }

    public void UpdateHallwayParticles() {
        if(Revealed) {
             RoomController[] adjacentRooms = GridMaster.Instance.GetSortedAdjacentRooms(this);
            if(RoomType == RoomType.Standard) {
                for(int i = 0; i < 4; i++) {
                    if(adjacentRooms[i] != null && adjacentRooms[i].RoomType == RoomType.Standard) {
                        RoomConnection.RoomConnectionType connectionType = GetConnectionTypeFromEdge((Edge)i);
                        if(!adjacentRooms[i].Revealed && createdHallwayParticles[i] == null && connectionType  == RoomConnection.RoomConnectionType.Open) {
                            createdHallwayParticles[i] = (GameObject)Instantiate(hallwayParticles, GetEdgePosition((Edge)i), Quaternion.identity);
                            Vector3 lookAtTarget = transform.position;
                            lookAtTarget.y = createdHallwayParticles[i].transform.position.y;
                            createdHallwayParticles[i].transform.LookAt(lookAtTarget);
                        } else if(adjacentRooms[i].Revealed && createdHallwayParticles[i] != null) {
                            Destroy(createdHallwayParticles[i]);
                            createdHallwayParticles[i] = null;
                        }
                    }
                }
            }
        }
    }

    public void Rotate(int remaining) {
        bool clockwise = remaining > 0;
        if(clockwise) {
            RoomConnection.RoomConnectionType temp = PosZConnection;
            PosZConnection = NegXConnection;
            NegXConnection = NegZConnection;
            NegZConnection = PosXConnection;
            PosXConnection = temp;
            remaining--;
        } else {
            RoomConnection.RoomConnectionType temp = PosZConnection;
            PosZConnection = PosXConnection;
            PosXConnection = NegZConnection;
            
            NegZConnection = NegXConnection;
            NegXConnection = temp;
            remaining++;
        }

        if (remaining != 0)
            Rotate(remaining);
    }

    Vector3 GetEdgePosition(Edge edge) {
        Vector3 pos = transform.position;
        pos.y += GameplayConstants.Instance.RoomSize / 4f;
        if (edge == Edge.Left)
            pos.x -= GameplayConstants.Instance.RoomSize / 2f;
        else if (edge == Edge.Top)
            pos.z += GameplayConstants.Instance.RoomSize / 2f;
        else if (edge == Edge.Right)
            pos.x += GameplayConstants.Instance.RoomSize / 2f;
        else
            pos.z -= GameplayConstants.Instance.RoomSize / 2f;
        return pos;
    }

    Edge GetDirectionFromOtherRoom(RoomController other) {
        Coordinate dist = GridPosition.Delta(other.GridPosition);
        if (dist.X < 0)
            return Edge.Left;
        if (dist.X > 0)
            return Edge.Right;
        if (dist.Z > 0)
            return Edge.Top;
        return Edge.Bottom;
    }

    RoomConnection.RoomConnectionType GetConnectionTypeFromEdge(Edge edge) {
        if (edge == Edge.Left)
            return NegXConnection;
        else if (edge == Edge.Top)
            return PosZConnection;
        else if (edge == Edge.Right)
            return PosXConnection;
        return NegZConnection;
    }
}

public enum Edge {
    Left = 0,
    Top = 1,
    Right = 2,
    Bottom = 3
}

public enum RoomType {
    Standard,
    Treasure,
    Spawn
}
