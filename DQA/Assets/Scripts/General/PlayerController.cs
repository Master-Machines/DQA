using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    public const float TimeToChangeRooms = 2f;
    public Player player;
    public Animation Anim;
    public GameObject CryptParticlesPrefab;
    public GameObject Model;

    private float movementTime;
    private float movementTimeMax;
    private Vector3 movementStartingPosition;
    private Vector3 movementEndingPosition;
    private GameObject createdCryptParticles;

	void Start () {
        Anim.Play("free");
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMovement();
	}

    void UpdateMovement() {
        if (movementTimeMax > 0) {
            movementTime += Time.deltaTime;

            if (movementTime > movementTimeMax)
                movementTime = movementTimeMax;
            transform.position = Vector3.Lerp(movementStartingPosition, movementEndingPosition, movementTime / movementTimeMax);
            if(movementTime == movementTimeMax) {
                movementTimeMax = 0;
                NotificationManager.PostNotification(Notifications.NOTIFICATION_PLAYER_MOVED_TO_ROOM, new Dictionary<string, object> { {Notifications.KEY_ROOM, player.CurrentRoom}, {Notifications.KEY_PLAYER, player} });
                Anim.Play("free");
            }
        }
    }

    public void MoveToRoom(RoomController room) {
        if(!player.InCrypt) {
            room.Reveal(player.CurrentRoom);
        }
        player.CurrentRoom = room;
        MoveToPoint(new Vector3(room.transform.position.x, transform.position.y, room.transform.position.z), TimeToChangeRooms);
    }

    public void SwitchCryptMode(bool inCrypt) {
        player.InCrypt = inCrypt;
        Model.SetActive(!inCrypt);
        if(inCrypt) {
            createdCryptParticles = (GameObject)Instantiate(CryptParticlesPrefab, transform.position, Quaternion.identity);
            createdCryptParticles.transform.Translate(Vector3.up * 3f);
            createdCryptParticles.transform.SetParent(transform);
        } else {
            Destroy(createdCryptParticles);
        }
    }

    private void MoveToPoint(Vector3 target, float time) {
        movementStartingPosition = transform.position;
        movementEndingPosition = target;
        movementTime = 0;
        movementTimeMax = time;
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
        Anim.Play("walk");
    }
}
