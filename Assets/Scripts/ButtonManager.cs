using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    public bool restartLevel;
    public bool quitGame;

    void OnTriggerEnter(Collider other) {
        Debug.Log("Button manager here on " + name + " and someone " + other.name + " poked me!");
        if (other.tag == "RightController") {
            if (restartLevel)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (quitGame) {
                Application.Quit();
            }

        }
    }

}
