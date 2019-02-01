using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardPropertyInfo : MonoBehaviour
{
    public SpriteRenderer render;
    public TextMeshPro nameDisplay, priceDisplay;
    public string propName;
    public int propPrice;
    public int[] playersOnThisZone = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };

    /*     
        TL0 TC1 TR2
        ML3 MC4 MR5
        BL6 BC7 BR8
    */

    private float xOffset = 0.148f, zOffset = 0.22f;

    private void Start ()
    {
        if(playersOnThisZone == null || playersOnThisZone.Length <= 0)
        {
            playersOnThisZone = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        }
    }
    
    public void SetInfo (Sprite image, string name, string currency, int price)
    {
        render.sprite = image;

        propName = name;
        if(nameDisplay != null)
            nameDisplay.text = propName;
        
        propPrice = price;
        if(priceDisplay != null)
            priceDisplay.text = currency + propPrice;
    }

    public Vector3 FindAvailablePoint (int playerIndex)
    {
        Vector3 returnValue = transform.position;

        // Always try to return center point
        if(playersOnThisZone[4] == -1)
        {
            playersOnThisZone[4] = playerIndex;
            return returnValue;
        }
        else
        {
            // Store available spaces
            List<int> availableSpaces = new List<int>();
            for(int i = 0; i < playersOnThisZone.Length; i++)
            {
                if(playersOnThisZone[i] == -1)
                {
                    availableSpaces.Add(i);
                }
            }

            // Choose between available spaces randomly
            int rmdPoint = Random.Range(0, availableSpaces.Count);
            playersOnThisZone[availableSpaces[rmdPoint]] = playerIndex;
            
            // Convert that space into a vector that wwe are going to position ourself
            returnValue = ConvertPointToVector(availableSpaces[rmdPoint]);

            return returnValue;
        }
    }

    public Vector3 ConvertPointToVector (int point)
    {
        // Left - / Right + / Up + / Down -
        switch(point) 
        {
            case 0 : return transform.position + (Vector3.right * -xOffset) + (Vector3.forward * zOffset); // TL
            case 1 : return transform.position + (Vector3.forward * zOffset); // TC
            case 2 : return transform.position + (Vector3.right * xOffset) + (Vector3.forward * zOffset); // TR
            case 3 : return transform.position + (Vector3.right * -xOffset); // ML
            case 4 : return transform.position; // MC
            case 5 : return transform.position + (Vector3.right * xOffset); // MR
            case 6 : return transform.position + (Vector3.right * -xOffset) + (Vector3.forward * -zOffset); // BL
            case 7 : return transform.position + (Vector3.forward * -zOffset); // BC
            case 8 : return transform.position + (Vector3.right * xOffset) + (Vector3.forward * -zOffset); // BR    
            default: return transform.position;  
        }
    }

    public void LeaveSpace (int playerIndex)
    {
        for(int i = 0; i < playersOnThisZone.Length; i++)
        {
            if(playersOnThisZone[i] == playerIndex)
            {
                playersOnThisZone[i] = -1;
                break;
            }
        }
    }
}
