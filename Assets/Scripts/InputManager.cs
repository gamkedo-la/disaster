using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class InputManager : MonoBehaviour {

	public GameObject meteor;
    public GameObject tornado;
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    public enum inputMode { None, Tornado, Storms, Meteor, Volcano };
    public inputMode currentAction = inputMode.None;
    GameObject focusedGO;
    bool isHandInCloud = false;
    bool isStiring = false;
    int movementSmoothingFrames = 0;
    Vector3 prevPOS;

    // Use this for initialization
    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

    void SpawnAndParentObject(GameObject toSpawn, bool lockedToHand, bool hasRB) {
        focusedGO = (GameObject)GameObject.Instantiate(toSpawn);
        
        focusedGO.transform.position = transform.position;
        focusedGO.transform.rotation = transform.rotation;
        if (hasRB) {
            Rigidbody toSpawnRB = focusedGO.gameObject.GetComponent<Rigidbody>();
            toSpawnRB.isKinematic = true;
        }
        if (lockedToHand) {
            focusedGO.gameObject.transform.SetParent(gameObject.transform);
        }
        
    }

    public void MarkHandAsInCloud() {
        isHandInCloud = true;
    }

    public void MarkHandAsNotInCloud() {
        isHandInCloud = false;
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
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
            if (isHandInCloud)
            {
                currentAction = inputMode.Storms;
                
            }
            else if (transform.position.y > Camera.main.transform.position.y + 0.3f)
            {
                currentAction = inputMode.Meteor;
                SpawnAndParentObject(meteor, true, true);
            }
            else if (transform.position.y < Terrain.activeTerrain.transform.position.y)
            {
                currentAction = inputMode.Volcano;
            }
            else {
                currentAction = inputMode.Tornado;
                SpawnAndParentObject(tornado, false, false);
            }
            Debug.Log("You activated 'PressDown' on the trigger and detecting: " + currentAction);
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("You activated 'PressUp' on the trigger");
            switch (currentAction) {
                case inputMode.Meteor:
                    focusedGO.gameObject.transform.SetParent(null);
                    Rigidbody meteorRB = focusedGO.gameObject.GetComponent<Rigidbody>();
                    meteorRB.isKinematic = false;
                    tossObject(meteorRB);
                    break;
                case inputMode.Tornado:
                    TornadoManager tm = focusedGO.GetComponent<TornadoManager>();
                    tm.AddPower(2.0f);
                    Debug.Log("I'm calling start moving!");
                    tm.StartMoving();
                    break;
                /*case inputMode.Storms:
                    break;
                case inputMode.Volcano:
                    break; */
                default:
                    Debug.Log("Unhandled case: " + currentAction);
                    break;
            }

            currentAction = inputMode.None;
        }
        if (currentAction == inputMode.Tornado) {
            Vector3 movementDelta = transform.position - prevPOS;
            movementDelta *= 2000.0f;
            if (movementDelta.magnitude > 15.0f)
            {
                if (isStiring == false)
                {
                    movementSmoothingFrames++;
                    if (movementSmoothingFrames > 10)
                    {
                        isStiring = true;
                        Debug.Log("Started stiring");
                        movementSmoothingFrames = 10;
                        if (focusedGO)
                        {
                            focusedGO.GetComponentInChildren<Spinner>().IncreasePower();
                        }
                    }
                }
            }
            else
            {
                if (isStiring)
                {
                    movementSmoothingFrames--;
                    if (movementSmoothingFrames < 0)
                    {
                        isStiring = false;
                        Debug.Log("Stopped stiring");
                        movementSmoothingFrames = 0;
                    }

                }
            }
        }
        
        //Debug.Log("movementDelta = " + movementDelta * 2000);
        prevPOS = transform.position;
    }

    void OnTriggerStay(Collider other) {
        Debug.Log("You have collided with " + other.name + " and activated OnTriggerStay");
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("You have collided with " + other.name + " while holding down Touch");
            
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) {
           
            
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
