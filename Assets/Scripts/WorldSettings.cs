using UnityEngine;
using System.Collections;

public class WorldSettings : MonoBehaviour {
    public bool isVREnabled = true;
    public GameObject viveCamParent;
    public GameObject localPlayCam;
    public ObjectSpawn[] objectsCreated;
    public float tooCloseToSpawn = 0.03f;
    // Use this for initialization
    public static WorldSettings instance;

    void Awake() {
        instance = this;
        if (SteamVR.instance == null)
        {
            isVREnabled = false;
        }
        else {
            isVREnabled = true;
        }
        objectsCreated = FindObjectsOfType(typeof(ObjectSpawn)) as ObjectSpawn[];
        // Debug.Log("There are this many objects created things: " + objectsCreated.Length);
    }

	void Start () {
        if (isVREnabled)
        {
            viveCamParent.SetActive(true);
            localPlayCam.SetActive(false);
        }
        else {
            localPlayCam.SetActive(true);
            viveCamParent.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
	}

    public bool IsSpaceClearNear(Vector3 checkAt) {
        for (int i = 0; i < objectsCreated.Length; i++) {
            if (objectsCreated[i].AmITooClose(checkAt, tooCloseToSpawn)) {
                return false;
            }
        } // for loop
        return true;
    } // end of IsSpaceClearNear
} // end of class
