using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class InputManager : MonoBehaviour {

	public GameObject meteor;
    public GameObject tornado;
    public GameObject volcanoCreator;
    public GameObject deadBody;
    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    public enum inputMode { None, Tornado, Storms, Meteor, Volcano };
    public inputMode currentAction = inputMode.None;
    GameObject focusedGO;
    GameObject cloud;
    public bool isScreenshotPressed = false;
    public bool isHandInCloud = false;
    bool isStiring = false;
    int movementSmoothingFrames = 0;
    Vector3 prevPOS;
    static int nIndex = 2;

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
        Debug.Log("marking as true");
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

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            GameObject db = (GameObject)GameObject.Instantiate(deadBody);
            db.transform.position = transform.position;
            db.GetComponent<PeopleMover>().knockOver();
        }

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
            else if (transform.position.y < Terrain.activeTerrain.SampleHeight(transform.position) + Terrain.activeTerrain.transform.position.y)
            {
                currentAction = inputMode.Volcano;
                
            }
            else {
                //currentAction = inputMode.Tornado;
                
            }
            //Debug.Log("You activated 'PressDown' on the trigger and detecting: " + currentAction);
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
                    //Debug.Log("I'm calling start moving!");
                    tm.StartMoving();
                    break;
                case inputMode.Storms:
                    RainCloud storm = cloud.GetComponent<RainCloud>();
                    if (storm != null)
                    {
                        storm.StartStorm();
                    }
                    else {
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
                        //Debug.Log("Started stiring");
                        movementSmoothingFrames = 10;
                        if (focusedGO)
                        {
                            if (focusedGO.GetComponentInChildren<Spinner>() != null) {
                                focusedGO.GetComponentInChildren<Spinner>().IncreasePower();
                            }
                        }
                        else {
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
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if(!isScreenshotPressed)
            {
                //Display capture ready message in the center of the screen
                print("Create GUI Canvas... Ready to capture screen. Release trackpad when ready.");
            }
            isScreenshotPressed = true;
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if(isScreenshotPressed)
            {
                //isScreenshotPressed was previously true, we are now taking the screen shot. 

                //Take screenshot
                TakeScreenshot();

                //Display success or fail message
                isScreenshotPressed = false;
            }

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

    void TakeScreenshot()
    {
        string fileName = GetScreenshotFilename();
        string fileExtension = ".png";
        string combinedFileName = fileName + fileExtension;
        string customPath = GetScreenshotDirectory(); //Load custom path here
        string directoryPath = "";
        if (customPath == "")
        {
            directoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        }
        else
            directoryPath = customPath;
        directoryPath = System.IO.Path.Combine(directoryPath, "DivineDisaster_Screenshots");
        string pngPath = System.IO.Path.Combine(directoryPath, combinedFileName);

        //Check if directory exists
        if (!System.IO.Directory.Exists(directoryPath))
        {
            //Failed to find directory, lets create it
            System.IO.Directory.CreateDirectory(directoryPath);
        }
        while (System.IO.File.Exists(pngPath))
        {
            //File already exists, create new index at end
            combinedFileName = fileName + "_" + nIndex.ToString() + fileExtension;
            pngPath = System.IO.Path.Combine(directoryPath, combinedFileName);
            nIndex++;
        }

        Application.CaptureScreenshot(pngPath);
        print("Create GUI Canvas... Screenshot saved to : " + pngPath);
    }

    string GetScreenshotFilename()
    {
        //Create interface for user to set file name preference
        return "Test_Capture";
    }

    string GetScreenshotDirectory()
    {
        //Create interface for user to set directory path 
        return "";
    }
}
