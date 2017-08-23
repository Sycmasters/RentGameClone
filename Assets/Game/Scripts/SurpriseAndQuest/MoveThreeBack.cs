using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThreeBack : MonoBehaviour
{
    [Multiline(4)]
    public string cardDescription;

    public void PerformCard()
    {
        int location = Game.Instance.GetPlayerReference().boardLocation;
        location = location - 3 < 0 ? 40-3 : location - 3;
        Game.Instance.GetPlayerReference().boardLocation = location;
        Game.Instance.GetPlayerReference().MoveToken();
    }

    public void SetDescription(TextMesh label)
    {
        label.text = cardDescription;
    }
}
