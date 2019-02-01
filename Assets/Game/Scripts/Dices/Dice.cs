using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Vector3 originalPosition;

    [Header("References")]
    public Rigidbody rbody;

    [Header("Data")]
    public int shownNumber;
    public bool isRolling;
    public bool checkValue;
    private Vector3 localYAxis;

    private void Start()
    {
        originalPosition = transform.position;
        if(rbody != null) rbody.isKinematic = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(isRolling) transform.rotation = Random.rotation;

        if(checkValue)
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

                checkValue = false;
            }
        }
    }
    
    public void RollDice ()
    {
        ResetDice();
        isRolling = true;
        rbody.isKinematic = false;
    }

    public bool CanBeRolled ()
    {
        return isRolling == false && checkValue == false && shownNumber == 0;
    }

    private int GetFaceNumber (Vector3 localYAxis)
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

    public void ResetDice (bool resetPosition = true)
    {
        if(resetPosition) transform.position = originalPosition;
        isRolling = false;
        checkValue = false;
        shownNumber = 0;
        rbody.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isRolling = false;
        checkValue = true;
    }
}
