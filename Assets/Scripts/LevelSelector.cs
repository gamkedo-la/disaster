using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

	public enum levelSelect { None, Level1, Level2 };
	public levelSelect currentAction = levelSelect.None;

    public void LoadMyLevel() {
        if (currentAction == levelSelect.Level1)
        {
            SceneManager.LoadScene("Scene 2");
        }
        else if (currentAction == levelSelect.Level2)
        {
            SceneManager.LoadScene("Level 3");
        }
    }

	void OnTriggerEnter(Collider other){
		if (other.tag == "RightController" || other.tag == "LeftController") {
            LoadMyLevel();
		}
	}
}
