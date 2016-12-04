using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuTextSelector : MonoBehaviour {

	public GameObject credits;
	public GameObject title;
	public Text credit_text;
	public Text title_text;
	bool titleActive;

	// Use this for initialization
	void Awake () {
		title.SetActive (false);
		title_text.enabled = true;
		credits.SetActive (true);
		credit_text.enabled = false;
		titleActive = true;
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "RightController" || other.tag == "LeftController") {
			if (titleActive) {
				titleActive = false;
				title.SetActive (true);
				title_text.enabled = false;
				credits.SetActive (false);
				credit_text.enabled = true;
			} else {
				title.SetActive (false);
				title_text.enabled = true;
				credits.SetActive (true);
				credit_text.enabled = false;
				titleActive = true;
			}
		}
	}
}
