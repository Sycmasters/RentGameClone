using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInfo : MonoBehaviour
{
    public int playerIndex, playerCurrency, playerJailExitCard;
    public string playerName;
    public List<int> propertiesOwned;
    public List<int> propertiesMortgaged;

    public Text playerLabel;
    public Image playerAvatar;
    public Image playerInfoButton;

    public GameObject minicardsWindow;
    public Image[] minicards;
    public Image minicardAvatar;
    public Text minicardText;
    
    [SerializeField]
    private Button thisBtn;
    public bool gameOver;

    [ContextMenu("GetReferences")]
    public void Init()
    {
        //anim = GetComponent<Animator>();
        thisBtn = GetComponent<Button>();
        Debug.Log("Init method in " + gameObject.name);
    }

    private void Update()
    {
        if (Game.Instance.playerTurnIndex == Game.Instance.playerTurn.IndexOf(playerIndex))
        {
            Game.Instance.bankruptcyButton.SetActive(!gameOver && playerCurrency <= 0);
        }
        thisBtn.interactable = !gameOver;
    }

    public void SetPlayer (int index)
    {
        playerIndex = Game.Instance.playerTurn[index];
        playerCurrency = GameSettings.initialCurrency;
        //playerName = "Player " + (playerIndex + 1) + " \nG " + playerCurrency;
        playerName = "Player " + (playerIndex + 1);

        playerLabel.text = playerName + " \nG " + playerCurrency;
        playerAvatar.sprite = Game.Instance.playerAvatar[index];
        playerInfoButton = GetComponent<Image>();
        gameObject.SetActive(true);
	}

    public void RefreshPlayerInfo ()
    {
        playerLabel.text = playerName + " \nG " + playerCurrency;
    }

    public void ShowButton (bool show)
    {
        //anim.SetBool("Show", show);
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

    public void ShowPlayerInfo ()
    {
        if(!Game.Instance.actions.trade.trading)
        {
            // Show properties
            for (int i = 0; i < minicards.Length; i++)
            {
                // Show data
                minicardAvatar.sprite = playerAvatar.sprite;
                minicardText.text = playerName;

                // Show if available
                if (minicards[i] != null)
                {
                    if (Game.Instance.properties.playerDisplay[Game.Instance.playerTurn.IndexOf(playerIndex)].propertiesOwned.Contains(i))
                    {
                        minicards[i].color = Color.white;
                    }

                    else
                    {
                        minicards[i].color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
                    }
                }
            }

            minicardsWindow.SetActive(true);
        }
        else
        {
            if (Game.Instance.playerTurn.IndexOf(playerIndex) == Game.Instance.playerTurnIndex)
            {
                return;
            }

            // Choose for trade
            Game.Instance.actions.trade.InitDataWhenOpen(Game.Instance.playerTurn.IndexOf(playerIndex));
            Game.Instance.actions.trade.blackScreen.SetActive(false);
        }
    }
}
