using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int moveOnly = 0; // This is for debugging only
    public static Game Instance;

    private BoardData _board;
    public BoardData board
    {
        get
        {
            if(_board == null) { _board = FindObjectOfType<BoardData>(); }
            return _board;
        }
    }

	// Use this for initialization
	void Start ()
    {
        // Set singleton
        Instance = this;
	}
}
