using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public BoardBehaviour board;
    public TokenController[] tokens;
    public int currentPlayer;
    public int spaceToMove = 5;

    // Start is called before the first frame update
    private void Start ()
    {
        for(int i = 0; i < tokens.Length; i++)
        {
            tokens[i].tokenIndex = i;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            tokens[currentPlayer].MoveTo(board.GetSpaces(spaceToMove, tokens[currentPlayer]), spaceToMove);
            if(currentPlayer < tokens.Length - 1)
            {
                currentPlayer++;
            }
            else
            {
                currentPlayer = 0;
            }
        }
    }
}
