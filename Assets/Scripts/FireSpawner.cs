using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireSpawner : MonoBehaviour {

    public enum FireSource
    {
        METEOR,     //METEOR: Spawns a ring of fire around the current transform.position
        LIGHTNING,  //LIGHTNING: Spawn a single fire at the point of particle impact
        VOLCANO     //VOLCANO: Spawn a single fire at point of particle impact. Or a series of fires as the lava move along the ground? 
    }
    public FireSource sourceOfFire;
    public float ringRadius = 0.3f;
    public int fireCount = 6;
    public float fireLifeTime = 10.0f;
    private int currentFireIndex;
    private float rotationIncrement;
    public GameObject fireSpawn;
    ParticleSystem theParticleSystem;
    List<ParticleSystem.Particle> enterParticles = new List<ParticleSystem.Particle>();
    Vector3 nextFirePosition = Vector3.zero;

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

        switch(sourceOfFire)
        {
            case FireSource.METEOR:
                //Spawn in the update instead of Start() to reduce lag upon creation

                //Spawn # of fires equal to fireCount evenly spaced out in a ring around the gameobject center
                //TODO : precreate a pool of fire objects, grab from the pool instead of instantiating
                if (fireCount > 0 && currentFireIndex < fireCount)
                {
                    Vector3 nextPosition = GetSpawnPosition_Ring(rotationIncrement * currentFireIndex);
                    GameObject singleFire = Instantiate(fireSpawn, nextPosition, Quaternion.LookRotation(nextPosition - transform.position)) as GameObject;
                    Destroy(singleFire, fireLifeTime);
                    currentFireIndex++;
                }
                break;

            case FireSource.LIGHTNING:
                //If nextFirePosition is not zero, then create a fire at this position
                if(nextFirePosition != Vector3.zero)
                {
                    GameObject singleFire = Instantiate(fireSpawn, nextFirePosition, Quaternion.identity) as GameObject;
                    FireMover moverScript = singleFire.GetComponent<FireMover>();
                    if (moverScript)
                    {
                        moverScript.movementType = FireMover.FireMovement.STATIONARY;
                    }
                    Destroy(singleFire, fireLifeTime);
                    nextFirePosition = Vector3.zero;
                }
                break;

            case FireSource.VOLCANO:
                //Handled in OnParticleCollision
                break;

            default:
                print(gameObject.name + ".FireSpawner in Update() : Invalid FireSource found. No fire created.");
                break;

        }
    }

    Vector3 GetSpawnPosition_Ring(float rAngle)
    {
        //Get the position at an angle of rAngle, at a distance of ringRadius away from the origin position

        //ignore Y position for now, get a position on the x, z plane of the gameobjects transform position

        float z = Mathf.Sin(rAngle * Mathf.Deg2Rad) * ringRadius;
        float x = Mathf.Cos(rAngle * Mathf.Deg2Rad) * ringRadius;

        return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }

    void OnParticleTrigger()
    {
        //Using the trigger module of the particle system. Any particles that enter the collider objects listed in the Colliders array of the module will call this function

        if (theParticleSystem == null)
            theParticleSystem = GetComponent<ParticleSystem>();

        int numParticles = theParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles);

        for(int i = 0; i < numParticles; i++)
        {
            ParticleSystem.Particle p = enterParticles[i];

            //Play particle collision sound? 

            //Set the nextFirePosition to create a fire - this will only create 1 fire per frame even if multiple particles are triggering on this frame
            nextFirePosition = transform.position; //p.position is in local space, the lightning is being shot in the forward vector of the particle system. Hardcode the position correction for now, cause I don't know any other way to adjust.
            nextFirePosition.y += -p.position.z;
            nextFirePosition.x += p.position.x;
            nextFirePosition.z += p.position.y;

           // print("position of particle trigger " + p.position.ToString());
        }

    }
}
