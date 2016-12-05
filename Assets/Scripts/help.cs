using UnityEngine;
using System.Collections;

public class help : MonoBehaviour {

    public GameObject helpText;
    bool timerLock = false;
    float changeTimer = 2.0f;
    bool textEnabled = false;
	// Use this for initialization
	void Start () {
        helpText.SetActive(false);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightController" || other.tag == "LeftController")
        {
            if (timerLock == false && textEnabled == false)
            {
                timerLock = true;
                textEnabled = true;
                changeTimer = 2.0f;
                helpText.SetActive(true);
            }
            else if (timerLock == false && textEnabled == true) {
                timerLock = true;
                textEnabled = false;
                changeTimer = 2.0f;
                helpText.SetActive(false);
            }
        }
    }
    void Update()
    {
        changeTimer -= Time.deltaTime;
        if (changeTimer <= 0.0f)
        {
            timerLock = false;
        }
    }
}
