using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardBehaviour : MonoBehaviour
{
    public BoardGameSet gameSet;
    public BoardPropertyInfo[] properties;

    // Start is called before the first frame update
    private void Start()
    {
        if(gameSet != null)
        {
            for(int i = 0; i < properties.Length; i++)
            {
                //Set sprites on the board
                properties[i].SetInfo(gameSet.sections[i].image,
                    gameSet.sections[i].name,
                    gameSet.currencyString,
                    gameSet.sections[i].price);
            }
        }
    }

    public Vector3 MoveToSpace (int spaceIndex, TokenController player)
    {
        return properties[spaceIndex].FindAvailablePoint(player.tokenIndex);
    }
}
