using UnityEngine;
using System.Collections;

public class ThunderCloud : MonoBehaviour {

    public ParticleSystem lightningEffect;
    public ParticleSystem fireEffect;

    public void StartStorm()
    {
        lightningEffect.Play();
        fireEffect.Play();
    }

    public void StopStorm()
    {
        lightningEffect.Stop();
        fireEffect.Stop();
    }

    public void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
