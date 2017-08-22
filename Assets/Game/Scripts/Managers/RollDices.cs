using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDices : MonoBehaviour
{
    public Dice[] dices;
	public int dicesValue, dicesRolled, pairsRolled, rollTries;
    public int testValueDice;

    public bool alreadyRolled = false;

	public Button rollDiceBtn, useCardBtn;    

	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.P) && !alreadyRolled)
        {
			RollBothDices ();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddDiceValue(testValueDice);

            if(dicesRolled >= 2)
            {
                dicesValue = 0;
                dicesRolled = 0;
            }
        }
	}

    public void AddDiceValue (int val)
    {
		if(!Game.Instance.GetPlayerReference().inJail)
		{
	        // We already rolled one
	        if (dicesRolled == 1)
	        {
	            // If we rolled same number
	            if (dicesValue == val)
	            {
	                // Give another roll
	                alreadyRolled = false;
	                pairsRolled++;
	            }
	        }

	        // Increase dice value
	        dicesValue += val;

	        // Increase rolled dice amount
	        dicesRolled += 1;

	        // When both dices are rolled then show the final result
	        if(dicesRolled >= 2 && pairsRolled < 3)
	        {
	            //Debug.Log(dicesValue);
	            Game.Instance.MoveToken(dicesValue);
	        }
	        else if(dicesRolled >= 2 && pairsRolled >= 3)
	        {
	            // Get player 
	            TokenController player = Game.Instance.GetPlayerReference();

	            // Force player to go to Jail
	            player.boardLocation = 10; // 10 = Jail
	            player.MoveToken();

	            // Tell player is in jail
	            player.SetPlayerInJail();

                // End of the turn
                Game.Instance.nextTurnButton.SetActive(false);
                alreadyRolled = true;
            }
		}
		else
		{
			// We already rolled one
			if (dicesRolled == 1) 
			{
				// If we rolled same number
				if (dicesValue == val) 
				{
					// Get out of jail and roll again
					Game.Instance.GetPlayerReference ().FreePlayerFromJail ();

					alreadyRolled = false;
					pairsRolled++;
				}
			}

			// Increase dice value
			dicesValue += val;

			// Increase rolled dice amount
			dicesRolled += 1;

			// We tried X times
			rollTries++;

            Game.Instance.nextTurnButton.SetActive(true);

            // When both dices are rolled then show the final result
            if (dicesRolled >= 2 && pairsRolled < 3 && !Game.Instance.GetPlayerReference().inJail)
			{
				//Debug.Log(dicesValue);
				Game.Instance.MoveToken(dicesValue);
				rollTries = 0;
                Game.Instance.nextTurnButton.SetActive(false);
            }
			else if(dicesRolled >= 2 && rollTries >= 6)
			{
				rollDiceBtn.interactable = false;
				rollTries = 0;
            }
        }
    }

    public void PayForAService(int val)
    {
        // Increase dice value
        dicesValue += val;

        // Increase rolled dice amount
        dicesRolled += 1;

        // When both dices are rolled then show the final result
        if (dicesRolled >= 2)
        {
            //Debug.Log(dicesValue);
            Game.Instance.actions.payment.PayWhatDicesSay(dicesValue);
        }
    }

    public void RollBothDices ()
	{
		// Restart value each time dices are going to roll
		dicesValue = 0;
		dicesRolled = 0;

		for(int i = 0; i < dices.Length; i++)
		{
			// Roll Dices
			dices[i].RollDice();
		}

        if(!Game.Instance.payingService)
        {
            alreadyRolled = true;
        }
	}

    public void ResetDices ()
    {
        // Restart value each time dices are going to roll
        dicesValue = 0;
        dicesRolled = 0;
		pairsRolled = 0;

        for (int i = 0; i < dices.Length; i++)
        {
            // Reset Dices
            dices[i].ResetDice();
        }

        // Can roll again
        alreadyRolled = false;
    }
}
