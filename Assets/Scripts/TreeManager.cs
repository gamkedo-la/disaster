using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour {

    bool isBurntYet = false; 
    public Material burnedMat;

    public void FireBurnedOut() {
        if (isBurntYet == false) {
            isBurntYet = true;
            Debug.Log("Tree is on fire!");

            for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).name.Contains ("tree")) {
					transform.GetChild(i).GetComponent<MeshRenderer>().material = burnedMat;
				}
            }
        }
    }
}
