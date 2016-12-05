using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MouseKeyboardSteer : MonoBehaviour {
    public GameObject meteorToThrow;
	//public GameObject rainCloud;
	//public GameObject thunderCloud;
    public Screenshot_Handler screenShotHandler;
    float camLat = 0.0f;
    float camLon = 0.0f;
	//public GameObject tornado;
	public GameObject volcanoMaker;
	float minAltitude = 0.0f;

	// Use this for initialization
	void Start () {
        screenShotHandler = GetComponent<Screenshot_Handler>();
	}
	
	// Update is called once per frame
	void Update () {
		float minHeight =
			Terrain.activeTerrain.transform.position.y + Terrain.activeTerrain.SampleHeight(transform.position) + 0.08f;
		float seaLevel = WorldBounds.instance.WaterHeight();
		if(minHeight < seaLevel) {
			minHeight = seaLevel;
		}
		if(transform.position.y < minHeight) {
			Vector3 fixedH = transform.position;
			fixedH.y = minHeight;
			transform.position = fixedH;
		}

		transform.position += transform.forward * 0.5f * Time.deltaTime * Input.GetAxis("Vertical");
		transform.position += transform.right * 0.4f * Time.deltaTime * Input.GetAxis("Horizontal");
		transform.position += Vector3.up * -0.4f * Time.deltaTime * Input.GetAxis("Elevation");

		camLat += Time.deltaTime * Input.GetAxis("Mouse Y") * 20.0f * -1.0f;
		camLat = Mathf.Clamp(camLat, -70.0f, 70.0f);
        camLon += Time.deltaTime * Input.GetAxis("Mouse X") * 20.0f;

        transform.rotation = Quaternion.Euler(camLat, camLon, 0.0f);

        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject tempGO = (GameObject) GameObject.Instantiate(meteorToThrow);
            tempGO.transform.position = transform.position;
            Rigidbody rb = tempGO.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 40.0f);
			tempGO.GetComponent<AudioSource> ().Play ();
        }

		/*if (Input.GetKeyDown (KeyCode.T)) {
			GameObject tempGo = (GameObject)GameObject.Instantiate (tornado);
			tempGo.transform.position = transform.position;
			tempGo.transform.rotation = transform.rotation;
			TornadoManager tm = tempGo.GetComponent<TornadoManager>();
			tm.AddPower(2.0f);
			tm.StartMoving();
			if (tempGo.GetComponentInChildren<Spinner>() != null) {
				tempGo.GetComponentInChildren<Spinner>().IncreasePower();
				tempGo.GetComponentInChildren<Spinner>().IncreasePower();
				tempGo.GetComponentInChildren<Spinner>().IncreasePower();
				tempGo.GetComponentInChildren<Spinner>().IncreasePower();
			}
		}*/

		if (Input.GetKeyDown (KeyCode.V)) {
			GameObject tempGO = (GameObject) GameObject.Instantiate(volcanoMaker);
			tempGO.transform.position = transform.position;
			Rigidbody rb = tempGO.GetComponent<Rigidbody>();
			rb.AddForce(transform.forward * 140.0f);
		}

		/*if(Input.GetKeyDown (KeyCode.R)) {
			RainCloud storm = rainCloud.GetComponent<RainCloud>();
			if (storm != null)
			{
				storm.StartStorm();
			}
		}

		if(Input.GetKeyDown (KeyCode.L)) {
			ThunderCloud storm = thunderCloud.GetComponent<ThunderCloud>();
			if (storm != null)
			{
				storm.StartStorm();
			}
		}*/
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenu");
        }
        if(Input.GetKeyDown(KeyCode.P))
            screenShotHandler.PressDown();

        if (Input.GetKeyUp(KeyCode.P))
            screenShotHandler.PressUp();
    }
}
