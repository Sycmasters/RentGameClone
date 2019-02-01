using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class GameLoopManager : MonoBehaviour
{
    public BoardBehaviour board;
    public TokenController[] tokens;
    public DiceManager dices;
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
            PlayTurn();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            dices.RollDices();
            this.tt("@GetValueFromDices").Reset()
            .Wait(() => dices.CanRollAgain() && dices.dicesValue != 0)
            .Add(() => 
            {
                spaceToMove += dices.dicesValue;
                dices.ResetDices();

                if(spaceToMove > 39)
                {
                    spaceToMove -= 40;
                }
                
                PlayTurn();
            }).Immutable();
        }
    }

    private void PlayTurn ()
    {
        tokens[currentPlayer].MoveTo(board.GetSpaces(spaceToMove, tokens[currentPlayer]), spaceToMove);
        if(currentPlayer < tokens.Length - 1)
        {
            currentPlayer++;
            spaceToMove = tokens[currentPlayer].currentTargetIndex;
        }
        else
        {
            currentPlayer = 0;
            spaceToMove = tokens[currentPlayer].currentTargetIndex;            
        }
    }
}
