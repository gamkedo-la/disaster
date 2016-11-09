using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {

    Vector3 startPos;
    Vector3 targetPos;
    float percTraveled = 0.0f;
    float arrowSpeed = 0.2f;
    float arcHeight = 0.3f;
    float missMax = 0.3f;
    public void SetupArrow(Vector3 startAt, Vector3 endAt) {
        startPos = startAt;
        float distFromPerfect = Random.Range(0.0f, missMax);
        Vector2 scatterOffset = Random.insideUnitCircle * distFromPerfect;
        targetPos = endAt + scatterOffset.x * Vector3.right + scatterOffset.y * Vector3.forward;
    }

	
	// Update is called once per frame
	void Update () {
        Vector3 diffToGoal = targetPos - transform.position;
        float distMovedThisFrame = Time.deltaTime * arrowSpeed;
        float distRemaining = diffToGoal.magnitude;
        if (percTraveled >= 1.0f) {
            Destroy(gameObject);
            return;
        }
        float heightBoostPerc;

        percTraveled += Time.deltaTime * arrowSpeed;
        float heightTempToSQ = (percTraveled - 0.5f) * 2.0f;
        heightBoostPerc = -(heightTempToSQ * heightTempToSQ) + 1;
       /* Vector3 a = startPos * (1.0f - percTraveled);
        Vector3 b = targetPos * percTraveled;
        Vector3 c = Vector3.up * heightBoostPerc * arcHeight;

        transform.position = a + b + c;*/

        transform.position = startPos * (1.0f - percTraveled) + targetPos * percTraveled + Vector3.up * heightBoostPerc * arcHeight;
        float nextPercTraveled = percTraveled + 0.05f;
        heightTempToSQ = (nextPercTraveled - 0.5f) * 2.0f;
        heightBoostPerc = -(heightTempToSQ * heightTempToSQ) + 1;
        Vector3 futurePosToPointAt = startPos * (1.0f - nextPercTraveled) + targetPos * nextPercTraveled + Vector3.up * heightBoostPerc * arcHeight;
        transform.LookAt(futurePosToPointAt);
    }
}
