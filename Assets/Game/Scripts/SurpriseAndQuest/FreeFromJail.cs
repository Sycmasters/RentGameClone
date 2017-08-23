using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFromJail : MonoBehaviour
{
    [Multiline(4)]
    public string cardDescription;

    public void PerformCard()
    {
        Game.Instance.CurrentPlayer.playerJailExitCard++;
    }

    public void SetDescription(TextMesh label)
    {
        label.text = cardDescription;
    }
}
