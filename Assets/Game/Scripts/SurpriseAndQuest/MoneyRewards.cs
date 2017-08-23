using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyRewards : MonoBehaviour
{
    [Multiline(4)]
    public string cardDescription;
    public int reward;

    public void PerformCard()
    {
        Game.Instance.CurrentPlayer.AddCurrency(reward);
    }

    public void SetDescription(TextMesh label)
    {
        label.text = cardDescription;
    }
}
