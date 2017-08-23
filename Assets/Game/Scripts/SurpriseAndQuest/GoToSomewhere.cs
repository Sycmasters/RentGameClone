using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSomewhere : MonoBehaviour
{
    [Multiline(4)]
    public string cardDescription;
    public int place;
    public int reward;

    public void PerformCard()
    {
        int location = Game.Instance.GetPlayerReference().boardLocation;
        if (location > place)
        {
            Game.Instance.actions.payment.EnableInitialPayment();
        }

        Game.Instance.GetPlayerReference().boardLocation = place;
        Game.Instance.GetPlayerReference().MoveToken();

        if (reward > 0)
        {
            Game.Instance.CurrentPlayer.AddCurrency(reward);
        }
    }

    public void SetDescription(TextMesh label)
    {
        label.text = cardDescription;
    }
}
