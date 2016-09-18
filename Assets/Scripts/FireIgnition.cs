using UnityEngine;
using System.Collections;

/****************
 * FireIgnition is part of the particle system script for fires.
 *  
 * Any object with a collider and this script attached will interact with particle systems that have 
 *      1. Collision turned on
 *      2. Send Collision Messages set to true (checked)
 * 
 * The goal of this script is to be able to react to a fire particle system touching the object that this 
 * script belongs to, starting a fire (or creating another fire particle system that is a child of this object)
 * **************/
public class FireIgnition : MonoBehaviour {
    bool isOnFire = false;
    public enum FireType
    {
        CONE, 
        MESH_RENDERER
    }
    public FireType fireType;
    public GameObject firePrefab;
    public float coneRadius;
    public float fuelAmount;
    public float burnRate;
    private float fireLifetime;
    private float elapsedTime;
    private GameObject theFireCreated;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(isOnFire)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= fireLifetime)
            {
                DestroyFire();
            }
        }
	}

    public bool IsOnFire()
    {
        return isOnFire;
    }

    private void DestroyFire()
    {
        if(theFireCreated != null)
        {
            Destroy(theFireCreated);
            theFireCreated = null;
            isOnFire = false; //will this cause the fire being destroyed to reignite the fire before it is destroyed?
            fireLifetime = 0;
            elapsedTime = 0;
        }
    }

    public void StartFire()
    {
        if (!isOnFire)
        {
            isOnFire = true;

            //Start a fire
            if (burnRate != 0)
                fireLifetime = fuelAmount / burnRate;
            else
                fireLifetime = 0;
            print("starting a (" + fireType.ToString() + ") fire on: (" + gameObject.name.ToString() + ") with a duration of (" + fireLifetime.ToString() + ") seconds");
            theFireCreated = Instantiate(firePrefab);
            theFireCreated.transform.parent = gameObject.transform;
            

            switch (fireType)
            {
                case FireType.CONE:
                    {
                        //newFire.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
                        ParticleSystem fireSystem = theFireCreated.GetComponent<ParticleSystem>();
                        if (fireSystem)
                        {
                            ParticleSystem.ShapeModule systemShape = fireSystem.shape;
                            systemShape.shapeType = ParticleSystemShapeType.Cone;
                            systemShape.angle = 0;
                            systemShape.radius = coneRadius;
                            print("set shape radius to " + coneRadius.ToString());
                        }
                    }
                    break;
                case FireType.MESH_RENDERER:
                    {
                        MeshRenderer theMesh = gameObject.GetComponent<MeshRenderer>();
                        if (theMesh)
                        {
                            //get the particle system from newFire

                            ParticleSystem fireSystem = theFireCreated.GetComponent<ParticleSystem>();
                            if (fireSystem)
                            {

                                print("set the mesh");
                                ParticleSystem.ShapeModule systemShape = fireSystem.shape;
                                systemShape.shapeType = ParticleSystemShapeType.MeshRenderer;
                                systemShape.meshRenderer = theMesh;
                            }
                        }
                    }
                    break;
            }
            
            //set position and move above burning object
            Vector3 newPosition = new Vector3(0.0f, 0.8f, 0.0f);
            theFireCreated.transform.localPosition = newPosition;
        }
    }
}
