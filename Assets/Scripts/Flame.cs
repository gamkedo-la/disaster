using UnityEngine;
using System.Collections;

public class Flame : MonoBehaviour {
    /*********
     * Flame is a fire starter. 
     * Objects collided with will start on fire if burnable.
     * *******/
	// Use this for initialization
	void Start () {
	    
	}
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        print("trigger enter");
        CheckForIgnition(other.gameObject);
    }

    void CheckForIgnition(GameObject other)
    {
        FireIgnition igniteScript = other.GetComponent<FireIgnition>();
        if (igniteScript != null)
        {
            //print("flame trigger enter on: " + other.name.ToString());
            //Object has an FireIgnition script, so start a fire 
            igniteScript.StartFire();
        }
    }

    //The particles colliding with an object 'other' call this function
    //start the object on fire if it has a FireIgnition script 
    void OnParticleCollision(GameObject other)
    {
        CheckForIgnition(other);
    }
}
