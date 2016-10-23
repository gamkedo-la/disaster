using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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
	public TextAsset namesFile;
	List <string> namesList;
	string[] names;
	public FireIgnition fireScript;
    Text nameLabel;
    public bool wasJustOnFire = false;
    public int teamNumber;
    Vector3 scaredFrom;
	Vector3 scaredFromFire;
    bool screamed = false;
	public Renderer rend;
    bool underwater = false;
    Transform water;

    void OnTriggerEnter(Collider other) {
		if (teamNumber > 0) {
			Debug.Log("I've been entered by " + other.name);
			if (other.GetComponent<Meteor>() && knockedOver == false)
			{
				knockOver();
			}
			else if (other.name == "TornadoBody" && knockedOver == false) {
				knockOver();
			}
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
				SetScare(scaredFromFire);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetTeam(int team) {
        teamNumber = team;
    }

    IEnumerator TemporarilyFireproof() {
        wasJustOnFire = true;
        yield return new WaitForSeconds(2.0f);
        wasJustOnFire = false;
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
		if (underwater || teamNumber == 0) {
			return false;
		}
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
		string content = namesFile.text;
		namesList = new List<string> (content.Split ('\n'));
		names = namesList.ToArray ();
        water = GameObject.Find("Water4Advanced").GetComponent<Transform>();

        nameLabel.text = teamNumber + ": " + names[Random.Range (0, names.Length)];
        gameObject.name = nameLabel.text;
        numberID++;
		fireScript = GetComponent<FireIgnition>();
		rend.material.shader = Shader.Find ("Standard");
		if (teamNumber == 0) {
			rend.material.SetColor ("_Color", Color.green);
		} else {
			rend.material.SetColor ("_Color", Color.red);
		}

    }

    private void ScaredBehaivor() {
        Vector3 scaredOffset = transform.position - scaredFrom;
        scaredOffset.y = 0.0f;
        transform.position += scaredOffset.normalized * Time.deltaTime * scaredSpeed * speedModifier;
        scaredTimer -= Time.deltaTime;
		if (scream.isPlaying == false && screamed == false) {
            scream.pitch = Random.Range(0.6f, 1.6f);
			scream.Play ();
            screamed = true;
		}
        if (scaredTimer < 0) {
            scared = false;
            screamed = false;
        }
    }

    //The fire on this person burned out, it's entire lifetime has passed before being extinguished so the person has died. 
    public void FireBurnedOut() {
        if (Random.Range(0, 100) < 30)
        {
            knockOver();
        }
        else {
            StartCoroutine(TemporarilyFireproof());
        }
        
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
        if (transform.position.y < water.transform.position.y) {
            ExtinguishFire();
            knockOver();
			fireScript.ExtinguishFire ();
			wasJustOnFire = true;
        }
		if (teamNumber == 0) {
			wasJustOnFire = true;
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

        lastHeight = transform.position.y;
    }
}
