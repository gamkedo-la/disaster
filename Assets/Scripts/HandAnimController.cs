using UnityEngine;
using System.Collections;

public class HandAnimController : MonoBehaviour {

    private Animator anim;
    public bool grab = false;
    public bool point = false;
    public bool click = false;
    bool doUpdate = false;
	// Use this for initialization
	void Start () {
        //point = true;
        doUpdate = true;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (anim != null && doUpdate)
        {
            anim.SetBool("point", point);
            anim.SetBool("grab", grab);
            
           // anim.SetBool("click", click);
            doUpdate = false;
        }
	}

    public void SetTrigger(bool state)
    {
        grab = state;

        doUpdate = true;
    }

    public void SetPoint(bool state)
    {
        point = state;

        doUpdate = true;
    }
}
