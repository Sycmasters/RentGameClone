using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaymentAction : MonoBehaviour
{
    public bool initialPayment = false;
    public Button initialPaymentButton;

    public void PayRent(int currPosition, int ownerIndex)
    {
        // How much will the rent be
        int rent = HowMuchToPay(currPosition, ownerIndex);

        // Charge Player
        Game.Instance.CurrentPlayer.SubstractCurrency(rent);
        Game.Instance.CurrentPlayer.RefreshPlayerInfo();
        Game.Instance.payerAvatar.sprite = Game.Instance.CurrentPlayer.playerAvatar.sprite;
        Game.Instance.payerName.text = Game.Instance.CurrentPlayer.playerName;

        // Pay owner
        Game.Instance.properties.playerDisplay[ownerIndex].AddCurrency(rent);
        Game.Instance.properties.playerDisplay[ownerIndex].RefreshPlayerInfo();
        Game.Instance.ownerAvatar.sprite = Game.Instance.properties.playerDisplay[ownerIndex].playerAvatar.sprite;
        Game.Instance.ownerName.text = Game.Instance.properties.playerDisplay[ownerIndex].playerName;

        // Show amount
        Game.Instance.payAmount.text = "G " + rent;
        Game.Instance.getPaidWindow.SetActive(true);
    }

    public int HowMuchToPay (int currPosition, int ownerIndex)
    {
        // First check if there are houses on the property
        if (Game.Instance.board.boardCardHouses[currPosition].currHouses > 0)
        {
            // Charge current player for number of houses
            int houseIndex = Game.Instance.board.boardCardHouses[currPosition].currHouses - 1;
            return Game.Instance.board.cardsPrice[currPosition].withProp[houseIndex];
        }
        else
        {
            // Check if player has all of the same property kind to get double rent
            if (Game.Instance.board.boardCardHouses[currPosition].DoesPlayerHaveThemAll(ownerIndex) && 
                !Game.Instance.board.boardCardHouses[currPosition].OthersHaveHouses(currPosition))
            {
                // Charge current player for double rent 
                return (Game.Instance.board.cardsPrice[currPosition].rent * 2);
            }
            else
            {
                // Charge current player for normal rent 
                return Game.Instance.board.cardsPrice[currPosition].rent;
            }
        }
    }

    public void EnableInitialPayment ()
    {
        initialPayment = true;
        initialPaymentButton.interactable = true;
    }

    public void DisableInitialPayment()
    {
        if (initialPayment == true)
        {
            // Set unclaimed money to the free parking
            Debug.Log("Unclaimed initial money");
        }

        initialPayment = false;
        initialPaymentButton.interactable = false;
    }

    public void ClaimInitialMoney ()
    {
        if(initialPayment)
        {
            Game.Instance.CurrentPlayer.AddCurrency(200);
            initialPayment = false;
            initialPaymentButton.interactable = false;
        }
    }

    public void PayToFreeJail ()
    {
        Game.Instance.CurrentPlayer.SubstractCurrency(50);
        Game.Instance.GetPlayerReference().FreePlayerFromJail();
        Game.Instance.jailWindow.SetActive(false);
        Game.Instance.dices.rollDiceBtn.interactable = true;
        Game.Instance.dices.rollTries = 0;
    }
}