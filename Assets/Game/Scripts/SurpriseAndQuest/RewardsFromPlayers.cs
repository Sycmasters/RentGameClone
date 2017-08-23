using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsFromPlayers : MonoBehaviour
{
    [Multiline(4)]
    public string cardDescription;
    public int reward;

    public void PerformCard()
    {
        for(int i = 0; i < Game.Instance.playerTurn.Count; i++)
        {
            if(i == Game.Instance.playerTurnIndex)
            {
                continue;
            }
            Game.Instance.properties.playerDisplay[i].SubstractCurrency(reward);
            Game.Instance.CurrentPlayer.AddCurrency(reward);
        }
    }

    public void SetDescription(TextMesh label)
    {
        label.text = cardDescription;
    }
}
