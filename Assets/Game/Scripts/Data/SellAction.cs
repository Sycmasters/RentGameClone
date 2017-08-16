using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript;

public class SellAction : MonoBehaviour
{
    public GameObject sellWindow;
    public Button sellButton;
    public bool selling;

	// Use this for initialization
	private void Start ()
    {
        sellButton = GetComponent<Button>();
	}

    private void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed += OnTouch;
        }
    }

    private void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed -= OnTouch;
        }
    }

    public void CheckOnSellButton ()
    {
        // Do we own a property?
        bool isActive = Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex].propertiesOwned.Count > 0;
        if(sellButton == null)
        {
            sellButton = GetComponent<Button>();
        }
        sellButton.interactable = isActive;
    }

    public void StartSelling ()
    {
        sellWindow.SetActive(true);
        selling = true;
    }

    public void EndSelling ()
    {
        selling = false;
        sellWindow.SetActive(false);
    }

    private void OnTouch (object sender, PointerEventArgs e)
    {
        // Only when selling
        if(!selling)
        {
            return;
        }

        for (int i = 0; i < e.Pointers.Count; i++)
        {
            // Tell if we touched a card
            if (e.Pointers[i].GetPressData().Target != null && e.Pointers[i].GetPressData().Target.tag == "Card")
            {
                // Store in var for easier handling
                Transform lastTouchedCard = e.Pointers[i].GetPressData().Target;

                int lastTouchedCardIndex = int.Parse(lastTouchedCard.name);

                PlayerInfo player = Game.Instance.properties.playerDisplay[Game.Instance.playerTurnIndex];

                // Check if we own this card
                if (player.propertiesOwned.Contains(lastTouchedCardIndex))
                {
                    // Remove from owned cards
                    player.propertiesOwned.Remove(lastTouchedCardIndex);

                    // Give player money back
                    player.playerCurrency += Game.Instance.board.cardsPrice[lastTouchedCardIndex].price;

                    // Hide ownership
                    Game.Instance.board.boardPositions[lastTouchedCardIndex].GetComponentInChildren<SpriteRenderer>().enabled = false;

                    // Make it available again
                    Game.Instance.properties.availableCards.Add(lastTouchedCardIndex);

                    // Refresh Info
                    player.RefreshPlayerInfo();
                }
            }
        }
    }
}
