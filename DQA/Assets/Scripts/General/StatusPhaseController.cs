using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StatusPhaseController : MonoBehaviour {
    public GameObject UIParent;

    private bool phaseEnabled = false;
	void Awake () {
        NotificationManager.Observe(this, "PhaseChanged", Notifications.NOTIFICATION_PHASE_INCREMENTED);
	}

    void PhaseChanged(Dictionary<string, object> parameters) {
        Phase p = PhaseTracker.GetPhaseFromNotificationParams(parameters);
        if(p == Phase.Status && !phaseEnabled) {
            StartCoroutine(QueuePhase());
        }
    }

    IEnumerator QueuePhase() {
        yield return new WaitForSeconds(PhaseTracker.PhaseTransitionTime);
        EnablePhase();
    }

    void EnablePhase() {
        phaseEnabled = true;
        if(Game.CurrentGame.CurrentPlayer.InCrypt) {
            NotificationManager.PostNotification(Notifications.NOTIFICATION_CRYPT_MODE_ENABLED, null);
        }
        if(UIParent != null)
            UIParent.SetActive(true);
        Continue();
    }
	
	void Continue() {
        phaseEnabled = false;
        if(UIParent != null)
            UIParent.SetActive(false);
        PhaseTracker.Instance.ProceedToNextPhase();
    }
}
