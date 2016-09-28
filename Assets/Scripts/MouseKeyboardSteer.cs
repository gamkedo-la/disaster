using UnityEngine;
using System.Collections;

public class MouseKeyboardSteer : MonoBehaviour {
    public GameObject meteorToThrow;
    public Screenshot_Handler screenShotHandler;
    float camLat = 0.0f;
    float camLon = 0.0f;

	// Use this for initialization
	void Start () {
        screenShotHandler = GetComponent<Screenshot_Handler>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.forward * 4.0f * Time.deltaTime * Input.GetAxis("Vertical");
        transform.position += Vector3.right * 4.0f * Time.deltaTime * Input.GetAxis("Horizontal");

		camLat += Time.deltaTime * Input.GetAxis("Mouse Y") * 50.0f * -1.0f;
        camLon += Time.deltaTime * Input.GetAxis("Mouse X") * 50.0f;

        transform.rotation = Quaternion.Euler(camLat, camLon, 0.0f);

        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject tempGO = (GameObject) GameObject.Instantiate(meteorToThrow);
            tempGO.transform.position = transform.position;
            Rigidbody rb = tempGO.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 40.0f);
        }

        if(Input.GetKeyDown(KeyCode.P))
            screenShotHandler.PressDown();

        if (Input.GetKeyUp(KeyCode.P))
            screenShotHandler.PressUp();
    }
}
