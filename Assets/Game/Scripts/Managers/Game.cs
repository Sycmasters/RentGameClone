using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [Header("---------------")]
    public List<int> playerTurn;
    public List<Sprite> playerAvatar;

    [Header("---------------")]
    public int playerTurnIndex = 0;
    public int parkingMoney;
    public bool turnPlayed = false, payingService = false;

    [Header("---------------")]
    public RollDices diceSystem;
    public SurpriseQuestCards surprise;
    public SurpriseQuestCards quest;

    [Header("---------------")]
    public GameObject getPaidWindow;
	public GameObject jailWindow;
    public GameObject taxWindow;
    public Image payerAvatar;
    public Image ownerAvatar;
    public Text payerName;
    public Text ownerName;
    public Text payAmount;
    public GameObject nextTurnButton;
    public GameObject bankruptcyButton;

    [Header("---------------")]
    public GameObject gameOverWindow;
    public Text winnerName;
    public Image winnerAvatar;

    public static Game Instance;

    [Header("---------------")]
    [SerializeField]
    private int testMoveSpaces;
    [SerializeField]
    private bool dontHideTurnBtn;


    // BOARD 
    private BoardData _board;
    public BoardData board
    {
        get
        {
            if(_board == null) { _board = FindObjectOfType<BoardData>(); }
            return _board;
        }
    }

    // DICE SYSTEM
    private RollDices _dices;
    public RollDices dices
    {
        get
        {
            if (_dices == null) { _dices = FindObjectOfType<RollDices>(); }
            return _dices;
        }
    }

    // PROPERTY SYSTEM
    private PropertiesManager _properties;
    public PropertiesManager properties
    {
        get
        {
            if (_properties == null) { _properties = FindObjectOfType<PropertiesManager>(); }
            return _properties;
        }
    }

    // PLAYERS REFERENCE
    private TokenController[] _players;
    public TokenController[] players
    {
        get
        {
            if (_players == null)
            {
                GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");

                _players = new TokenController[playersObj.Length];

                for(int i = 0; i < playersObj.Length; i++)
                {
                    //Debug.Log(playersObj[i].name);
                    _players[i] = playersObj[i].GetComponent<TokenController>();
                }
            }
            return _players;
        }
    }

    // ACTIONS SYSTEM
    private PlayerActions _actions;
    public PlayerActions actions
    {
        get
        {
            if (_actions == null) { _actions = FindObjectOfType<PlayerActions>(); }
            return _actions;
        }
    }

    // CURRENT PLAYER
    public PlayerInfo CurrentPlayer
    {
        get
        {
            return properties.playerDisplay[playerTurnIndex]; 
        }
    }

    private void Awake()
    {
        // Set singleton
        Instance = this;
    }

    private void Start ()
    {
        // Init list of turns
        //playerTurn = new List<int>() { 0, 1, 2, 3, 4}; // This should automatically set
        // Randomly set the order of players turn (Shuffle orders)
        for(int i = 0; i < playerTurn.Count; i++)
        {
            // Turns
            int temp = playerTurn[i];
            int randomIndex = Random.Range(i, playerTurn.Count);
            playerTurn[i] = playerTurn[randomIndex];
            playerTurn[randomIndex] = temp;

            // Sprites
            Sprite tempSprite = playerAvatar[i];
            playerAvatar[i] = playerAvatar[randomIndex];
            playerAvatar[randomIndex] = tempSprite;
        }

        // Turn on player info display 
        for(int i = 0; i < playerTurn.Count; i++)
        {
            properties.playerDisplay[i].SetPlayer(i);
        }

        // This is for the focus effect on buttons
        // We need to start in the last to focus the first
        //Debug.Log(playerTurn.Count - 1);
        playerTurnIndex = playerTurn.Count - 1;
        // Now set next turn to start with the first player (player 0)
        NextTurn();

        // Init actions
        actions.sell.CheckOnSellButton();
        actions.build.CheckOnBuildButton();

        // Hide end turn button
        if (!dontHideTurnBtn)
        {
            nextTurnButton.SetActive(false);
        }
    }

    private void Update()
    {
        // Just for testing -- Delete after production
        if(Input.GetKeyDown(KeyCode.L))
        {
            MoveToken(testMoveSpaces);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            NextTurn();
        }
    }

    public void MoveToken(int spaces)
    {
        GetPlayerReference().MoveToken(spaces);
    }
    
    public void Bankruptcy ()
    {
        CurrentPlayer.gameOver = true;
        GetPlayerReference().gameObject.SetActive(false);
        bankruptcyButton.SetActive(false);
        CheckOnWinner();
        NextTurn();
    }

    public void NextTurn()
    {
        // Hide end turn button
        if (!dontHideTurnBtn)
        {
            nextTurnButton.SetActive(false);
        }

        // Allow to roll again
        diceSystem.ResetDices();

        // If selling or building, stop it
        if (actions.sell.selling) { actions.sell.EndSelling(); }
        if (actions.build.building) { actions.build.EndBuilding(); }
        if (actions.trade.trading) { actions.trade.EndTrading(); }

        // If is not last player turn go next
        if (playerTurnIndex < playerTurn.Count-1)
        {
            // Unfocus last player 
            CurrentPlayer.playerInfoButton.color = Color.white;

            // Go next player
            playerTurnIndex++;

            // Continue with next it this is not participating anymore
            if (!CurrentPlayer.gameOver)
            {
                // Focus next player
                if (!GetPlayerReference().inJail)
                {
                    CurrentPlayer.playerInfoButton.color = Color.green;
                }
                else
                {
                    CurrentPlayer.playerInfoButton.color = Color.red;
                    jailWindow.SetActive(true);
                    dices.useCardBtn.interactable = CurrentPlayer.playerJailExitCard > 0;
                }
            }
            else
            {
                NextTurn();
            }
        }
        // If is the last player ending turn, repeat cycle
        else
        {
            // Unfocus last player 
            CurrentPlayer.playerInfoButton.color = Color.white;

            playerTurnIndex = 0;

            // Continue with next it this is not participating anymore
            if (!CurrentPlayer.gameOver)
            {
                // Focus next player
                if (!GetPlayerReference().inJail)
                {
                    CurrentPlayer.playerInfoButton.color = Color.green;
                }
                else
                {
                    CurrentPlayer.playerInfoButton.color = Color.red;
                    jailWindow.SetActive(true);
                    dices.useCardBtn.interactable = CurrentPlayer.playerJailExitCard > 0;
                }
            }
            else
            {
                NextTurn();
            }
        }

        // Disable initial payment in case it was active
        actions.payment.DisableInitialPayment();
    }

    private void CheckOnWinner ()
    {
        int playerLeft = 0;
        int winner = 0;

        for(int i = 0; i < playerTurn.Count; i++)
        {
            if (properties.playerDisplay[i].gameOver)
            {
                playerLeft++;
            }
            else
            {
                winner = i;
            }
        }

        if (playerLeft >= Game.Instance.playerTurn.Count - 1)
        {
            winnerName.text = properties.playerDisplay[winner].playerName;
            winnerAvatar.sprite = properties.playerDisplay[winner].playerAvatar.sprite;
            gameOverWindow.SetActive(true);
        }
    }

    public void QuitButton ()
    {
        Application.Quit();
    }

    public void TokenReachedDestination(int currPosition)
    {
        // If player reached a buyable property
        if(board.cardsSprite[currPosition] != null && properties.availableCards.Contains(currPosition))
        {
            properties.OpenSellWindow(currPosition);
            // Clean any undesired data just in case
            actions.payment.payDoubleTransport = false;
            actions.payment.payTenTimes = false;
        }
        // This card is possesed by someone 
        else if(board.cardsSprite[currPosition] != null && !properties.availableCards.Contains(currPosition))
        {
            int ownerIndex = WhoOwnsThisCard(currPosition);
            if (!CurrentPlayer.propertiesOwned.Contains(currPosition))
            {
                // Need to pay for services so we need to roll dices first
                if (currPosition == 12 || currPosition == 28)
                {
                    payingService = true;
                    dices.RollBothDices();
                }
                // We are paying any other kind of card
                else
                {
                    actions.payment.PayRent(currPosition, ownerIndex);
                }
            }
        }
        // If we reached at go to jail
        else if(currPosition == 30)
        {
            // Force player to go to Jail
            GetPlayerReference().boardLocation = 10; // 10 = Jail
            GetPlayerReference().MoveToken();

            // Tell player is in jail
            GetPlayerReference().SetPlayerInJail();
        }
        // Pay taxes
        else if(currPosition == 4 || currPosition == 38)
        {
            actions.payment.CheckParkingMoney();
            
            // For first tax choose payment method
            if(currPosition == 4)
            {
                taxWindow.SetActive(true);
            }
            else
            {
                actions.payment.PayTaxesToBank(75);
            }
        }
        // Win parking money
        else if(currPosition == 20)
        {
            CurrentPlayer.AddCurrency(parkingMoney);
            parkingMoney = 0;
            actions.payment.CheckParkingMoney();
        }
        else if(currPosition == 2 || currPosition == 17 || currPosition == 33)
        {
            // Surprise!
            surprise.ShowCard();
        }
        else if (currPosition == 7 || currPosition == 22 || currPosition == 36)
        {
            // Quest!
            quest.ShowCard();
        }

        // We can end the turn now
        if (dices.alreadyRolled)
        {
            nextTurnButton.SetActive(true);
        }
    }

    public void FreeFromJailWithCard ()
    {
        CurrentPlayer.playerJailExitCard--;
        dices.useCardBtn.interactable = CurrentPlayer.playerJailExitCard > 0;
        GetPlayerReference().FreePlayerFromJail();
        jailWindow.SetActive(false);
    }

    public int WhoOwnsThisCard (int cardIndex)
    {
        for(int i = 0; i < properties.playerDisplay.Count; i++)
        {
            // This player has the card
            if(properties.playerDisplay[i].propertiesOwned.Contains(cardIndex))
            {
                // Return player index
                return i;
            }
        }

        return -1;
    }

    public TokenController GetPlayerReference ()
    {
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].playerIndex == playerTurn[playerTurnIndex])
            {
                return players[i];
            }
        }

        return null;
    }
}
