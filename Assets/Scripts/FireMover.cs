using UnityEngine;
using System.Collections;

public class FireMover : MonoBehaviour {
    public enum FireMovement
    {
        STRAIGHT, 
        STRAIGHT_VARIABLE, 
        STATIONARY, 
        STATIONARY_VARIABLE,
        RANDOM
    }

    public FireMovement movementType;
    public float moveSpeed;
    private float elapsedTime;
    private Vector3 moveDirection;
    public float varianceMaxDistance;
    private float varianceTracker;
    private float varianceUpdate;
    private bool bMoveRight;
    // Use this for initialization
    void Start () {
        bMoveRight = false;
        elapsedTime = 0;
        varianceUpdate = 0;
    }
	
	// Update is called once per frame
	void Update () {
	    switch(movementType)
        {
            case FireMovement.STRAIGHT:
                StraightMove();
                break;
            case FireMovement.STRAIGHT_VARIABLE:
                StraightVariableMove();
                break;
            case FireMovement.STATIONARY:
                StationaryMove();
                break;
            case FireMovement.STATIONARY_VARIABLE:
                StationaryVariableMove();
                break;
            case FireMovement.RANDOM:
                RandomMove();
                break;
        }
        ClampToTerrain();
	}
    
    void ClampToTerrain()
    {

    }

    void SetNewDirection(Vector3 newDirection)
    {

    }

    void AlignForward()
    {
        //Set the forward vector of the fire, this is the direction the fire will travel

    }

    void StraightMove()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }

    void StraightVariableMove()
    {
        elapsedTime += Time.deltaTime;
        //move randomly left or right after variableUpdate time has passed
        if (elapsedTime > varianceUpdate)
        {
            bMoveRight = Random.Range(0, 2) == 0 ? false : true;
            varianceUpdate = Random.Range(0.0f, 2.0f); //random amount of seconds between 0 and 2 seconds to change direction
            print("variableupdate timer set to: " + varianceUpdate.ToString());
            elapsedTime = 0;
        }
        float rMoveAmount = (bMoveRight ? 1 : -1) * Time.deltaTime * moveSpeed;
        
        if(varianceTracker + rMoveAmount > varianceMaxDistance)
        {
            //out of bounds, set rMoveAmount to the difference between max and tracker
            rMoveAmount = varianceMaxDistance - varianceTracker;
        }
        else if(varianceTracker + rMoveAmount < -varianceMaxDistance)
        {
            //out of bounds, set rMoveAmount to the difference between -max and tracker
            rMoveAmount = -varianceMaxDistance - varianceTracker;
        }
        varianceTracker += rMoveAmount;
        StraightMove(); // move straight, then move to the side;
        transform.position += transform.right * rMoveAmount;
    }

    void StationaryMove()
    {
        //TODO
    }

    void StationaryVariableMove()
    {
        //TODO

        //choose a random location within a radius 
    }

    void RandomMove()
    {
        //TODO

        
    }
}
