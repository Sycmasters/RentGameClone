using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayer : MonoBehaviour
{
    public Image render, cardFrame;
    public Text rentInfo, cardName;
    public GameObject showSellOption;
    public bool isShowingInfo;

    public void ShowCardInfo (Sprite sprite, SpriteRenderer frameRender, int[] prices, string name, string house, string hotel, string transport, string service, string currency, bool justShowing)
    {
        // Display the sprite and get the color 
        render.sprite = sprite;
        cardFrame.color = frameRender.color;

        cardName.text = name != "" ? name : "Tarjeta";

        // Show rent prices text
        if(prices.Length <= 2)
        {
            ServiceCards(prices, service, currency);
        }
        else if(prices.Length <= 4)
        {
            TransportCards(prices, transport, currency);
        }
        else
        {
            PropertyCards(prices, currency, house, hotel);
        }

        isShowingInfo = true;

        if(!justShowing)
        {
            showSellOption.SetActive(true);
        }

        gameObject.SetActive(true);
    }

    public void CloseWindow ()
    {
        isShowingInfo = false;
        showSellOption.SetActive(false);
        gameObject.SetActive(false);
    }

    private void PropertyCards (int[] prices, string currency, string house, string hotel)
    {
        rentInfo.text = "Estadía: " + prices[0] + " " + currency + "\n";
        rentInfo.text += "1 " + house + ": " + prices[1] + " " + currency + "\n";
        rentInfo.text += "2 " + house + "s: " + prices[2] + " " + currency + "\n";
        rentInfo.text += "3 " + house + "s: " + prices[3] + " " + currency + "\n";
        rentInfo.text += "4 " + house + "s: " + prices[4] + " " + currency + "\n";
        rentInfo.text += hotel + ": " + prices[5] + " " + currency;
    }

    private void TransportCards (int[] prices, string transport, string currency)
    {
        rentInfo.text = "1 " + transport + ": " + prices[0] + " " + currency + "\n";
        rentInfo.text += "2 " + transport + "s: " + prices[1] + " " + currency + "\n";
        rentInfo.text += "3 " + transport + "s: " + prices[2] + " " + currency + "\n";
        rentInfo.text += "4 " + transport + "s: " + prices[3] + " " + currency;
    }

    private void ServiceCards (int[] prices, string service, string currency)
    {
        rentInfo.text = "1 " + service + ": Valor de los dados x" + prices[0] + "\n";
        rentInfo.text += "2 " + service + "s: Valor de los dados x" + prices[1];
    }
}
