using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public bool rolling, checkValue;

    private Rigidbody rbody;
    private Vector3 originPosition;
    private int shownNumber;

    // Use this for initialization
    void Start ()
    {
		if(rbody == null)
        {
            rbody = GetComponent<Rigidbody>();
        }

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
                Vector3 localYAxis = transform.InverseTransformDirection(worldYAxis);
                localYAxis.Normalize();

                shownNumber = GetFaceNumber(localYAxis);

                if (shownNumber > 0)
                {
                    Debug.Log(gameObject.name + " result " + shownNumber + " local " + localYAxis);
                    Game.Instance.dices.AddDiceValue(shownNumber);
                }
                else
                {
                    Game.Instance.dices.ResetDices();
                    Debug.Log("Error de conexion, por favor intente de nuevo");
                }

                checkValue = false;
            }
        }
    }

    int GetFaceNumber (Vector3 localYAxis)
    {
        // What number resulted from dice roll
        if (localYAxis == Vector3.up)
        {
            shownNumber = 6;
        }
        else if (localYAxis == Vector3.down)
        {
            shownNumber = 1;
        }
        else if (localYAxis == Vector3.left)
        {
            shownNumber = 4;
        }
        else if (localYAxis == Vector3.right)
        {
            shownNumber = 3;
        }
        else if (localYAxis == Vector3.forward)
        {
            shownNumber = 2;
        }
        else if (localYAxis == Vector3.back)
        {
            shownNumber = 5;
        }

        return shownNumber;
    }

    public void RollDice()
    {
        // Apply gravity
        rbody.isKinematic = false;

        // Reset dices position
        transform.localPosition = originPosition;

        // Reset number result
        shownNumber = 0;

        // Roll dices
        rolling = true;

        // Check on value
        checkValue = true;
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
