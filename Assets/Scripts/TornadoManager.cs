using UnityEngine;
using System.Collections;

public class TornadoManager : MonoBehaviour {

    public float windPower = 0.0f;
    public float maxForce = 2.0f;
    bool isMoving = false;

    public void AddPower(float powerIncrease) {
        windPower += powerIncrease;
        if (windPower > maxForce) {
            windPower = maxForce;
        }
    }

    public void StartMoving() {
        isMoving = true;
        Destroy(gameObject, 4.0f);
    }

    void MoveTornado() {
        transform.Translate(Vector3.forward * Time.deltaTime * windPower);
    }

	// Update is called once per frame
	void Update () {
        if (isMoving) {
            MoveTornado();
        }
    }
}
