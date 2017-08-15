using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedeemAction : MonoBehaviour
{
    private Button redeemButton;

    // Use this for initialization
    private void Start()
    {
        // Is mortgage available?
        gameObject.SetActive(GameSettings.mortgage);

        redeemButton = GetComponent<Button>();
    }

    public void CheckOnRedeemButton()
    {
        // Do we own a property?
        bool isActive = Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex].propertiesMortgaged.Count > 0;
        redeemButton.interactable = isActive;
    }
}
