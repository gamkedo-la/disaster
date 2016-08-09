using UnityEngine;
using System.Collections;
using System;

public class PeopleMover : MonoBehaviour {
    public Transform childTransform;
    bool scared = false;
    bool knockedOver = false;

    void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Meteor>() && knockedOver == false) {
            knockOver();
        }
    }

    private void knockOver()
    {
        GetComponent<PutOnGround>().enabled = false;
        GetComponentInChildren<CameraFacingScript>().enabled = false;
        Vector3 newSpot = transform.position;
        newSpot.y += 0.0109f;
        transform.position = newSpot;
        Quaternion rot = transform.rotation;
        childTransform.rotation = Quaternion.identity;
        transform.rotation = rot * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        knockedOver = true;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
