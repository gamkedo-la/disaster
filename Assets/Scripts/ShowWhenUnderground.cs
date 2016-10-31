using UnityEngine;
using System.Collections;

public class ShowWhenUnderground : MonoBehaviour {

    Renderer bubble;
	// Use this for initialization
	void Start () {
        bubble = gameObject.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (WorldBounds.instance.SafelyAboveWater(transform.position) && transform.position.y > Terrain.activeTerrain.transform.position.y + Terrain.activeTerrain.SampleHeight(transform.position) + 0.08f)
        {
            bubble.enabled = false;
        }
        else {
            bubble.enabled = true;
        }
	}
}
