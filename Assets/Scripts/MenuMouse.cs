using UnityEngine;
using System.Collections;

public class MenuMouse : MonoBehaviour {

    void Start() {
        Cursor.visible = true;
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rhInfo;
            if (Physics.Raycast(ray, out rhInfo)) {
                Debug.Log("Clicked on: " + rhInfo.collider.name);
                MenuTextSelector mtsScript = rhInfo.collider.GetComponentInParent<MenuTextSelector>();
                if (mtsScript) {
                    mtsScript.ChangeText();
                }
                LevelSelector lsScript = rhInfo.collider.GetComponent<LevelSelector>();
                if (lsScript) {
                    lsScript.LoadMyLevel();
                }
            } //Raycast hit end
        } // mouse button down end
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
	}
}
