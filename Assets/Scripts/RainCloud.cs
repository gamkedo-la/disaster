using UnityEngine;
using System.Collections;

public class RainCloud : MonoBehaviour {

    public ParticleSystem rainEffect;

    public bool IsRaining() {
        return rainEffect.isPlaying;
    }

    public void StartStorm() {
        rainEffect.Play();
    }

    public void StopStorm() {
        rainEffect.Stop();
    }
}
