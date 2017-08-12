using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public List<int> playerTurn;
    public int playerTurnIndex = 0;
    public bool turnPlayed = false;

    public RollDices diceSystem;

    public static Game Instance;

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

    private void Start ()
    {
        // Set singleton
        Instance = this;

        // Init list of turns
        playerTurn = new List<int>() { 0, 1, 2, 3, 4, 5}; // This should automatically set

        // Randomly set the order of players turn (Shuffle orders)
        for(int i = 0; i < playerTurn.Count; i++)
        {
            int temp = playerTurn[i];
            int randomIndex = Random.Range(i, playerTurn.Count);
            playerTurn[i] = playerTurn[randomIndex];
            playerTurn[randomIndex] = temp;
        }
	}

    public void MoveToken (int spaces)
    {
        GetPlayerReference().MoveToken(spaces);
    }

    public void NextTurn ()
    {
        // Allow to roll again
        diceSystem.ResetDices();

        // If is not last player turn go next
        if(playerTurnIndex < playerTurn.Count - 1)
        {
            playerTurnIndex++;
        }
        // If is the last player ending turn, repeat cycle
        else
        {
            playerTurnIndex = 0;
        }
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
