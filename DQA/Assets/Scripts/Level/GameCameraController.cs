using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets;

public class GameCameraController : MonoBehaviour {

    public static GameCameraController Instance;
    

    // Straight top down
    private const float MaxXAngle = 65f;
    private const float MinXAngle = 55f;
    private const float RoomHighlightXAngle = 60f;
    private Vector3 PlayerHighlightOffset = new Vector3(0, 15f, -7f);
    private Vector3 RoomHighlightOffset = new Vector3(0, 35f, -15f);
    private Vector3 GridCenterOffset = new Vector3(0f, -10f, -40f);

    private const float RealtimeMovementSpeed = 25f;
    private const float RealtimeRotationSpeed = 6f;

    private float posMovementTime = 0f;
    private float posMovementMax = 0f;
    private Vector3 posEndPosition;
    private Vector3 posStartPosition;

    private float rotTime = 0f;
    private float rotMaxTime = 0f;
    private float rotXStart;
    private float rotXEnd;

    

    private bool updatingForActionPhase;

    void Awake() {
        Instance = this;
        NotificationManager.Observe(this, "PhaseChanged", Notifications.NOTIFICATION_PHASE_INCREMENTED);
        
    }

    void PhaseChanged(Dictionary<string, object> paramaters) {
        Phase p = PhaseTracker.GetPhaseFromNotificationParams(paramaters);
        updatingForActionPhase = false;
        if(p == Phase.Intermediate) {
            MoveToTargetPositionOverTime(GridMaster.Instance.CurrentMap.GridCenter + GridCenterOffset, PhaseTracker.PhaseTransitionTime);
            MoveToTargetRotationOverTime(MaxXAngle, PhaseTracker.PhaseTransitionTime);
        } else if(p == Phase.Status) {
            MoveToTargetPositionOverTime(GetPlayerCameraPosition(), PhaseTracker.PhaseTransitionTime);
            MoveToTargetRotationOverTime(MinXAngle, PhaseTracker.PhaseTransitionTime);
        } else if(p == Phase.Action) {
            StartCoroutine(QueueAction());
        }
    }

    IEnumerator QueueAction() {
        yield return new WaitForSeconds(PhaseTracker.PhaseTransitionTime);
        updatingForActionPhase = true;
    }

    void MoveToTargetPositionOverTime(Vector3 pos, float time) {
        posMovementTime = 0f;
        posMovementMax = time;
        posStartPosition = transform.position;
        posEndPosition = pos;
    }

    void MoveToTargetRotationOverTime(float xRotation, float time) {
        rotTime = 0f;
        rotMaxTime = time;
        rotXStart = transform.eulerAngles.x;
        rotXEnd = xRotation;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(updatingForActionPhase) {
            RealtimeUpdate();
        } else {
            UpdatePosition();
            UpdateRotation();
        }
	}

    
    void RealtimeUpdate() {
        ActionPhaseController.ActionSubPhase subPhase = ActionPhaseController.Instance.CurrentSubPhase;
        if(subPhase == ActionPhaseController.ActionSubPhase.Overall) {
            posEndPosition = GetPlayerCameraPosition();
            rotXEnd = MinXAngle;
        } else if(subPhase == ActionPhaseController.ActionSubPhase.Move) {
            posEndPosition = ActionPhaseController.Instance.MovementRooms[ActionPhaseController.Instance.CurrentlyHighlightedRoom].transform.position;
            posEndPosition += RoomHighlightOffset;
            rotXEnd = RoomHighlightXAngle;
        }

        Vector3 pos = transform.position;
        if(pos.x < posEndPosition.x) {
            pos.x += RealtimeMovementSpeed * Time.deltaTime;
            if (pos.x > posEndPosition.x)
                pos.x = posEndPosition.x;
        } else if (pos.x > posEndPosition.x) {
            pos.x -= RealtimeMovementSpeed * Time.deltaTime;
            if (pos.x < posEndPosition.x)
                pos.x = posEndPosition.x;
        }

        if(pos.y < posEndPosition.y) {
            pos.y += RealtimeMovementSpeed * Time.deltaTime;
            if (pos.y > posEndPosition.y)
                pos.y = posEndPosition.y;
        } else if (pos.y > posEndPosition.y) {
            pos.y -= RealtimeMovementSpeed * Time.deltaTime;
            if (pos.y < posEndPosition.y)
                pos.y = posEndPosition.y;
        }

        if(pos.z < posEndPosition.z) {
            pos.z += RealtimeMovementSpeed * Time.deltaTime;
            if (pos.z > posEndPosition.z)
                pos.z = posEndPosition.z;
        } else if (pos.z > posEndPosition.z) {
            pos.z -= RealtimeMovementSpeed * Time.deltaTime;
            if (pos.z < posEndPosition.z)
                pos.z = posEndPosition.z;
        }

        transform.position = pos;

        Vector3 rot = transform.eulerAngles;
        if(rot.x < rotXEnd) {
            rot.x += RealtimeRotationSpeed * Time.deltaTime;
            if (rot.x > rotXEnd)
                rot.x = rotXEnd;
        } else if(rot.x > rotXEnd) {
            rot.x -= RealtimeRotationSpeed * Time.deltaTime;
            if (rot.x < rotXEnd)
                rot.x = rotXEnd;
        }

        transform.eulerAngles = rot;
    }

    void UpdatePosition() {
        if(posMovementMax > 0f) {
            posMovementTime += Time.deltaTime;
            if (posMovementTime > posMovementMax)
                posMovementTime = posMovementMax;
            float timeRatio = posMovementTime / posMovementMax;
            transform.position = Vector3.Lerp(posStartPosition, posEndPosition, timeRatio);
            if (posMovementTime == posMovementMax)
                posMovementMax = 0;
        }
    }

    void UpdateRotation() {
        if(rotMaxTime > 0f) {
            rotTime += Time.deltaTime;
            if (rotTime > rotMaxTime)
                rotTime = rotMaxTime;
            float timeRatio = rotTime / rotMaxTime;
            transform.position = Vector3.Lerp(posStartPosition, posEndPosition, timeRatio);
            Vector3 currentAngle = transform.eulerAngles;
            currentAngle = new Vector3(Mathf.Lerp(rotXStart, rotXEnd, timeRatio), currentAngle.y, currentAngle. z);
            transform.eulerAngles = currentAngle;
            if (rotTime == rotMaxTime)
                rotMaxTime = 0;
        }
    }

    Vector3 GetPlayerCameraPosition() {
        Vector3 playerPosition = Game.CurrentGame.CurrentPlayer.PlayerController.transform.position;
        playerPosition += PlayerHighlightOffset;
        return playerPosition;
    }

}
