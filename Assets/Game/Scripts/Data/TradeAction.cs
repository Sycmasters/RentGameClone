using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeAction : MonoBehaviour
{
    public Image playerFromAvatar;
    public Image playerToAvatar;
    public Text playerFromName;
    public Text playerToName;

    public Image[] playerFromCards;
    public Image[] playerToCards;
    public Text playerFromMoneyDisplay;
    public Text playerToMoneyDisplay;

    public Slider playerFromSlider;
    public Slider playerToSlider;

    public List<int> fromTradeList;
    public List<int> toTradeList;

    public Button lessFromButton;
    public Button moreFromButton;
    public Button lessToButton;
    public Button moreToButton;

    public Text confirmBtn;
    public Text cancelBtn;

    public bool trading;

    public GameObject blackScreen;
    public GameObject tradeWindow;

    private int currentTrading;
    private int tradePlayerIndex;
    
    public void StartTrading ()
    {
        trading = true;
        blackScreen.SetActive(true);
    }

    public void EndTrading()
    {
        trading = false;
        blackScreen.SetActive(false);
        tradeWindow.SetActive(false);
    }

    public void InitDataWhenOpen(int tradeWith)
    {
        // With who are we trading
        tradePlayerIndex = tradeWith;
        currentTrading = Game.Instance.playerTurnIndex;

        // Open if needed 
        if(!tradeWindow.activeInHierarchy)
        {
            tradeWindow.SetActive(true);
        }

        // Clean data for trade
        playerFromSlider.gameObject.SetActive(true);
        playerToSlider.gameObject.SetActive(true);

        lessFromButton.gameObject.SetActive(true);
        moreFromButton.gameObject.SetActive(true);
        lessToButton.gameObject.SetActive(true);
        moreToButton.gameObject.SetActive(true);

        playerFromSlider.value = 0;
        playerToSlider.value = 0;

        confirmBtn.text = " Intercambiar ";
        cancelBtn.text = "           Cancelar           ";

        fromTradeList = new List<int>();
        toTradeList = new List<int>();

        // Clean cards and show which it actually have
        for(int i = 0; i < playerFromCards.Length; i++)
        {
            // Show if available
            if (playerFromCards[i] != null)
            {
                if (Game.Instance.CurrentPlayer.propertiesOwned.Contains(i))
                {
                    playerFromCards[i].color = Color.white;
                }

                else
                {
                    playerFromCards[i].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }

                playerFromCards[i].transform.Find("Arrow").GetComponent<Text>().text = "";

                // Set function
                {
                    int index = i;
                    Button fromBtn = playerFromCards[index].GetComponent<Button>();

                    fromBtn.onClick.RemoveAllListeners();

                    fromBtn.onClick.AddListener(() =>
                    {
                        SetCardIntoList(playerFromCards[index].name, playerFromCards[index].transform);
                    });
                    fromBtn.interactable = true;
                }
            }

            // TO
            if (playerToCards[i] != null)
            {
                if (Game.Instance.properties.playerDisplay[tradeWith].propertiesOwned.Contains(i))
                {
                    playerToCards[i].color = Color.white;
                }
                else
                {
                    playerToCards[i].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }

                playerToCards[i].transform.Find("Arrow").GetComponent<Text>().text = "";

                // Set function
                {
                    int index = i;
                    Button toBtn = playerToCards[index].GetComponent<Button>();

                    toBtn.onClick.RemoveAllListeners();

                    toBtn.onClick.AddListener(() =>
                    {
                        SetCardIntoList(playerToCards[index].name, playerToCards[index].transform);
                    });
                    toBtn.interactable = true;
                }
            }
        }

        // Show traders name
        playerFromName.text = Game.Instance.CurrentPlayer.playerName;
        playerToName.text = Game.Instance.properties.playerDisplay[tradePlayerIndex].playerName;

        // Show traders sprite
        playerFromAvatar.sprite = Game.Instance.CurrentPlayer.playerAvatar.sprite;
        playerToAvatar.sprite = Game.Instance.properties.playerDisplay[tradePlayerIndex].playerAvatar.sprite;
        
        // Set max amount of money
        playerFromSlider.maxValue = Game.Instance.CurrentPlayer.playerCurrency;
        playerToSlider.maxValue = Game.Instance.properties.playerDisplay[tradePlayerIndex].playerCurrency;
    }

    public void SetCardIntoList (string cardName, Transform btn)
    {
        int cardIndex = int.Parse(cardName);

        // Function comes from "from"
        if (btn.parent.parent.name.Contains("From"))
        {
            // If is in, take it out
            if(fromTradeList.Contains(cardIndex))
            {
                fromTradeList.Remove(cardIndex);
                btn.Find("Arrow").GetComponent<Text>().text = "";
            }
            // If not put it in
            else
            {
                fromTradeList.Add(cardIndex);
                btn.Find("Arrow").GetComponent<Text>().text = "->";
            }
        }
        // Function comes from "to"
        else if (btn.parent.parent.name.Contains("To"))
        {
            // If is in, take it out
            if (toTradeList.Contains(cardIndex))
            {
                toTradeList.Remove(cardIndex);
                btn.Find("Arrow").GetComponent<Text>().text = "";
            }
            // If not put it in
            else
            {
                toTradeList.Add(cardIndex);
                btn.Find("Arrow").GetComponent<Text>().text = "<-";
            }
        }
    }

    public void CopyValueFromTo (bool from)
    {
        if(from)
        {
            playerFromMoneyDisplay.text = "G " + playerFromSlider.value;
        }
        else
        {
            playerToMoneyDisplay.text = "G " + playerToSlider.value;
        }
    }
    
    public void IncreaseDecreaseValFrom(int val)
    {
        playerFromSlider.value += val;
        playerFromMoneyDisplay.text = "G " + playerFromSlider.value;
    }

    public void IncreaseDecreaseValTo(int val)
    {
        playerToSlider.value += val;
        playerToMoneyDisplay.text = "G " + playerToSlider.value;
    }

    public void OfferButton ()
    {
        if (currentTrading == Game.Instance.playerTurnIndex)
        {
            // Sender change to receiver
            currentTrading = tradePlayerIndex;

            //Deactivate buttons
            for (int i = 0; i < playerFromCards.Length; i++)
            {
                if (playerFromCards[i] != null)
                {
                    playerFromCards[i].color = Color.white;
                    playerFromCards[i].GetComponent<Button>().interactable = false;
                }
                if (playerToCards[i] != null)
                {
                    playerToCards[i].color = Color.white;
                    playerToCards[i].GetComponent<Button>().interactable = false;
                }
            }

            playerFromSlider.gameObject.SetActive(false);
            playerToSlider.gameObject.SetActive(false);

            moreFromButton.gameObject.SetActive(false);
            lessFromButton.gameObject.SetActive(false);
            moreToButton.gameObject.SetActive(false);
            lessToButton.gameObject.SetActive(false);

            // Change buttons label
            confirmBtn.text = "Confirmar";
            cancelBtn.text = "Rechazar";
        }
        else
        {
            // Accept trade
            
            // Give my props to him
            for(int fi = 0; fi < fromTradeList.Count; fi++)
            {
                // Give these to the other player
                Game.Instance.properties.playerDisplay[tradePlayerIndex].propertiesOwned.Add(fromTradeList[fi]);

                // Release mine
                Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex].propertiesOwned.Remove(fromTradeList[fi]);

                // Change ownershipt
                Game.Instance.board.boardOwnershipRender[fromTradeList[fi]].sprite = Game.Instance.properties.playerDisplay[tradePlayerIndex].playerAvatar.sprite;
            }

            // Get his props
            for (int ti = 0; ti < toTradeList.Count; ti++)
            {
                // Get these from the other player
                Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex].propertiesOwned.Add(toTradeList[ti]);

                // Release his
                Game.Instance.properties.playerDisplay[tradePlayerIndex].propertiesOwned.Remove(toTradeList[ti]);

                // Change ownershipt
                Game.Instance.board.boardOwnershipRender[toTradeList[ti]].sprite = Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex].playerAvatar.sprite;
            }

            // Exchange money
            Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex].SubstractCurrency((int)playerFromSlider.value);
            Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex].AddCurrency((int)playerToSlider.value);

            Game.Instance.properties.playerDisplay[tradePlayerIndex].SubstractCurrency((int)playerToSlider.value);
            Game.Instance.properties.playerDisplay[tradePlayerIndex].AddCurrency((int)playerFromSlider.value);

            Game.Instance.actions.payment.CheckIfIStillNeedToPay();

            // Set windows false
            tradeWindow.SetActive(false);
        }
    }

    public void CancelOffer ()
    {
        if (currentTrading == Game.Instance.playerTurnIndex)
        {
            EndTrading();
        }
        else
        {
            EndTrading();

            // Show window for other player on online
        }
        Game.Instance.actions.payment.CheckIfIStillNeedToPay();
    }
}