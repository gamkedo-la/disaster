using UnityEngine;
using System.Collections;

public class ThunderCloud : MonoBehaviour {

    public ParticleSystem lightningEffect;
    public ParticleSystem fireEffect;
	public AudioClip  lightning;
	public AudioClip thunder;
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
		player.clip = thunder;
		player.Play ();
		yield return new WaitForSeconds (thunder.length);
		lightningEffect.Play();
		fireEffect.Play();
		player.clip = lightning;
		player.Play ();
	}
}
