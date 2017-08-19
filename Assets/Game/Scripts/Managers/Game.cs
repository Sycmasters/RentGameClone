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
    public bool turnPlayed = false;

    [Header("---------------")]
    public RollDices diceSystem;

    [Header("---------------")]
    public GameObject getPaidWindow;
    public Image payerAvatar;
    public Image ownerAvatar;
    public Text payerName;
    public Text ownerName;
    public Text payAmount;
    public GameObject nextTurnButton;

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

    public void MoveToken (int spaces)
    {
        GetPlayerReference().MoveToken(spaces);
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

        // If is not last player turn go next
        if(playerTurnIndex < playerTurn.Count - 1)
        {
            // Unfocus last player 
            CurrentPlayer.playerInfoButton.color = Color.white;

            // Go next player
            playerTurnIndex++;

            // Focus next player
            CurrentPlayer.playerInfoButton.color = Color.green;
        }
        // If is the last player ending turn, repeat cycle
        else
        {
            // Unfocus last player 
            CurrentPlayer.playerInfoButton.color = Color.white;

            playerTurnIndex = 0;

            // Focus next player
            CurrentPlayer.playerInfoButton.color = Color.green;
        }

        // Disable initial payment in case it was active
        actions.payment.DisableInitialPayment();
    }

    public void TokenReachedDestination(int currPosition)
    {
        // If player reached a buyable property
        if(board.cardsSprite[currPosition] != null && properties.availableCards.Contains(currPosition))
        {
            properties.OpenSellWindow(currPosition);
        }
        // This card is possesed by someone 
        else if(board.cardsSprite[currPosition] != null && !properties.availableCards.Contains(currPosition))
        {
            int ownerIndex = WhoOwnsThisCard(currPosition);
            if (!CurrentPlayer.propertiesOwned.Contains(currPosition))
            {
                actions.payment.PayRent(currPosition, ownerIndex);
            }
        }

        // We can end the turn now
        nextTurnButton.SetActive(true);
    }

    private int WhoOwnsThisCard (int cardIndex)
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

    private TokenController GetPlayerReference ()
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
