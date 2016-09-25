using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PeopleMover : MonoBehaviour {
    public Transform childTransform;
    public float scareRadius = 0.25f;
    public float scaredSpeed = 0.2f;
    public float scaredTimer = 0.0f;
    public float scaredTimerDefault = 1.0f;
    float lastHeight;
    float speedModifier = 1.0f;
    bool scared = false;
    bool knockedOver = false;
	public AudioSource scream;
    public static int numberID = 0;
	FireIgnition fireScript;
    Text nameLabel;

    Vector3 scaredFrom;
	Vector3 scaredFromFire;

    void OnTriggerEnter(Collider other) {
        Debug.Log("I've been entered by " + other.name);
        if (other.GetComponent<Meteor>() && knockedOver == false)
        {
            knockOver();
        }
        else if (other.name == "TornadoBody" && knockedOver == false) {
            knockOver();
        }
    }

    IEnumerator AIReact() {
        yield return new WaitForSeconds(Random.Range(0.0f, 0.2f));
        int peopleMask = LayerMask.GetMask("People");
        Collider myCollider = GetComponent<Collider>();
        while (true) {
            Collider[] hitList = Physics.OverlapSphere(transform.position, scareRadius, peopleMask);
            for (int i = 0; i < hitList.Length; i++) {
                if (hitList[i] != myCollider) {
                    PeopleMover pmScript = hitList[i].GetComponent<PeopleMover>();
					scaredFromFire = pmScript.transform.position;  // this will use the closest person for the random scared location when they are on fire.
                    if (pmScript.IsKnockedOver()) {
						SetScare(pmScript.transform.position);
                    }
                }
            }

            if(!scared && IsOnFire())
            {
//                scared = true;
//                scaredFrom = transform.position;
//                scaredTimer = scaredTimerDefault;
				SetScare(scaredFromFire);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public bool IsKnockedOver() {
        return knockedOver;
    }

	void SetScare(Vector3 position){
		scared = true;
		scaredFrom = position;
		scaredTimer = scaredTimerDefault;
	}

    public bool IsOnFire()
    {
        if(fireScript)
        {
            return fireScript.IsOnFire();
        }
        return false;
    }

    void Start() {
        StartCoroutine(AIReact());
        lastHeight = Terrain.activeTerrain.SampleHeight(transform.position) + Terrain.activeTerrain.transform.position.y;
        nameLabel = GetComponentInChildren<Text>();
        nameLabel.text = nameLabel.text + "#" + numberID;
        gameObject.name = nameLabel.text;
        numberID++;
		fireScript = GetComponent<FireIgnition>();
    }

    private void ScaredBehaivor() {
        Vector3 scaredOffset = transform.position - scaredFrom;
        scaredOffset.y = 0.0f;
        transform.position += scaredOffset.normalized * Time.deltaTime * scaredSpeed * speedModifier;
        scaredTimer -= Time.deltaTime;
		if (scream.isPlaying == false) {
            scream.pitch = Random.Range(0.6f, 1.6f);
			scream.Play ();
		}
        if (scaredTimer < 0) {
            scared = false;
        }
    }

    //The fire on this person burned out, it's entire lifetime has passed before being extinguished so the person has died. 
    public void FireBurnedOut() {
        knockOver();
    }

    public void ExtinguishFire()
    {
        //Flame was put out early
        //print("flame on person: " + nameLabel.text +" was put out early");
        scaredTimer = 0;
        scared = false;
    }

    public void knockOver()
    {
        GetComponentInChildren<CameraFacingScript>().enabled = false;
        Vector3 newSpot = transform.position;
        newSpot.y += 0.0109f;
        transform.position = newSpot;
        childTransform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
        knockedOver = true;
    }

    public void MakeScared() {
        scared = true;
    }

    // Update is called once per frame
    void Update () {
        transform.position = WorldBounds.instance.ForceInbounds(transform.position);
        if (IsOnFire()) {
			SetScare(scaredFromFire);
        }
        if (scared && knockedOver == false) {
            ScaredBehaivor();
        }
        
	}

    void FixedUpdate() {
        float changeInY = transform.position.y - lastHeight;
        float speedModifierTarget = 1.0f;
        if (changeInY > 0.0f)
        {
            speedModifierTarget += changeInY * -500.5f;
        }
        else {
            speedModifierTarget += changeInY * -1.0f;
        }
        float slopeSmoothK = 0.2f;
        speedModifier = slopeSmoothK * speedModifier + (1.0f - slopeSmoothK) * speedModifierTarget;

        if (nameLabel.text == "Bob#3")
        {
            //Debug.Log("SpeedModifier is " + speedModifier + " for " + nameLabel.text);
        }
        lastHeight = transform.position.y;
    }
}
