using UnityEngine;
using System.Collections;

public class SteamManager : MonoBehaviour {

	public ParticleSystem steamEffect;

	public void SteamOn(){
		steamEffect.Play ();
		gameObject.GetComponent<AudioSource> ().Play ();
	}
}
