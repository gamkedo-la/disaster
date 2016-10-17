using UnityEngine;
using System.Collections;

public class HutManager : MonoBehaviour {

    bool isBurntYet = false;
    public Material burnedMat;
    public GameObject hutModel;

    public void FireBurnedOut()
    {
        if (isBurntYet == false)
        {
            isBurntYet = true;
            Debug.Log("Hut is on fire!");

            hutModel.GetComponent<MeshRenderer>().material = burnedMat;
        }
    }
}