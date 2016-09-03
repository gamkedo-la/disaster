using UnityEngine;
using System.Collections;

public class ThunderCloud : MonoBehaviour {

    public ParticleSystem lightningEffect;

    public void StartStorm()
    {
        lightningEffect.Play();
    }

    public void StopStorm()
    {
        lightningEffect.Stop();
    }
}
