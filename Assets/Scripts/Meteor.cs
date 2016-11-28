using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour {

	public GameObject explosionFire;
	public GameObject explosionSmoke;
    public GameObject explosionFireRing;

	private void Explode(){
        Debug.Log("In Explode");
		GameObject fire = Instantiate (explosionFire, gameObject.transform.position, Quaternion.identity) as GameObject;
		Destroy (fire, 10);

		GameObject smoke = Instantiate (explosionSmoke, gameObject.transform.position, Quaternion.identity) as GameObject;
        Destroy(smoke, 10);

        GameObject fireRing = Instantiate(explosionFireRing, gameObject.transform.position, Quaternion.identity) as GameObject;

        //if there is a TerrainDeformer script attached to this object, get the radius of the crater left by the meteor, set the fire ring radius
        TerrainDeformer deformerScript = GetComponent<TerrainDeformer>();
        if(deformerScript)
        {
            FireSpawner fireScript = fireRing.GetComponent<FireSpawner>();
            if(fireScript)
                fireScript.ringRadius = deformerScript.RadiusWorldUnits;
        }

        Destroy(fireRing, 10);

        Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other){
		if (other.name == "Terrain") {
			Explode ();
		}
	}
}
