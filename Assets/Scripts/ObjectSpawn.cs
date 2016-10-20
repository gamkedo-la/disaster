using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawn : MonoBehaviour {

    public int howManyObjectsToSpawn = 20;
    public float creationRadius = 3.0f;
    public GameObject objectPrefab;
	public int team = 0;

    List<GameObject> objectList;
	// Use this for initialization
	void Start () {
        objectList = new List<GameObject>();
        for (int i = 0; i < howManyObjectsToSpawn; i++) {
            GameObject tempGo = (GameObject)GameObject.Instantiate(objectPrefab);
           
            Vector2 randOffset = Random.insideUnitCircle * creationRadius;
            Vector3 newSpot = transform.position;
            //Vector3 highY = new Vector3(0.0f, 1.0f, 0.0f);
            newSpot += Vector3.right * randOffset.x + Vector3.forward * randOffset.y;
            newSpot.y = Terrain.activeTerrain.transform.position.y;
            tempGo.transform.position = newSpot;

            tempGo.transform.parent = transform;
            PeopleMover pmScript = tempGo.GetComponent<PeopleMover>();
            if (pmScript) {
				pmScript.SetTeam(team);
            }
            objectList.Add(tempGo);
        }
                
	}
}
