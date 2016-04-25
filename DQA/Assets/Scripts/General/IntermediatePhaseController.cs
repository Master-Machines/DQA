using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IntermediatePhaseController : MonoBehaviour {
    public GameObject UIParent;
    public Text HeaderText;

    private bool phaseEnabled = false;
	void Awake () {
        NotificationManager.Observe(this, "PressedA", Notifications.INPUT_A_PRESSED);
        NotificationManager.Observe(this, "PhaseChanged", Notifications.NOTIFICATION_PHASE_INCREMENTED);
	}

    void PressedA() {
        if(phaseEnabled) {
            Continue();
        }
    }

    void PhaseChanged(Dictionary<string, object> parameters) {
        Phase p = PhaseTracker.GetPhaseFromNotificationParams(parameters);
        if(p == Phase.Intermediate && !phaseEnabled) {
            StartCoroutine(QueuePhase());
        }
    }

    IEnumerator QueuePhase() {
        yield return new WaitForSeconds(PhaseTracker.PhaseTransitionTime);
        EnablePhase();
    }

    void EnablePhase() {
        phaseEnabled = true;
        HeaderText.text = "Player " + (Game.CurrentGame.CurrentPlayerIndex + 1).ToString() + "'s Turn!";
        UIParent.SetActive(true);
    }
	
	void Continue() {
        phaseEnabled = false;
        UIParent.SetActive(false);
        PhaseTracker.Instance.ProceedToNextPhase();
    }
}
