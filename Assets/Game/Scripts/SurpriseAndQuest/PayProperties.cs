using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayProperties : MonoBehaviour
{
    [Multiline(4)]
    public string cardDescription;
    public int housePrice;
    public int hotelPrice;

    public void PerformCard()
    {
        // Houses total values
        int housesTotal = 0;

        // Check if we own houses
        for (int h = 0; h < Game.Instance.CurrentPlayer.propertiesOwned.Count; h++)
        {
            HouseManager manager = Game.Instance.board.boardCardHouses[Game.Instance.CurrentPlayer.propertiesOwned[h]];
            if (manager != null)
            {
                int houseCount = manager.currHouses;
                if (houseCount >= 5)
                {
                    housesTotal += hotelPrice;
                }
                else
                {
                    housesTotal += (housePrice * houseCount);
                }
            }
        }

        // Substract from player and add to parking
        Game.Instance.CurrentPlayer.SubstractCurrency(housesTotal);
        Game.Instance.parkingMoney += housesTotal;

        // Check parking
        Game.Instance.actions.payment.CheckParkingMoney();
    }

    public void SetDescription(TextMesh label)
    {
        label.text = cardDescription;
    }
}
