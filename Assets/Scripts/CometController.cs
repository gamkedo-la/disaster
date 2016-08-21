using UnityEngine;
using System.Collections;

public class CometController : MonoBehaviour {

    public Transform[] cometSpawnPoints;
    public GameObject comet;
    public Transform playAreaCenter;
    public float cometSpeed = 13.0f;


    IEnumerator CometSpawner() {
        yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        while (true) {
            SpawnComet();
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }

    void SpawnComet() {
        GameObject tempComet = (GameObject) GameObject.Instantiate(comet);
        int rand = Random.Range(0, 3);
        tempComet.transform.position = cometSpawnPoints[rand].position;
        Vector3 randLoc = tempComet.transform.position;
        Vector2 randOffset = Random.insideUnitCircle * 2.0f;
        randLoc += Vector3.right * randOffset.x + Vector3.forward * randOffset.y;
        tempComet.transform.position = randLoc;
        Quaternion rot = cometSpawnPoints[rand].localRotation;
        tempComet.transform.rotation = rot;
        Rigidbody rb = tempComet.GetComponent<Rigidbody>();
        rb.velocity = tempComet.transform.forward * cometSpeed;
        Destroy(tempComet, 4.0f);
    }


    void Start() {
        StartCoroutine(CometSpawner());
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M)) {
            //StartCoroutine(Spawncomet());
            SpawnComet();
        }
	}
}
