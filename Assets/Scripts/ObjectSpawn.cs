using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawn : MonoBehaviour {

    public int howManyObjectsToSpawn = 20;
    public float creationRadius = 3.0f;
    public GameObject objectPrefab;
	public int team = 0;

    List<GameObject> objectList = new List<GameObject>();
	// Use this for initialization
	void Start () {
        
        for (int i = 0; i < howManyObjectsToSpawn; i++) {
            GameObject tempGo = (GameObject)GameObject.Instantiate(objectPrefab);
            Vector3 newSpot;
            int safteyBreak = 100;
            do
            {
                Vector2 randOffset = Random.insideUnitCircle * creationRadius;
                newSpot = transform.position;
                //Vector3 highY = new Vector3(0.0f, 1.0f, 0.0f);
                newSpot += Vector3.right * randOffset.x + Vector3.forward * randOffset.y;
                newSpot.y = Terrain.activeTerrain.SampleHeight(newSpot) + Terrain.activeTerrain.transform.position.y;
                if (safteyBreak-- < 0) {
                    Debug.Log("Could not find space to place " + tempGo.name);
                    break;
                }
            } while (WorldSettings.instance.IsSpaceClearNear(newSpot) == false || WorldBounds.instance.SafelyAboveWater(newSpot) == false);

            tempGo.transform.position = newSpot;

            tempGo.transform.parent = transform;
            PeopleMover pmScript = tempGo.GetComponent<PeopleMover>();
            if (pmScript) {
				pmScript.SetTeam(team);
            }
            objectList.Add(tempGo);
        }
                
	}

    public bool AmITooClose(Vector3 position, float tooClose) {
        foreach (GameObject eachGO in objectList) {
            Vector3 flatPosDiff = position - eachGO.transform.position;
            flatPosDiff.y = 0;
            if (flatPosDiff.magnitude < tooClose) {
                return true;
            }
        } // end of foreach
        return false;
    }
} // end of class
