using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellAction : MonoBehaviour
{
    private Button sellButton;

	// Use this for initialization
	private void Start ()
    {
        sellButton = GetComponent<Button>();
	}
	
	public void CheckOnSellButton ()
    {
        // Do we own a property?
        bool isActive = Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex].propertiesOwned.Count > 0;
        sellButton.interactable = isActive;
    }
}
