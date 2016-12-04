using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuTextSelector : MonoBehaviour {

	public GameObject credits;
	public GameObject title;
	public Text credit_text;
	public Text title_text;
	bool titleActive;
    float changeTimer = 2.0f;
    bool timerLock = false;

	// Use this for initialization
	void Awake () {
		title.SetActive (false);
		title_text.enabled = true;
		credits.SetActive (true);
		credit_text.enabled = false;
		titleActive = true;
	}
	
	public void ChangeText(){
        if (timerLock == false) {
            if (titleActive)
            {
                titleActive = false;
                title.SetActive(true);
                title_text.enabled = false;
                credits.SetActive(false);
                credit_text.enabled = true;
            }
            else
            {
                title.SetActive(false);
                title_text.enabled = true;
                credits.SetActive(true);
                credit_text.enabled = false;
                titleActive = true;
            }
            timerLock = true;
            changeTimer = 2.0f;
        }
	}

    void Update() {
        changeTimer -= Time.deltaTime;
        if (changeTimer <= 0.0f) {
            timerLock = false;
        }
    }
}
