using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPayment : MonoBehaviour
{
    [Multiline(4)]
    public string cardDescription;
    public int payment;

    public void PerformCard()
    {
        Game.Instance.CurrentPlayer.SubstractCurrency(payment);
    }

    public void SetDescription(TextMesh label)
    {
        label.text = cardDescription;
    }
}
