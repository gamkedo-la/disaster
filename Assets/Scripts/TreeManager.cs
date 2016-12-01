using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour {

    bool isBurntYet = false; 
	public GameObject burnedTree;
	public GameObject liveTree;

	void Awake(){
		burnedTree.SetActive (false);
		Vector3 euler = transform.eulerAngles;
		euler.y = Random.Range (0.0f, 360.0f);
		transform.eulerAngles = euler;
	}

    public void FireBurnedOut() {
        if (isBurntYet == false) {
            isBurntYet = true;
			burnedTree.SetActive (true);
			liveTree.SetActive (false);
        }
    }
}
