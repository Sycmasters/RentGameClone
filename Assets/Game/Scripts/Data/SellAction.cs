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
    [ContextMenu("GetReferences")]
    public void Init ()
    {
        sellButton = GetComponent<Button>();
        Debug.Log("Init method in " + gameObject.name);
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
        bool isActive = Game.Instance.CurrentPlayer.propertiesOwned.Count > 0;
        sellButton.interactable = isActive;
    }

    public void StartSelling ()
    {
        // Stop build action if is building
        if (Game.Instance.actions.build.building)
        {
            Game.Instance.actions.build.EndBuilding();
        }

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

                // Check if we own this card
                if (Game.Instance.CurrentPlayer.propertiesOwned.Contains(lastTouchedCardIndex))
                {
                    // Get houses manager
                    HouseManager manager = Game.Instance.board.boardCardHouses[lastTouchedCardIndex];

                    // Check if it has houses or hotels 
                    if (manager.currHouses > 0 && !manager.cantClickBuy)
                    {
                        // Take off house
                        manager.TakeOffHouses();

                        // Get paid
                        Game.Instance.CurrentPlayer.AddCurrency(manager.housePrice);

                        // Refresh Info
                        Game.Instance.CurrentPlayer.RefreshPlayerInfo();
                    }
                    // Else we sell the property
                    else
                    {
                        if (!manager.OthersHaveHouses(lastTouchedCardIndex))
                        {
                            // Remove from owned cards
                            Game.Instance.CurrentPlayer.propertiesOwned.Remove(lastTouchedCardIndex);

                            // Give player money back
                            Game.Instance.CurrentPlayer.AddCurrency(Game.Instance.board.cardsPrice[lastTouchedCardIndex].price);

                            // Hide ownership
                            Game.Instance.board.boardOwnershipRender[lastTouchedCardIndex].enabled = false;

                            // Make it available again
                            Game.Instance.properties.availableCards.Add(lastTouchedCardIndex);

                            // Refresh Info
                            Game.Instance.CurrentPlayer.RefreshPlayerInfo();

                            // Check if we can sell more
                            CheckOnSellButton();

                            // If we sell a property might be posible not to build
                            Game.Instance.actions.build.CheckOnBuildButton();
                        }
                        else
                        {
                            Debug.Log("Other properties have houses, sell them first");
                        }
                    }
                    Game.Instance.actions.payment.CheckIfIStillNeedToPay();
                }
            }
        }
    }
}
