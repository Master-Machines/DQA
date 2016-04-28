using UnityEngine;
using System.Collections;

public class CameraEffectController : MonoBehaviour {
    public UnityStandardAssets.ImageEffects.Bloom bloomEffect;
    private const float BloomIntensityCrypt = 2.5f;
    private float effectTime;
    private float effectTimeMax;
    private float effectValueStart;
    private float effectValueEnd;

	// Use this for initialization
	void Start () {
	    NotificationManager.Observe(this, "CryptEnabled", Notifications.NOTIFICATION_CRYPT_MODE_ENABLED);
        NotificationManager.Observe(this, "CryptDisabled", Notifications.NOTIFICATION_CRYPT_MODE_DISABLED);
        bloomEffect.bloomIntensity = 0f;
	}
	
	// Update is called once per frame
	void Update () {
	    if(effectTimeMax > 0f) {
            effectTime += Time.deltaTime;
            if (effectTime > effectTimeMax)
                effectTime = 1f;
            bloomEffect.bloomIntensity = Mathf.Lerp(effectValueStart, effectValueEnd, effectTime / effectTimeMax);
            if (effectTime == effectTimeMax)
                effectTimeMax = 0f; 
       }
	}

    void CryptEnabled() {
        effectTime = 0f;
        effectTimeMax = .8f;
        effectValueStart = bloomEffect.bloomIntensity;
        effectValueEnd = BloomIntensityCrypt;
    }

    void CryptDisabled() {
        effectTime = 0f;
        effectTimeMax = .6f;
        effectValueStart = bloomEffect.bloomIntensity;
        effectValueEnd = 0f;
    }
}
