using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour {

    bool isBurntYet = false;
    Material[] materials;

    public void Ignite() {
        if (isBurntYet == false) {
            isBurntYet = true;
            Debug.Log("Tree is on fire!");
            materials = GetComponentsInChildren<Material>();
            // #TODO add mertial changing here
        }
    }
}
