using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class DiceManager : MonoBehaviour
{
    public Dice dice1, dice2;
    public int dicesValue;
    public float checkRatio = 1;

    private bool canRollAgain;

    private void Start ()
    {
        this.tt("@CheckOnDices").Add(checkRatio, () =>
        {
            if(!canRollAgain)
            {
                if(dice1.shownNumber > 0)
                {
                    dicesValue += dice1.shownNumber;
                    dice1.ResetDice(false);
                }
                if(dice2.shownNumber > 0)
                {
                    dicesValue += dice2.shownNumber;
                    dice2.ResetDice(false);
                }
                
                if(dice1.CanBeRolled() && dice2.CanBeRolled())
                {
                    canRollAgain = true;
                }
            }
        }).Repeat();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && canRollAgain)
        {
            dicesValue = 0;
            RollDices();
            canRollAgain = false;
        }
    }

    public void RollDices ()
    {
        dice1.RollDice();
        dice2.RollDice();
    }
}
