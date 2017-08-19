using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript; 

public class BuildAction : MonoBehaviour
{
    public bool building;
    public GameObject buildWindow;

    [SerializeField]
    private Button buildButton;
    [SerializeField]
    private HouseManager[] housesManager;

    // Use this for initialization
    [ContextMenu("GetReferences")]
    public void Init ()
    {
        buildButton = GetComponent<Button>();
        housesManager = FindObjectsOfType<HouseManager>();
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

    public void StartBuilding()
    {
        // Stop sell action if is selling
        if(Game.Instance.actions.sell.selling)
        {
            Game.Instance.actions.sell.EndSelling();
        }

        building = true;
        buildWindow.SetActive(true);
    }

    public void EndBuilding()
    {
        building = false;
        buildWindow.SetActive(false);
    }

    public void CheckOnBuildButton ()
    {
        // Do we own a property?
        bool isActive = false;
        for(int i = 0; i < housesManager.Length; i++)
        {
            if(housesManager[i].DoesPlayerHaveThemAll())
            {
                isActive = true;
                break;
            }
        }

        buildButton.interactable = isActive;
    }

    private void OnTouch(object sender, PointerEventArgs e)
    {
        // Only when selling
        if (!building)
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
                    // Get the manager for this property
                    HouseManager manager = Game.Instance.board.boardCardHouses[lastTouchedCardIndex];

                    // Build Houses for the prop
                    if(manager != null && manager.currHouses < 5 && !manager.cantClickBuy)
                    {
                        // Buy house
                        manager.BuyHouses();

                        // Get paid
                        Game.Instance.CurrentPlayer.SubstractCurrency(manager.housePrice);

                        // Refresh Info
                        Game.Instance.CurrentPlayer.RefreshPlayerInfo();
                    }
                }
            }
        }
    }
}
