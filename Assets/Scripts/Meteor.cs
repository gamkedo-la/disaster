using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour {

	public GameObject explosionFire;
	public GameObject explosionSmoke;

	private void Explode(){
		GameObject fire = Instantiate (explosionFire, gameObject.transform.position, Quaternion.identity) as GameObject;
		Destroy (fire, 10);

		GameObject smoke = Instantiate (explosionSmoke, gameObject.transform.position, Quaternion.identity) as GameObject;
		Destroy (smoke, 10);
	}

	void OnTriggerEnter(Collider other){
		if (other.name == "Terrain") {
			Explode ();
		}
	}
}
