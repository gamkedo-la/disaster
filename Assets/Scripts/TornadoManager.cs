using UnityEngine;
using System.Collections;

public class TornadoManager : MonoBehaviour {

    float windPower = 0.0f;
    public float maxForce = 10.0f;
    
    // Use this for initialization
	void Start () {
	
	}

    public void AddPower(float powerIncrease) {
        windPower += powerIncrease;
        if (windPower > maxForce) {
            windPower = maxForce;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
