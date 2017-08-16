using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertiesManager : MonoBehaviour
{
    public List<PlayerInfo> playerDisplay;

    public GameObject sellingWindow;
    public Text cardPrice;
    public Image cardImage;
    public Button cardBuyButton;

    public List<int> availableCards = new List<int>();
    private int cardToBuy;


    public void OpenSellWindow (int index)
    {
        // what card are we buying?
        cardToBuy = index;

        cardPrice.text = "G " + Game.Instance.board.cardsPrice[index].price;
        cardImage.sprite = Game.Instance.board.cardsSprite[index];

        // Do we have enough money to buy it?
        cardBuyButton.interactable = playerDisplay[Game.Instance.playerTurnIndex].playerCurrency >= Game.Instance.board.cardsPrice[index].price;
        
        sellingWindow.SetActive(true);
    }

    public void BuyProperty ()
    {
        // Do we have enough money to buy it?
        if(playerDisplay[Game.Instance.playerTurnIndex].playerCurrency >= Game.Instance.board.cardsPrice[cardToBuy].price)
        {
            // Get paid
            playerDisplay[Game.Instance.playerTurnIndex].playerCurrency -= Game.Instance.board.cardsPrice[cardToBuy].price;

            // Remove it from available cards
            int cardIndex = availableCards.IndexOf(cardToBuy);
            availableCards.RemoveAt(cardIndex);

            // Give it to the player and refresh info
            playerDisplay[Game.Instance.playerTurnIndex].propertiesOwned.Add(cardToBuy);
            playerDisplay[Game.Instance.playerTurnIndex].RefreshPlayerInfo();

            // Show whose owner
            SpriteRenderer ownerAvatar = Game.Instance.board.boardPositions[cardToBuy].GetComponentInChildren<SpriteRenderer>();
            ownerAvatar.sprite = playerDisplay[Game.Instance.playerTurnIndex].playerAvatar.sprite;
            ownerAvatar.enabled = true;

            // Hide selling window
            sellingWindow.SetActive(false);

            // Check if we can sell now
            Game.Instance.actions.sell.CheckOnSellButton();

            // Flush data
            cardToBuy = 0;
        }
    }
}
