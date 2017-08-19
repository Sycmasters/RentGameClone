using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInfo : MonoBehaviour
{
    public int playerIndex;
    public int playerCurrency;
    public string playerName;
    public List<int> propertiesOwned;
    public List<int> propertiesMortgaged;

    public Text playerLabel;
    public Image playerAvatar;
    public Image playerInfoButton;

    private Animator anim;

    [ContextMenu("GetReferences")]
    public void Init()
    {
        anim = GetComponent<Animator>();
        Debug.Log("Init method in " + gameObject.name);
    }

    public void SetPlayer (int index)
    {
        playerIndex = Game.Instance.playerTurn[index];
        playerCurrency = GameSettings.initialCurrency;
        playerName = "Player " + (playerIndex + 1) + " \nG " + playerCurrency;

        playerLabel.text = playerName;
        playerAvatar.sprite = Game.Instance.playerAvatar[index];
        playerInfoButton = GetComponent<Image>();
        gameObject.SetActive(true);
	}

    public void RefreshPlayerInfo ()
    {
        playerName = "Player " + (playerIndex + 1) + " \nG " + playerCurrency;
        playerLabel.text = playerName;
    }

    public void ShowButton (bool show)
    {
        anim.SetBool("Show", show);
    }

    public void AddCurrency (int addValue)
    {
        playerCurrency += addValue;
        RefreshPlayerInfo();
    }

    public void SubstractCurrency (int substractValue)
    {
        playerCurrency -= substractValue;
        RefreshPlayerInfo();
    }
}
