using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public bool rolling, checkValue, checkAgain;
    
    [SerializeField]
    private Rigidbody rbody;
    public Vector3 originPosition, localYAxis;
    public int shownNumber;

    [ContextMenu("GetReferences")]
    public void Init ()
    {
        rbody = GetComponent<Rigidbody>();
        Debug.Log("Init method in " + gameObject.name);
    } 

    // Use this for initialization
    void Awake ()
    {
        // Original position
        originPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Dice is rolling
		if(rolling)
        {
            // Rotate dice
            transform.rotation = Random.rotation;
        }
        
        if (checkValue)
        {
            // Check if the dice stopped moving/rolling
            if (rbody.velocity.sqrMagnitude < 0.01f && rbody.IsSleeping())
            {
                // Get right direction in Y axis
                Vector3 worldYAxis = -Physics.gravity;
                // Get dice direction in Y axis
                localYAxis = transform.InverseTransformDirection(worldYAxis);
                localYAxis.Normalize();

                shownNumber = GetFaceNumber(localYAxis);

                if (shownNumber > 0)
                {
                    //Debug.Log(gameObject.name + " result " + shownNumber + " local " + localYAxis);
                    if(!Game.Instance.payingService)
                    {
                        // We are moving the player
                        Game.Instance.dices.AddDiceValue(shownNumber);
                    }
                    else
                    {
                        // We are paying for a service
                        Game.Instance.dices.PayForAService(shownNumber);
                    }
                }
                else
                {
                    Debug.LogError("Error, try again");
                }

                checkValue = false;
            }
        }
    }

    int GetFaceNumber (Vector3 localYAxis)
    {
        // What number resulted from dice roll
        if (Mathf.Round(localYAxis.y) > 0)
        {
            shownNumber = 6;
        }
        else if (Mathf.Round(localYAxis.y) < 0)
        {
            shownNumber = 1;
        }
        else if (Mathf.Round(localYAxis.x) < 0)
        {
            shownNumber = 4;
        }
        else if (Mathf.Round(localYAxis.x) > 0)
        {
            shownNumber = 3;
        }
        else if (Mathf.Round(localYAxis.z) > 0)
        {
            shownNumber = 2;
        }
        else if (Mathf.Round(localYAxis.z) < 0)
        {
            shownNumber = 5;
        }

        return shownNumber;
    }

    public void RollDice(bool reset = true)
    {
        // Apply gravity
        rbody.isKinematic = false;
        
        // Reset number result
        shownNumber = 0;

        // Roll dices
        rolling = true;

        // Check on value
        checkValue = true;

        if (reset)
        {
            // Reset dices position
            transform.localPosition = originPosition;
        }
    }

    public void ResetDice ()
    {
        // Reset dice position and physics
        rbody.isKinematic = true;
        transform.localPosition = originPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rolling = false;
    }
}
