﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class InputManager : MonoBehaviour {

	public GameObject meteor;
    public GameObject tornado;
    public GameObject volcanoCreator;
    public GameObject deadBody;
    public Screenshot_Handler screenShotHandler;
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    public enum inputMode { None, Tornado, Storms, Meteor, Volcano };
    public inputMode currentAction = inputMode.None;
    GameObject focusedGO;
    GameObject cloud;
    public bool isHandInCloud = false;
    bool isStiring = false;
    int movementSmoothingFrames = 0;
    Vector3 prevPOS;
    bool buttonEnabled;
    public GameObject buttonHolder;
	public GameObject buttonHolderMainMenu;
    bool meteorPresent = false;

    public HandAnimController handAnim;
    public HandAnimController handAnimOther;

    public Transform parentOffset;
    private bool inMenuScene = false;
    // Use this for initialization
    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        screenShotHandler = GetComponent<Screenshot_Handler>();
        buttonEnabled = false;
        if (buttonHolder != null)
        {
            buttonHolder.SetActive(false);
        }
        else {
            Debug.Log("for " + name + " there was no button Holder");
        }
        if (SceneManager.GetActiveScene().name.CompareTo("MainMenu") == 0)
        {
            inMenuScene = true;
            if (handAnim != null && inMenuScene)
                handAnim.SetPoint(true);
        }
    }

    void SpawnAndParentObject(GameObject toSpawn, bool lockedToHand, bool hasRB) {
        focusedGO = (GameObject)GameObject.Instantiate(toSpawn);

        if (parentOffset != null)
        {
            focusedGO.transform.position = parentOffset.position;
            focusedGO.transform.rotation = parentOffset.rotation;
        }
        else
        {
            focusedGO.transform.position = transform.position;
            focusedGO.transform.rotation = transform.rotation;
        }
        if (hasRB) {
            Rigidbody toSpawnRB = focusedGO.gameObject.GetComponent<Rigidbody>();
            toSpawnRB.isKinematic = true;
        }
        if (lockedToHand) {
            if (parentOffset != null)
            {
                focusedGO.gameObject.transform.SetParent(parentOffset);
            }
            else
            {
                focusedGO.gameObject.transform.SetParent(gameObject.transform);
            }
               
        }
        
    }

    public void MarkHandAsInCloud() {
        Debug.Log("marking as true");
        isHandInCloud = true;
    }

    public void MarkHandAsNotInCloud() {
        isHandInCloud = false;
    }

    void Update() {

        device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)){
            Debug.Log("You hit the menu button!");
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && gameObject.tag == "LeftController")
        {
			if (inMenuScene == false) {
				if (buttonEnabled == false) {
					buttonEnabled = true;
					buttonHolder.SetActive (true);
					if (handAnim != null && !inMenuScene)
						handAnim.SetPoint (true);
					if (handAnim != null && !inMenuScene)
						handAnimOther.SetPoint (true);
				} else if (buttonEnabled == true) {
					buttonEnabled = false;
					buttonHolder.SetActive (false && !inMenuScene);
					if (handAnim != null)
						handAnim.SetPoint (false && !inMenuScene);
					if (handAnim != null)
						handAnimOther.SetPoint (false);
				}	
			} else {
				if (buttonEnabled == false)
				{
					buttonEnabled = true;
					buttonHolderMainMenu.SetActive(true);
					if (handAnim != null && !inMenuScene)
						handAnim.SetPoint(true);
					if (handAnim != null && !inMenuScene)
						handAnimOther.SetPoint(true);
				}
				else if (buttonEnabled == true)
				{
					buttonEnabled = false;
					buttonHolderMainMenu.SetActive(false && !inMenuScene);
					if (handAnim != null)
						handAnim.SetPoint(false && !inMenuScene);
					if (handAnim != null)
						handAnimOther.SetPoint(false);
				}
			}
            
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        
        device = SteamVR_Controller.Input((int)trackedObj.index);
        if (!inMenuScene)
        {
            /* if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && gameObject.tag == "LeftController") {
                 if (buttonEnabled == false)
                 {
                     buttonEnabled = true;
                     buttonHolder.SetActive(true);
                 }
                 else if (buttonEnabled == true) {
                     buttonEnabled = false;
                     buttonHolder.SetActive(false);
                 }
             }*/

            /*if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                GameObject db = (GameObject)GameObject.Instantiate(deadBody);
                db.transform.position = transform.position;
                db.GetComponent<PeopleMover>().knockOver();
            }*/

            //if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            //{
            //    Debug.Log("You activated 'TouchUp' on the trigger");
            //}
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (handAnim != null)
                {
                    handAnim.SetTrigger(true);
                }
                if (isHandInCloud)
                {
                    currentAction = inputMode.Storms;

                }
                else if (transform.position.y > Camera.main.transform.position.y + 0.3f)
                {
                    currentAction = inputMode.Meteor;
                    if (meteorPresent == false)
                    {
                        SpawnAndParentObject(meteor, true, true);
                        meteorPresent = true;
                    }

                }
                else if (transform.position.y < Terrain.activeTerrain.SampleHeight(transform.position) + Terrain.activeTerrain.transform.position.y)
                {
                    currentAction = inputMode.Volcano;

                }
                else
                {
                    //currentAction = inputMode.Tornado;

                }
                //Debug.Log("You activated 'PressDown' on the trigger and detecting: " + currentAction);
            }

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (handAnim != null)
                {
                    handAnim.SetTrigger(false);
                }
                Debug.Log("You activated 'PressUp' on the trigger");
                switch (currentAction)
                {
                    case inputMode.Meteor:
                        focusedGO.gameObject.transform.SetParent(null);
                        Rigidbody meteorRB = focusedGO.gameObject.GetComponent<Rigidbody>();
                        meteorRB.isKinematic = false;
                        focusedGO.GetComponent<AudioSource>().Play();
                        tossObject(meteorRB);
                        meteorPresent = false;
                        break;
                    case inputMode.Tornado:
                        /*TornadoManager tm = focusedGO.GetComponent<TornadoManager>();
                        tm.AddPower(2.0f);
                        //Debug.Log("I'm calling start moving!");
                        tm.StartMoving();*/
                        break;
                    case inputMode.Storms:
                        RainCloud storm = cloud.GetComponent<RainCloud>();
                        if (storm != null)
                        {
                            if (storm.IsRaining())
                            {
                                storm.StopStorm();
                            }
                            else
                            {
                                storm.StartStorm();
                            }
                        }
                        else
                        {
                            cloud.GetComponent<ThunderCloud>().StartStorm();
                        }
                        cloud.transform.SetParent(null);
                        break;
                    case inputMode.Volcano:
                        SpawnAndParentObject(volcanoCreator, true, false);
                        break;
                    default:
                        Debug.Log("Unhandled case: " + currentAction);
                        break;
                }

                currentAction = inputMode.None;
            }
            if (currentAction == inputMode.Tornado)
            {
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
                            //Debug.Log("Started stiring");
                            movementSmoothingFrames = 10;
                            if (focusedGO)
                            {
                                if (focusedGO.GetComponentInChildren<Spinner>() != null)
                                {
                                    focusedGO.GetComponentInChildren<Spinner>().IncreasePower();
                                }
                            }
                            else
                            {
                                SpawnAndParentObject(tornado, false, false);
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
                            //Debug.Log("Stopped stiring");
                            movementSmoothingFrames = 0;
                        }

                    }
                }
            }
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            screenShotHandler.PressDown();
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            screenShotHandler.PressUp();
        }
            //Debug.Log("movementDelta = " + movementDelta * 2000);
            prevPOS = transform.position;
    }

    void OnTriggerStay(Collider other) {
        
        if (other.gameObject.GetComponent<RainCloud>() != null || other.gameObject.GetComponent<ThunderCloud>() != null)
        {
            MarkHandAsInCloud();
            cloud = other.gameObject;
            if (currentAction == inputMode.Storms)
            {
                other.gameObject.transform.SetParent(gameObject.transform);
            }

        }

        /*if (other.gameObject.GetComponent<PeopleMover>() != null) {
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                other.gameObject.GetComponent<PeopleMover>().knockOver();
            }
        }*/
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<RainCloud>() != null || other.gameObject.GetComponent<ThunderCloud>() != null) {
            MarkHandAsNotInCloud();
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
        Destroy(rigidbody.gameObject, 4.0f);
    }
}
