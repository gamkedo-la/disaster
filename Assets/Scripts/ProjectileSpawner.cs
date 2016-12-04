using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour {

    public GameObject arrowObject;
    public static Transform arrowQuiver = null;
    PeopleMover target;
    float visionRadius = 3.2f;
    PeopleMover myMover;

    IEnumerator ConsiderShooting() {
        while (true && myMover.arrows) {
            yield return new WaitForSeconds(3.0f + Random.Range(2.0f, 8.0f));
            // If I'm dead stop shooting arrows (kills the coroutine)
            if (myMover.IsKnockedOver()) {
                yield break;
            }
            if (myMover.IsOnFire()) {
                continue;
            }
            // Forget target if target already dead
            if (target != null && target.IsKnockedOver()) {
                target = null;
            }
            // If no target go out and find oue
            if (target == null) {
                Collider[] collidersNearby = Physics.OverlapSphere(transform.position, visionRadius);
                for (int i = 0; i < collidersNearby.Length; i++) {
                    PeopleMover tempPM = collidersNearby[i].GetComponent<PeopleMover>();
                    if (tempPM && tempPM.teamNumber != myMover.teamNumber && tempPM.IsKnockedOver() == false && Random.Range(0.0f, 1.0f) < 0.2f) {
                        target = tempPM;
                        break;
                    }
                }
            }
            // If target shoot them!
            if (target != null) {
                GameObject tempGO = (GameObject)GameObject.Instantiate(arrowObject, transform.position, Quaternion.identity);
                tempGO.transform.SetParent(arrowQuiver);
                tempGO.transform.LookAt(target.transform.position);
                tempGO.layer = LayerMask.NameToLayer("ArrowTeam" + myMover.teamNumber);
                ProjectileMover tempMover = tempGO.GetComponent<ProjectileMover>();
                tempMover.SetupArrow(transform.position, target.transform.position);
                tempGO.name = "Arrow of " + name;
            } 
        }

    }
	// Use this for initialization
	void Start () {
        myMover = GetComponent<PeopleMover>();
        if (myMover.arrows == true) {
            if (arrowQuiver == null)
            {
                arrowQuiver = GameObject.Find("ArrowHolder").transform;

            }
            StartCoroutine(ConsiderShooting());
        }
        
	}
}
