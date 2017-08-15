using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MortgageAction : MonoBehaviour
{
    private Button mortgageButton;

    // Use this for initialization
    private void Start()
    {
        // Is mortgage available?
        gameObject.SetActive(GameSettings.mortgage);

        mortgageButton = GetComponent<Button>();
    }

    public void CheckOnMortGageButton()
    {
        // Do we own a property?
        bool isActive = Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex].propertiesOwned.Count > 0;
        mortgageButton.interactable = isActive;
    }
}
