using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToClosestTransport : MonoBehaviour
{
    [Multiline(4)]
    public string cardDescription;

    public void PerformCard ()
    {
        // Make it double
        Game.Instance.actions.payment.payDoubleTransport = true;

        int nearestTransport = GetNearest(Game.Instance.GetPlayerReference().boardLocation);

        if (Game.Instance.GetPlayerReference().boardLocation > nearestTransport)
        {
            Game.Instance.actions.payment.EnableInitialPayment();
        }

        Game.Instance.GetPlayerReference().boardLocation = nearestTransport;
        Game.Instance.GetPlayerReference().MoveToken();
    }

    public void SetDescription(TextMesh label)
    {
        label.text = cardDescription;
    }

    private int GetNearest (int index)
    {
        if(index < 5)
        {
            return 5;
        }
        else if(index < 15)
        {
            return 15;
        }
        else if(index < 25)
        {
            return 25;
        }
        else if(index < 35)
        {
            return 35;
        }
        else
        {
            return 5;
        }
    }
}
