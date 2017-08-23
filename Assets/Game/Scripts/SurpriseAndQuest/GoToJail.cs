using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToJail : MonoBehaviour
{
    [Multiline(4)]
    public string cardDescription;

    public void PerformCard ()
    {
        Game.Instance.GetPlayerReference().boardLocation = 10;
        Game.Instance.GetPlayerReference().MoveToken();

        // Tell player is in jail
        Game.Instance.GetPlayerReference().SetPlayerInJail();

        // End of the turn
        Game.Instance.nextTurnButton.SetActive(false);
        Game.Instance.dices.alreadyRolled = true;
    }

    public void SetDescription(TextMesh label)
    {
        label.text = cardDescription;
    }
}
