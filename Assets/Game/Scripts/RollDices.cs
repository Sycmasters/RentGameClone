using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDices : MonoBehaviour
{
    public Dice[] dices;
    public int dicesValue;

    public int dicesRolled;

    private bool alreadyRolled = false;

	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.P) && !alreadyRolled)
        {
            // Restart value each time dices are going to roll
            dicesValue = 0;
            dicesRolled = 0;

            for(int i = 0; i < dices.Length; i++)
            {
                // Roll Dices
                dices[i].RollDice();
            }

            alreadyRolled = true;
        }
	}

    public void AddDiceValue (int val)
    {
        // Increase dice value
        dicesValue += val;

        // Increase rolled dice amount
        dicesRolled += 1;

        // When both dices are rolled then show the final result
        if(dicesRolled >= 2)
        {
            //Debug.Log(dicesValue);
            Game.Instance.MoveToken(dicesValue);
        }
    }

    public void ResetDices ()
    {
        // Restart value each time dices are going to roll
        dicesValue = 0;
        dicesRolled = 0;

        for (int i = 0; i < dices.Length; i++)
        {
            // Reset Dices
            dices[i].ResetDice();
        }

        // Can roll again
        alreadyRolled = false;
    }
}
