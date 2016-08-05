using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour {

    public Transform sphere;
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    // Use this for initialization
    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        //if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
        //    Debug.Log("You're holding 'Touch' on the trigger");
        //}

        //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        //{
        //    Debug.Log("You activated 'TouchDown' on the trigger");
        //}

        //if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        //{
        //    Debug.Log("You activated 'TouchUp' on the trigger");
        //}

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            sphere.transform.position = Vector3.zero;
            sphere.GetComponent<Rigidbody>().velocity = Vector3.zero;
            sphere.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You activated 'PressDown' on the trigger");
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You activated 'PressUp' on the trigger");
        }
    }

    void OnTriggerStay(Collider other) {
        Debug.Log("You have collided with " + other.name + " and activated OnTriggerStay");
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("You have collided with " + other.name + " while holding down Touch");
            other.attachedRigidbody.isKinematic = true;
            other.gameObject.transform.SetParent(gameObject.transform);
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) {
           
            other.gameObject.transform.SetParent(null);
            other.attachedRigidbody.isKinematic = false;

            tossObject(other.attachedRigidbody);
        }
    }

    void tossObject(Rigidbody rigidbody)
    {
        Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
        if (origin != null)
        {
            rigidbody.velocity = origin.TransformVector(device.velocity);
            rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
        }
        else {
            rigidbody.velocity = device.velocity;
            rigidbody.angularVelocity = device.angularVelocity;
        }
        
    }
}
