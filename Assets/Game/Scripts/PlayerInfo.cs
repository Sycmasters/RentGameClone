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

    public Text playerLabel;
    public Image playerAvatar;

	public void SetPlayer (int index)
    {
        playerIndex = Game.Instance.playerTurn[index];
        playerCurrency = GameSettings.initialCurrency;
        playerName = "Player " + (playerIndex + 1) + " \nG " + playerCurrency;

        playerLabel.text = playerName;
        playerAvatar.sprite = Game.Instance.playerAvatar[index];
        gameObject.SetActive(true);
	}

    public void RefreshPlayerInfo ()
    {
        playerName = "Player " + (playerIndex + 1) + " \nG " + playerCurrency;
        playerLabel.text = playerName;
    }
}
