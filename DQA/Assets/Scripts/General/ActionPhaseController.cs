using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActionPhaseController : MonoBehaviour {
    public GameObject UIParent;
    public GameObject AllOptionsUI;
    public GameObject MovementUI;

    private bool phaseEnabled = false;
	void Awake () {
        NotificationManager.Observe(this, "PhaseChanged", Notifications.NOTIFICATION_PHASE_INCREMENTED);
        NotificationManager.Observe(this, "PressedA", Notifications.INPUT_A_PRESSED);
        NotificationManager.Observe(this, "PressedB", Notifications.INPUT_B_PRESSED);
        NotificationManager.Observe(this, "PressedX", Notifications.INPUT_X_PRESSED);
        NotificationManager.Observe(this, "PressedY", Notifications.INPUT_Y_PRESSED);
	}

    void PressedA() {
       
    }

    void PressedB() {
       
    }

    void PressedX() {
       
    }

    void PressedY() {
       
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
        if(UIParent != null)
            UIParent.SetActive(true);
    }
	
	void Continue() {
        phaseEnabled = false;
        if(UIParent != null)
            UIParent.SetActive(false);
        PhaseTracker.Instance.ProceedToNextPhase();
    }

    enum ActionSubPhase {
        Overall,
        Move,
        Search,
        Catacombs
    }
}
