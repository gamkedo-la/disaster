using UnityEngine;
using System.Collections;

public class DestroyMe : MonoBehaviour {

    public int timer = 5;
    public ParticleSystem particlesToStop;
	// Use this for initialization
	void Start () {
        if (particlesToStop == null)
        {
            Destroy(gameObject, timer);
        }
        else {
            StartCoroutine(StopParticlesWaitDestroy());
        }
        
	}

    IEnumerator StopParticlesWaitDestroy() {
        yield return new WaitForSeconds(timer);
        ParticleSystem.EmissionModule emitter = particlesToStop.emission;
        emitter.enabled = false;
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
