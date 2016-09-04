using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

    public float spinPower = 0.0f;
    public float spinPowerUp = 150.0f;
    ParticleSystem tornadoParticles;
    float endRadius = 0.05f;
    float startRadius = 0.5f;
    float maxSpinPower = 600.0f;
    float startParticleSize = 0.01f;
    float endParticleSize = 0.05f;

    void Start() {
        tornadoParticles = gameObject.GetComponent<ParticleSystem>();
        BaseParticlesOnSpeed();
    }

    void BaseParticlesOnSpeed() {
        ParticleSystem.ShapeModule tempShape;
        tempShape = tornadoParticles.shape;
        tempShape.angle = spinPower * 0.05f;
        tempShape.radius = Mathf.Lerp(startRadius, endRadius, spinPower / maxSpinPower);
        tornadoParticles.startSize = Mathf.Lerp(startParticleSize, endParticleSize, spinPower / maxSpinPower);
        Debug.Log("spin power = " + spinPower);
        //tornadoParticles.shape = tempShape;
    }

    public void IncreasePower() {
        spinPower += spinPowerUp;
        if (spinPower > maxSpinPower) {
            spinPower = maxSpinPower;
        }
        BaseParticlesOnSpeed();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward, spinPower * Time.deltaTime);
	}
}
