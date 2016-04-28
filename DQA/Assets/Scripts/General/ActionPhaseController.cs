using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActionPhaseController : MonoBehaviour {
    private static ActionPhaseController instance;
    public static ActionPhaseController Instance { get { return instance; } }
    public GameObject UIParent;
    public GameObject AllOptionsUI;
    public GameObject MovementUI;
    public GameObject SearchUI;
    public GameObject CatacombsUI;
    public GameObject RoomSelectionParticles;

    private ActionSubPhase subPhase { get; set; }
    public ActionSubPhase CurrentSubPhase { get { return subPhase; }  set {
            subPhase = value;
            SetupSubphase();    
    }}

    public RoomController[] MovementRooms;
    private int currentlyHighlightedRoomIndex { get; set; }
    public int CurrentlyHighlightedRoom {get { return currentlyHighlightedRoomIndex; } set {
            currentlyHighlightedRoomIndex = value;
            UpdateCurrentlyHighlightedRoom();
    } }

    private bool phaseEnabled = false;
	void Awake () {
        NotificationManager.Observe(this, "PhaseChanged", Notifications.NOTIFICATION_PHASE_INCREMENTED);
        NotificationManager.Observe(this, "PressedA", Notifications.INPUT_A_PRESSED);
        NotificationManager.Observe(this, "PressedB", Notifications.INPUT_B_PRESSED);
        NotificationManager.Observe(this, "PressedX", Notifications.INPUT_X_PRESSED);
        NotificationManager.Observe(this, "PressedY", Notifications.INPUT_Y_PRESSED);
        NotificationManager.Observe(this, "InputUp", Notifications.INPUT_UP);
        NotificationManager.Observe(this, "InputDown", Notifications.INPUT_DOWN);
        NotificationManager.Observe(this, "InputLeft", Notifications.INPUT_LEFT);
        NotificationManager.Observe(this, "InputRight", Notifications.INPUT_RIGHT);
        NotificationManager.Observe(this, "PlayerFinishedMoving", Notifications.NOTIFICATION_PLAYER_MOVED_TO_ROOM);
        instance = this;
	}

    void PlayerFinishedMoving() {
        Continue();
    }

    void PressedA() {
       if(phaseEnabled) {
           if(CurrentSubPhase == ActionSubPhase.Overall) {
                CurrentSubPhase = ActionSubPhase.Move;
           } else if(CurrentSubPhase == ActionSubPhase.Move) {
                Game.CurrentGame.CurrentPlayer.PlayerController.MoveToRoom(MovementRooms[CurrentlyHighlightedRoom]);
                RoomSelectionParticles.SetActive(false);
                MovementUI.SetActive(false);
                phaseEnabled = false;
           } else if(CurrentSubPhase == ActionSubPhase.Catacombs) {
                phaseEnabled = false;
                Game.CurrentGame.CurrentPlayer.PlayerController.SwitchCryptMode(true);
                StartCoroutine(Continue(2f));
                CatacombsUI.SetActive(false);
                NotificationManager.PostNotification(Notifications.NOTIFICATION_CRYPT_MODE_ENABLED, null);
           }
       }
    }

    

    void PressedB() {
        if(phaseEnabled) {
            if(CurrentSubPhase == ActionSubPhase.Move || CurrentSubPhase == ActionSubPhase.Catacombs || CurrentSubPhase == ActionSubPhase.Search) {
                CurrentSubPhase = ActionSubPhase.Overall;
            }
        }
    }

    void PressedX() {
       if(phaseEnabled) {
           if(CurrentSubPhase == ActionSubPhase.Overall) {
                CurrentSubPhase = ActionSubPhase.Search;
           }
       }
    }

    void PressedY() {
       if(phaseEnabled) {
           if(CurrentSubPhase == ActionSubPhase.Overall) {
                CurrentSubPhase = ActionSubPhase.Catacombs;
           }
       }
    }

    void InputUp() {
       if(phaseEnabled) {
           if(CurrentSubPhase == ActionSubPhase.Move) {
                if (MovementRooms[1] != null)
                    CurrentlyHighlightedRoom = 1;
           }
       }
    }

    void InputDown() {
       if(phaseEnabled) {
           if(CurrentSubPhase == ActionSubPhase.Move) {
                if (MovementRooms[3] != null)
                    CurrentlyHighlightedRoom = 3;
           }
       }
    }

    void InputLeft() {
       if(phaseEnabled) {
           if(CurrentSubPhase == ActionSubPhase.Move) {
                if (MovementRooms[0] != null)
                    CurrentlyHighlightedRoom = 0;
           }
       }
    }

    void InputRight() {
       if(phaseEnabled) {
           if(CurrentSubPhase == ActionSubPhase.Move) {
                if (MovementRooms[2] != null)
                    CurrentlyHighlightedRoom = 2;
           }
       }
    }

    void PhaseChanged(Dictionary<string, object> parameters) {
        Phase p = PhaseTracker.GetPhaseFromNotificationParams(parameters);
        if(p == Phase.Action && !phaseEnabled) {
            StartCoroutine(QueuePhase());
        }
    }

    IEnumerator QueuePhase() {
        yield return new WaitForSeconds(PhaseTracker.PhaseTransitionTime);
        EnablePhase();
    }

    void EnablePhase() {
        phaseEnabled = true;
        CurrentSubPhase = ActionSubPhase.Overall;
        SetupSubphase();
        if(UIParent != null)
            UIParent.SetActive(true);
    }

    void SetupSubphase() {
        AllOptionsUI.SetActive(CurrentSubPhase == ActionSubPhase.Overall);
        MovementUI.SetActive(CurrentSubPhase == ActionSubPhase.Move);
        SearchUI.SetActive(CurrentSubPhase == ActionSubPhase.Search);
        CatacombsUI.SetActive(CurrentSubPhase == ActionSubPhase.Catacombs);
        RoomSelectionParticles.SetActive(CurrentSubPhase == ActionSubPhase.Move);
        if(CurrentSubPhase == ActionSubPhase.Move) {
            if(Game.CurrentGame.CurrentPlayer.InCrypt) {
                MovementRooms = GridMaster.Instance.GetSortedAdjacentRooms(Game.CurrentGame.CurrentPlayer.CurrentRoom);
            } else {
                MovementRooms = GridMaster.Instance.GetEnterableRooms(Game.CurrentGame.CurrentPlayer.CurrentRoom);
            }
            
            if (MovementRooms[(int)Edge.Left] != null)
                CurrentlyHighlightedRoom = (int)Edge.Left;
            else if (MovementRooms[(int)Edge.Top] != null)
                CurrentlyHighlightedRoom = (int)Edge.Top;
            else if (MovementRooms[(int)Edge.Right] != null)
                CurrentlyHighlightedRoom = (int)Edge.Right;
            else if (MovementRooms[(int)Edge.Bottom] != null)
                CurrentlyHighlightedRoom = (int)Edge.Bottom;
            else
                Debug.LogError("No room to move to. This shouldn't happen.");
        }
    }
    
    void UpdateCurrentlyHighlightedRoom () {
        RoomController r = MovementRooms[CurrentlyHighlightedRoom];
        if(r != null) {
            Vector3 highlightedPosition = RoomSelectionParticles.transform.position;
            highlightedPosition = r.transform.position;
            RoomSelectionParticles.transform.position = highlightedPosition;
        }
    }

    IEnumerator Continue(float afterSeconds) {
        yield return new WaitForSeconds(afterSeconds);
        Continue();
    }
	
	void Continue() {
        phaseEnabled = false;
        if(UIParent != null)
            UIParent.SetActive(false);
        if (Game.CurrentGame.CurrentPlayer.InCrypt)
            NotificationManager.PostNotification(Notifications.NOTIFICATION_CRYPT_MODE_DISABLED, null);
        PhaseTracker.Instance.ProceedToNextPhase();
    }

    public enum ActionSubPhase {
        Overall,
        Move,
        Search,
        Catacombs
    }
}
