using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhaseTracker : MonoBehaviour {

    public static PhaseTracker Instance;
    public const float PhaseTransitionTime = 1f;
    public Game CurrentGame;
    public Phase CurrentPhase { get; set; }

    void Awake() {
        Instance = this;
        CurrentGame = new Game(4);
        CurrentPhase = Phase.Action;
        NotificationManager.Observe(this, "LevelLoaded", Notifications.NOTIFICATION_LEVEL_LOADED);        
    }

    void LevelLoaded() {
        ProceedToNextPhase();
    } 
	
	void Update () {
	
	}

    void IncrementPlayer() {
        CurrentGame.IncrementPlayer();
        if(!CurrentGame.CurrentPlayer.Initiated) {
            GridMaster.Instance.SpawnPlayer(CurrentGame.CurrentPlayer);
        }
    }

    public void ProceedToNextPhase() {
        if(CurrentPhase == null || CurrentPhase == Phase.Action) {
            CurrentPhase = Phase.Intermediate;
        } else if(CurrentPhase == Phase.Intermediate) {
            CurrentPhase = Phase.Status;
            IncrementPlayer();
        } else {
            CurrentPhase = Phase.Action;
        }
        NotificationManager.PostNotification(Notifications.NOTIFICATION_PHASE_INCREMENTED, new Dictionary<string, object> { { Notifications.KEY_PHASE_INCREMENTED_PHASE, CurrentPhase.ToString()}});
    }

    public static Phase GetPhaseFromNotificationParams(Dictionary<string, object> parameters)
    {
        string phase = (string)parameters[Notifications.KEY_PHASE_INCREMENTED_PHASE];
        
        if (phase.Equals("Status"))
            return Phase.Status;
        else if (phase.Equals("Action"))
            return Phase.Action;
        return Phase.Intermediate;

    }
}

public enum Phase {
      Status,
      Action,
      Intermediate  
}
