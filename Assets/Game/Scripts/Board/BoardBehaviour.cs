using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardBehaviour : MonoBehaviour
{
    public BoardGameSet gameSet;
    public BoardPropertyInfo[] properties;

    private int amountOfSpacesToGo, currentSpaceVector, currentProperty;
    private Vector3[] spacesVector;

    // Start is called before the first frame update
    private void Start()
    {
        if(gameSet != null)
        {
            for(int i = 0; i < properties.Length; i++)
            {
                //Set sprites on the board
                properties[i].SetInfo(gameSet.sections[i].image,                
                    gameSet.sections[i].cardImage,
                    gameSet.sections[i].name,
                    gameSet.currencyString,
                    gameSet.sections[i].price,
                    gameSet.sections[i].rent,
                    gameSet.hotelString,
                    gameSet.houseString,
                    gameSet.transportString,
                    gameSet.serviceString);
            }
        }
    }

    public Vector3[] GetSpaces (int spaceIndex, TokenController token)
    {
        // If we move forwards
        if(IsForwardFaster(spaceIndex, token.currentTargetIndex, out amountOfSpacesToGo))
        {
            // Store all vectors that we are going to be moving along
            currentSpaceVector = 0;
            currentProperty = token.currentTargetIndex;            
            spacesVector = new Vector3[amountOfSpacesToGo];

            while(currentSpaceVector != amountOfSpacesToGo)
            {
                properties[currentProperty].LeaveSpace(token.tokenIndex);

                if(currentProperty == 39) 
                    currentProperty = 0;
                else 
                    currentProperty++;
                    
                spacesVector[currentSpaceVector] = properties[currentProperty].FindAvailablePoint(token.tokenIndex);

                currentSpaceVector++;
            }
        }
        // If we need to go backwards
        else
        {            
            // Store all vectors that we are going to be moving along
            currentSpaceVector = 0;
            currentProperty = token.currentTargetIndex;
            spacesVector = new Vector3[amountOfSpacesToGo];

            while(currentSpaceVector != amountOfSpacesToGo)
            {
                properties[currentProperty].LeaveSpace(token.tokenIndex);

                if(currentProperty == 0) 
                    currentProperty = 39;
                else 
                    currentProperty--;
                    
                spacesVector[currentSpaceVector] = properties[currentProperty].FindAvailablePoint(token.tokenIndex);

                currentSpaceVector++;
            }
        }

        //return properties[spaceIndex].FindAvailablePoint(token.tokenIndex);
        return spacesVector;
    }

    public bool IsForwardFaster (int target, int current, out int amountOfSpaces)
    {
        int forward = 0;
        int backwards = 0;
        int steps = current;

        // Check Forward
        while(steps != target)
        {
            forward++;
            if(steps == 39)
            {
                steps = 0;
            }
            else
            {
                steps++;
            }
            // Debug.Log("Forward: " + forward + " - CurrentStep: " + steps);
        }
        
        steps = current;
        // Check Backward
        while(steps != target)
        {
            backwards++;
            if(steps == 0)
            {
                steps = 39;
            }
            else
            {
                steps--;
            }
            // Debug.Log("Backwards: " + backwards + " - CurrentStep: " + steps);
        }

        if(forward < backwards)
        {
            amountOfSpaces = forward;
            return true;
        }
        else
        {
            amountOfSpaces = backwards;
            return false;
        }
    }
}
