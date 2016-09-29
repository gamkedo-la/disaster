using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour {

    bool isBurntYet = false;
    Material[] materials;
    public Material burnedMat;

    public void FireBurnedOut() {
        if (isBurntYet == false) {
            isBurntYet = true;
            Debug.Log("Tree is on fire!");
            /*materials = GetComponentsInChildren<Material>();
            for (int i = 0; i < materials.Length; i++) {
                materials[i].
            }*/
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).GetComponent<MeshRenderer>().material = burnedMat;
            }
        }
    }
}
