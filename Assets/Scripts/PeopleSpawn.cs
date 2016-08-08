using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeopleSpawn : MonoBehaviour {

    public int howManyObjectsToSpawn = 20;
    public float creationRadius = 3.0f;
    public GameObject objectPrefab;

    List<GameObject> objectList;
	// Use this for initialization
	void Start () {
        objectList = new List<GameObject>();
        for (int i = 0; i < howManyObjectsToSpawn; i++) {
            GameObject tempGo = (GameObject)GameObject.Instantiate(objectPrefab);
           
            Vector2 randOffset = Random.insideUnitCircle * creationRadius;
            Vector3 newSpot = transform.position;
            newSpot += Vector3.right * randOffset.x + Vector3.forward * randOffset.y;
            newSpot.y = Terrain.activeTerrain.SampleHeight(newSpot);
            tempGo.transform.position = newSpot;

            tempGo.transform.parent = transform;

            objectList.Add(tempGo);
        }
                
	}
}
