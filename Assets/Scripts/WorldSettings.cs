using UnityEngine;
using System.Collections;

public class WorldSettings : MonoBehaviour {
    public bool isVREnabled = true;
    public GameObject viveCamParent;
    public GameObject localPlayCam;
    // Use this for initialization

    void Awake() {
        if (SteamVR.instance == null)
        {
            isVREnabled = false;
        }
        else {
            isVREnabled = true;
        }
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
