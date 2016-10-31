using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldBounds : MonoBehaviour {

    public static WorldBounds instance;
    public Transform edgeN;
    public Transform edgeE;
    public Transform edgeS;
    public Transform edgeW;
    public Transform water;

    private float Nz;
    private float Ex;
    private float Sz;
    private float Wx;
    private float waterY;
    public float spawnMarginAboveWater = 0.05f;
	List<float> boundries;

    public bool SafelyAboveWater(Vector3 locationToCheck) {
        return locationToCheck.y > spawnMarginAboveWater + waterY;
    }

    public Vector3 ForceInbounds(Vector3 before) {
        Vector3 after = before;
        if (after.z > Nz) {
            after.z = Nz;
        }
        if (after.z < Sz)
        {
            after.z = Sz;
        }
        if (after.x > Ex)
        {
            after.x = Ex;
        }
        if (after.x < Wx)
        {
            after.x = Wx;
        }
        return after;
    }

	public List<float> Boundries(){
		boundries.Add (Nz);
		boundries.Add (Ex);
		boundries.Add (Sz);
		boundries.Add (Wx);
		return boundries;
	}

    void Awake () {
        instance = this;
        Nz = edgeN.position.z;
        Ex = edgeE.position.x;
        Sz = edgeS.position.z;
        Wx = edgeW.position.x;
        waterY = water.position.y;
		boundries = new List<float> ();
	}
}
