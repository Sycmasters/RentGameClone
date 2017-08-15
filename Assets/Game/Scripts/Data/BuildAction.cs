using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildAction : MonoBehaviour
{    
    private Button buildButton;

    // Use this for initialization
    private void Start()
    {
        buildButton = GetComponent<Button>();
    }

    public void CheckOnBuildButton()
    {
        // Do we own a property?
        bool isActive = Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex].propertiesOwned.Count > 0;
        buildButton.interactable = isActive;
    }
}
