using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToClosestUtility : MonoBehaviour
{
    [Multiline(4)]
    public string cardDescription;

    public void PerformCard ()
    {
        Game.Instance.actions.payment.payTenTimes = true;

        int nearestUtility = GetNearest(Game.Instance.GetPlayerReference().boardLocation);

        if (Game.Instance.GetPlayerReference().boardLocation > nearestUtility)
        {
            Game.Instance.actions.payment.EnableInitialPayment();
        }

        Game.Instance.GetPlayerReference().boardLocation = nearestUtility;
        Game.Instance.GetPlayerReference().MoveToken();
    }

    public void SetDescription(TextMesh label)
    {
        label.text = cardDescription;
    }

    private int GetNearest (int index)
    {
        if(index < 12)
        {
            return 12;
        }
        else if(index < 28)
        {
            return 28;
        }
        else
        {
            return 0;
        }
    }
}
