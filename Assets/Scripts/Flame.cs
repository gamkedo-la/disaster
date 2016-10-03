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
        //print("fire trigger entered by : " + other.gameObject.name);

        //check if collider belongs to the "Rain" gameObject, else try to start it on fire
        if (other.gameObject.name == "Rain")
        {
            RainCloud cloudScript = other.GetComponentInParent<RainCloud>();

            //Play rain on fire sound
            //TODO: add rain sound

            //Tell yourself and your parents the fire is being extinguished
            if (cloudScript) {
                if (cloudScript.IsRaining()) {
                    SendMessageUpwards("ExtinguishFire", SendMessageOptions.DontRequireReceiver);
                }
            }
            
        }
        else
            CheckForIgnition(other.gameObject);
        
    }

    private void ExtinguishFire()
    {
        Destroy(gameObject);
    }

    bool CheckForIgnition(GameObject other)
    {
        FireIgnition igniteScript = other.GetComponent<FireIgnition>();
        if (igniteScript != null)
        {
            //print("flame trigger enter on: " + other.name.ToString());
            //Object has an FireIgnition script, so start a fire 
            igniteScript.StartFire();
            return true;
        }
        return false;
    }

    //The particles colliding with an object 'other' call this function
    //start the object on fire if it has a FireIgnition script 
    void OnParticleCollision(GameObject other)
    {
        CheckForIgnition(other);
    }
}
