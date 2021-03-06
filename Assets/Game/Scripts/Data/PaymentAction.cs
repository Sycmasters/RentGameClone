﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaymentAction : MonoBehaviour
{
    public bool initialPayment = false;
    public Button initialPaymentButton;

    public TextMesh parkingMoneyLabel;
    public GameObject parkingMoneyObject;

    // Outside of this script
    public bool payDoubleTransport;
    public bool payTenTimes;

    public int stillNeedToPay;
    public int whoIOwe;

    public void PayRent(int currPosition, int ownerIndex)
    {
        // How much will the rent be
        int rent = HowMuchToPay(currPosition, ownerIndex);

        // Pay owner
        if(rent > Game.Instance.CurrentPlayer.playerCurrency)
        {
            Game.Instance.properties.playerDisplay[ownerIndex].AddCurrency(Game.Instance.CurrentPlayer.playerCurrency);
            stillNeedToPay = rent - Game.Instance.CurrentPlayer.playerCurrency;
            whoIOwe = ownerIndex;
        }
        Game.Instance.properties.playerDisplay[ownerIndex].RefreshPlayerInfo();
        Game.Instance.ownerAvatar.sprite = Game.Instance.properties.playerDisplay[ownerIndex].playerAvatar.sprite;
        Game.Instance.ownerName.text = Game.Instance.properties.playerDisplay[ownerIndex].playerName;

        // Charge Player
        Game.Instance.CurrentPlayer.SubstractCurrency(rent);
        Game.Instance.CurrentPlayer.RefreshPlayerInfo();
        Game.Instance.payerAvatar.sprite = Game.Instance.CurrentPlayer.playerAvatar.sprite;
        Game.Instance.payerName.text = Game.Instance.CurrentPlayer.playerName;


        // Show amount
        Game.Instance.payAmount.text = "G " + rent;
        Game.Instance.getPaidWindow.SetActive(true);
    }

    public void PayWhatDicesSay (int dicesValue)
    {
        // First check how many services we have
        int servicesAmount = 0;
        // What services are we implementing, if any or both
        int cardToCheck = Game.Instance.WhoOwnsThisCard(12) == -1 ? 28 : 12;
        // Who is the owner of this service(s)
        int ownerIndex = Game.Instance.WhoOwnsThisCard(cardToCheck);

        if (Game.Instance.properties.playerDisplay[ownerIndex].propertiesOwned.Contains(12)) { servicesAmount++; }
        if (Game.Instance.properties.playerDisplay[ownerIndex].propertiesOwned.Contains(28)) { servicesAmount++; }

        int totalToMultiply = 0;

        if (!payTenTimes)
        {
            totalToMultiply = Game.Instance.board.cardsPrice[cardToCheck].withProp[servicesAmount - 1];
        }
        else
        {
            totalToMultiply = Game.Instance.board.cardsPrice[cardToCheck].withProp[1];
        }

        // Charge Player
        Game.Instance.CurrentPlayer.SubstractCurrency(dicesValue * totalToMultiply);
        Game.Instance.CurrentPlayer.RefreshPlayerInfo();
        Game.Instance.payerAvatar.sprite = Game.Instance.CurrentPlayer.playerAvatar.sprite;
        Game.Instance.payerName.text = Game.Instance.CurrentPlayer.playerName;

        // Pay owner
        Game.Instance.properties.playerDisplay[ownerIndex].AddCurrency(dicesValue * totalToMultiply);
        Game.Instance.properties.playerDisplay[ownerIndex].RefreshPlayerInfo();
        Game.Instance.ownerAvatar.sprite = Game.Instance.properties.playerDisplay[ownerIndex].playerAvatar.sprite;
        Game.Instance.ownerName.text = Game.Instance.properties.playerDisplay[ownerIndex].playerName;

        // Show amount
        Game.Instance.payAmount.text = "G " + dicesValue * totalToMultiply;
        Game.Instance.getPaidWindow.SetActive(true);

        // No pay anymore for service
        Game.Instance.payingService = false;
        payTenTimes = false;
    }

    public int HowMuchToPay (int currPosition, int ownerIndex)
    {
        // First check if is a special card, like transportation
        if(currPosition == 5 || currPosition == 15 || currPosition == 25 || currPosition == 35)
        {
            // How many of these we have
            int transportationPossesed = 0;
            if (Game.Instance.properties.playerDisplay[ownerIndex].propertiesOwned.Contains(5)) { transportationPossesed++; }
            if (Game.Instance.properties.playerDisplay[ownerIndex].propertiesOwned.Contains(15)) { transportationPossesed++; }
            if (Game.Instance.properties.playerDisplay[ownerIndex].propertiesOwned.Contains(25)) { transportationPossesed++; }
            if (Game.Instance.properties.playerDisplay[ownerIndex].propertiesOwned.Contains(35)) { transportationPossesed++; }

            if(payDoubleTransport)
            {
                payDoubleTransport = false;
                return Game.Instance.board.cardsPrice[currPosition].withProp[transportationPossesed - 1] * 2;
            }
            else
            {
                payDoubleTransport = false;
                return Game.Instance.board.cardsPrice[currPosition].withProp[transportationPossesed - 1];
            }
        }
        // Then check if there are houses on the property
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

    public void CheckIfIStillNeedToPay()
    {
        if(stillNeedToPay > 0 && Game.Instance.CurrentPlayer.playerCurrency < 0 && Game.Instance.CurrentPlayer.playerCurrency != stillNeedToPay)
        {
            Game.Instance.properties.playerDisplay[whoIOwe].AddCurrency(Mathf.Abs(stillNeedToPay + Game.Instance.CurrentPlayer.playerCurrency));
            stillNeedToPay -= Mathf.Abs(stillNeedToPay + Game.Instance.CurrentPlayer.playerCurrency);
            Game.Instance.properties.playerDisplay[whoIOwe].RefreshPlayerInfo();
            if (stillNeedToPay <= 0)
            {
                stillNeedToPay = 0;
                whoIOwe = 0;
            }
        }
        else if(stillNeedToPay > 0 && Game.Instance.CurrentPlayer.playerCurrency > 0)
        {
            Game.Instance.properties.playerDisplay[whoIOwe].AddCurrency(stillNeedToPay);
            Game.Instance.CurrentPlayer.SubstractCurrency(stillNeedToPay);
            Game.Instance.properties.playerDisplay[whoIOwe].RefreshPlayerInfo();
            Game.Instance.CurrentPlayer.RefreshPlayerInfo();

            stillNeedToPay = 0;
            whoIOwe = 0;;
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
            Game.Instance.parkingMoney += 200;
            CheckParkingMoney();
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

    public void PayTaxesToBank (int tax)
    {
        Game.Instance.CurrentPlayer.SubstractCurrency(tax);
        Game.Instance.parkingMoney += tax;

        // Check parking
        CheckParkingMoney();

        // Close tax window
        Game.Instance.taxWindow.SetActive(false);
    }

    public void PayTenPorcentToBank ()
    {
        // 10 porcent of total
        int total = 0;

        Debug.Log("total of money is: " + Game.Instance.CurrentPlayer.playerCurrency);

        // Add 10 porcent of total money
        total += CalculateTenPorcentOf(Game.Instance.CurrentPlayer.playerCurrency);

        // Properties total value
        int propTotal = 0;

        // Check if we have properties and get the total of value for those
        for(int p = 0; p < Game.Instance.CurrentPlayer.propertiesOwned.Count; p++)
        {
            propTotal += Game.Instance.board.cardsPrice[Game.Instance.CurrentPlayer.propertiesOwned[p]].price;
        }
        
        Debug.Log("total of props is: " + propTotal);

        // Add 10 porcent of total prop val
        total += CalculateTenPorcentOf(propTotal);

        // Houses total values
        int housesTotal = 0;

        // Check if we own houses
        for (int h = 0; h < Game.Instance.CurrentPlayer.propertiesOwned.Count; h++)
        {
            HouseManager manager = Game.Instance.board.boardCardHouses[Game.Instance.CurrentPlayer.propertiesOwned[h]];
            if (manager != null)
            {
                int houseCount = manager.currHouses;
                housesTotal += (manager.housePrice * houseCount);
            }
        }

        Debug.Log("total of houses is: " + housesTotal);

        // Add 10 porcent of total houses val
        total += CalculateTenPorcentOf(housesTotal);

        Debug.Log("10% of all is: " + total);

        // Substract from player and add to parking
        Game.Instance.CurrentPlayer.SubstractCurrency(total);
        Game.Instance.parkingMoney += total;

        // Check parking
        CheckParkingMoney();

        // Close tax window
        Game.Instance.taxWindow.SetActive(false);
    }

    public void CheckParkingMoney ()
    {
        if(Game.Instance.parkingMoney > 0)
        {
            parkingMoneyLabel.text = "G " + Game.Instance.parkingMoney.ToString();
            parkingMoneyObject.SetActive(true);
        }
        else
        {
            parkingMoneyObject.SetActive(false);
        }
    }

    private int CalculateTenPorcentOf (int totalValue)
    {
        if (totalValue > 0)
        {
            return (totalValue * 10) / 100;
        }

        return 0;
    }
}