using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int moveOnly = 0; // This is for debugging only
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

	void Start ()
    {
        // Set singleton
        Instance = this;
	}
}
