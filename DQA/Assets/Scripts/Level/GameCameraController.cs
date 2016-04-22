using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCameraController : MonoBehaviour {

    public static GameCameraController Instance;
    // Straight top down
    private const float MaxXAngle = 90f;
    private const float MinXAngle = 45f;

    private float posMovementTime = 0f;
    private float posMovementMax = 0f;
    private Vector3 posEndPosition;
    private Vector3 posStartPosition;

    private float rotTime = 0f;
    private float rotMaxTime = 0f;
    private float rotXStart;
    private float rotXEnd;

    void Awake() {
        Instance = this;
        NotificationManager.Observe(this, "PhaseChanged", Notifications.NOTIFICATION_PHASE_INCREMENTED);
    }

    void PhaseChanged(Dictionary<string, object> paramaters) {
        Phase p = PhaseTracker.GetPhaseFromNotificationParams(paramaters);
        if(p == Phase.Intermediate) {
            MoveToTargetPositionOverTime(GridMaster.Instance.CurrentMap.GridCenter, PhaseTracker.PhaseTransitionTime);
            MoveToTargetRotationOverTime(MaxXAngle, PhaseTracker.PhaseTransitionTime);
        } else if(p == Phase.Status) {
            MoveToTargetPositionOverTime(GetPlayerCameraPosition(), PhaseTracker.PhaseTransitionTime);
            MoveToTargetRotationOverTime(MinXAngle, PhaseTracker.PhaseTransitionTime);
        }
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
        UpdatePosition();
        UpdateRotation();
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
        playerPosition += new Vector3(0f, 25f, -15f);
        return playerPosition;
    }
}
