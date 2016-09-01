using UnityEngine;
using System.Collections;

public class ThunderStorms : MonoBehaviour {

    public bool isRainCloud = false;
    public bool isLightningCloud = false;

    void OnTriggerStay(Collider other) {
        Debug.Log("OnTrigerStay entered by " + other.name);
        if (other.gameObject.GetComponent<InputManager>() != null) {
            InputManager tempInput = other.gameObject.GetComponent<InputManager>();
            tempInput.MarkHandAsInCloud();
            if (tempInput.currentAction == InputManager.inputMode.Storms) {
                gameObject.transform.SetParent(other.gameObject.transform);
            }
            
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
