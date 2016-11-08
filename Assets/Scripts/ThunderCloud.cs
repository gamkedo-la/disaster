using UnityEngine;
using System.Collections;

public class ThunderCloud : MonoBehaviour {

    public ParticleSystem lightningEffect;
    public ParticleSystem fireEffect;
	public AudioSource player;

    public void StartStorm()
    {
		StartCoroutine (ThunderStormSounds ());
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

	IEnumerator ThunderStormSounds(){
		lightningEffect.Play();
		fireEffect.Play();
		yield return new WaitForSeconds (1);
		player.Play ();
	}
}
