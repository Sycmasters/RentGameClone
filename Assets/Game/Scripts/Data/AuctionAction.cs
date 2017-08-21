using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionAction : MonoBehaviour
{
    public GameObject auctionWindow;
    public Slider moneySlider;
    public Text playerDisplay;
    public Text moneyDisplay;
    public Text betsDisplay;
    public Image currProperty;
    public Button betBtn;
    public Button surrenderBtn;
    public Button closeBtn;

    public int currPlayerBetting;

    public int highestBet;
    public int highestBetPlayer;

    private List<int> surrenderPlayers = new List<int>();

    public void InitDataWhenOpen ()
    {
        // Open window if needed
        if(!auctionWindow.activeInHierarchy)
        {
            auctionWindow.SetActive(true);
        }

        // Clean data for new auction
        highestBet = 0;
        highestBetPlayer = 0;
        betsDisplay.text = "";
        surrenderPlayers = new List<int>();

        // Check who's betting
        currPlayerBetting = Game.Instance.playerTurnIndex;

        // Show what property are we aucting
        currProperty.sprite = Game.Instance.board.cardsSprite[Game.Instance.properties.cardToBuy];

        // Show auction data
        RefreshWindowInfo();
    }

    public void PlaceABet ()
    {
        // Set this to the highest bet
        highestBet = (int)moneySlider.value;

        //  This player is the made the highest bet
        highestBetPlayer = currPlayerBetting;

        // Temp string to store former bets
        string formerBets = betsDisplay.text;

        // Set new bet at the top and then the others // Player 1 - G 700
        betsDisplay.text = Game.Instance.properties.playerDisplay[highestBetPlayer].playerName + " - G " + highestBet + "\n";
        betsDisplay.text += formerBets;

        // Go next player to bet, skip player who surrendered
        currPlayerBetting = NextPlayerToBet(currPlayerBetting);

        // Refresh with new data  
        RefreshWindowInfo();
    }

    public void Surrender ()
    {
        // First check if auction can go on or all other players surrendered
        if(surrenderPlayers.Count >= Game.Instance.playerTurn.Count - 2)
        {
            // All players surrendered
            surrenderPlayers.Add(currPlayerBetting);

            // Temp string to store former bets
            string formerBets = betsDisplay.text;

            // Tell others I surrendered // Player 1 - Se rindio
            betsDisplay.text = Game.Instance.properties.playerDisplay[currPlayerBetting].playerName + " - Se rindio \n";
            betsDisplay.text += formerBets;

            formerBets = betsDisplay.text;

            // Tell who won
            betsDisplay.text = Game.Instance.properties.playerDisplay[highestBetPlayer].playerName + " - Gano \n";
            betsDisplay.text += formerBets;


            // Buttons not interactable
            betBtn.interactable = false;
            surrenderBtn.interactable = false;

            // Give winner the property

            // Get paid
            int cardIndex = Game.Instance.properties.cardToBuy;
            Game.Instance.properties.playerDisplay[highestBetPlayer].SubstractCurrency(highestBet);

            // Remove it from available cards
            int cardIndexInList = Game.Instance.properties.availableCards.IndexOf(cardIndex);
            Game.Instance.properties.availableCards.RemoveAt(cardIndexInList);

            // Give it to the player and refresh info
            Game.Instance.properties.playerDisplay[highestBetPlayer].propertiesOwned.Add(cardIndex);
            Game.Instance.properties.playerDisplay[highestBetPlayer].RefreshPlayerInfo();

            // Show whose owner
            SpriteRenderer ownerAvatar = Game.Instance.board.boardOwnershipRender[cardIndex];
            ownerAvatar.sprite = Game.Instance.properties.playerDisplay[highestBetPlayer].playerAvatar.sprite;
            ownerAvatar.enabled = true;

            // Flush data
            Game.Instance.properties.cardToBuy = 0;

            // We can close now
            closeBtn.interactable = true;
        }
        else
        {
            // I surrender but auction can go on
            surrenderPlayers.Add(currPlayerBetting);

            // Temp string to store former bets
            string formerBets = betsDisplay.text;

            // Tell others I surrendered // Player 1 - Se rindio
            betsDisplay.text = Game.Instance.properties.playerDisplay[currPlayerBetting].playerName + " - Se rindio \n";
            betsDisplay.text += formerBets;

            // Go next player to bet, skip player who surrendered
            currPlayerBetting = NextPlayerToBet(currPlayerBetting);

            // Refresh with new data  
            RefreshWindowInfo();
        }
    }

    public void UpdateMoneyDisplay (Slider moneyHandler)
    {
        moneyDisplay.text = "G " + moneyHandler.value;
    }

    private void RefreshWindowInfo ()
    {
        // Who is betting right now?
        playerDisplay.text = Game.Instance.properties.playerDisplay[currPlayerBetting].playerName + " apostando";

        // We cannot bet more than we have
        moneySlider.maxValue = Game.Instance.properties.playerDisplay[currPlayerBetting].playerCurrency;

        // Can we actually bet? Or highest bet is more than we can?
        betBtn.interactable = highestBet < moneySlider.maxValue;

        // Set minimun slider value to minimun value posible
        moneySlider.minValue = highestBet > moneySlider.maxValue ? moneySlider.maxValue : highestBet + 1;
        moneySlider.value = moneySlider.minValue;

        // Display Value
        moneyDisplay.text = "G " + moneySlider.value;
    }

    private int NextPlayerToBet (int currentIndex)
    {
        int returnValue = currentIndex;

        // This is not the last player
        if (currentIndex < Game.Instance.playerTurn.Count - 1)
        {
            currPlayerBetting++;
        }
        // Repeat the cycle with the first one
        else
        {
            currPlayerBetting = 0; 
        }

        // This player surrendered or is the highest bet
        if(surrenderPlayers.Contains(currPlayerBetting) || currPlayerBetting == highestBetPlayer)
        {
            returnValue = NextPlayerToBet(currPlayerBetting);
        }
        // This player hasn't surrender
        else if (!surrenderPlayers.Contains(currPlayerBetting))
        {
            returnValue = currPlayerBetting;
        }

        return returnValue;
    }

    public void IncreaseDecreaseVal (int val)
    {
        moneySlider.value += val;
        moneyDisplay.text = "G " + moneySlider.value;
    }
}
