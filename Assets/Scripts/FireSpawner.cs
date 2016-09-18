using UnityEngine;
using System.Collections;

public class FireSpawner : MonoBehaviour {

    public float ringRadius = 0.3f;
    public int fireCount = 6;
    public float fireLifeTime = 10.0f;
    private int currentFireIndex;
    private float rotationIncrement;
    public GameObject fireSpawn;

	// Use this for initialization
	void Start () {
        currentFireIndex = 0;

        if (fireCount > 0)
            rotationIncrement = 360.0f / fireCount;
        else
            rotationIncrement = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {
        //Spawn in the update instead of Start() to reduce lag upon creation

        //Spawn # of fires equal to fireCount evenly spaced out in a ring around the gameobject center
        //TODO : precreate a pool of fire objects, grab from the pool instead of instantiating
        if (fireCount > 0 && currentFireIndex < fireCount)
        {
            Vector3 nextPosition = GetSpawnPosition(rotationIncrement * currentFireIndex);
            GameObject singleFire = Instantiate(fireSpawn, nextPosition, Quaternion.LookRotation(nextPosition - transform.position)) as GameObject;
            Destroy(singleFire, fireLifeTime);
            currentFireIndex++;
        }
    }

    Vector3 GetSpawnPosition(float rAngle)
    {
        //Get the position at an angle of rAngle, at a distance of ringRadius away from the origin position

        //ignore Y position for now, get a position on the x, z plane of the gameobjects transform position

        float z = Mathf.Sin(rAngle * Mathf.Deg2Rad) * ringRadius;
        float x = Mathf.Cos(rAngle * Mathf.Deg2Rad) * ringRadius;

        return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }
}
