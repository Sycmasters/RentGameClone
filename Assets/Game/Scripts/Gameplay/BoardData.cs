﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript;

[System.Serializable]
public struct CardInfo
{
    public int price;
    public int rent;
    public int[] withProp;
}

public class BoardData : MonoBehaviour
{
    [Header("Board")]
    public Transform[] boardPositions;
    public HouseManager[] boardCardHouses;
    public SpriteRenderer[] boardOwnershipRender;

    [Header("Cards")]
    public Sprite[] cardsSprite;
    public CardInfo[] cardsPrice;

    [Header("Cards References")]
    public GameObject cardWindow;
    public Text cardPrice;
    public Image cardImage;

    [ContextMenu("GetReferences")]
    public void Init ()
    {
        boardCardHouses = new HouseManager[boardPositions.Length];
        boardOwnershipRender = new SpriteRenderer[boardPositions.Length];

        for(int i = 0; i < boardPositions.Length; i++)
        {
            HouseManager manager = boardPositions[i].GetComponentInChildren<HouseManager>();
            if(manager != null)
            {
                boardCardHouses[i] = manager;
            }
            else
            {
                boardCardHouses[i] = null;
            }

            SpriteRenderer render = boardPositions[i].GetComponentInChildren<SpriteRenderer>();
            if(render != null)
            {
                boardOwnershipRender[i] = render;
            }
            else
            {
                boardOwnershipRender[i] = null;
            }
        }
        Debug.Log("Init method in " + gameObject.name);
    }

    private void OnEnable ()
    {
		if(TouchManager.Instance != null)
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

    private void OnTouch (object sender, PointerEventArgs e)
    {
        // Don't show properties when selling
        if(Game.Instance.actions.sell.selling || Game.Instance.actions.build.building || 
            Game.Instance.actions.trade.trading || Game.Instance.actions.surpriseCards.shown || Game.Instance.actions.questCards.shown)
        {
            return;
        }

        for(int i = 0; i < e.Pointers.Count; i++)
        {
            // Tell if we touched a card
            if (e.Pointers[i].GetPressData().Target != null && e.Pointers[i].GetPressData().Target.tag == "Card")
            {
                // Store in var for easier handling
                Transform lastTouchedCard = e.Pointers[i].GetPressData().Target;

                int lastTouchedCardIndex = int.Parse(lastTouchedCard.name);

                // Check if is a displayable card (no quest, surprise, etc..)
                if (cardsSprite[lastTouchedCardIndex] != null)
                {
                    DisplayCardInfo(lastTouchedCardIndex);
                }
            }
        }
    }

    private void DisplayCardInfo (int index)
    {
        cardPrice.text = "G " + cardsPrice[index].price.ToString();
        cardImage.sprite = cardsSprite[index];
        cardWindow.SetActive(true);
    }
}

/* 
 * new Vector3[40] 
    {
        new Vector3(2.05f, 0, -2.05f), new Vector3(1.5f, 0, -2.05f), new Vector3(1.15f, 0, -2.05f), new Vector3(0.75f, 0, -2.05f), new Vector3(0.38f, 0, -2.05f),
        new Vector3(0, 0, -2.05f), new Vector3(-0.38f, 0, -2.05f), new Vector3(-0.75f, 0, -2.05f), new Vector3(-1.15f, 0, -2.05f), new Vector3(-1.5f, 0, -2.05f),
        new Vector3(-2.05f, 0, -2.05f), new Vector3(-2.05f, 0, -1.5f), new Vector3(-2.05f, 0, -1.15f), new Vector3(-2.05f, 0, -0.75f), new Vector3(-2.05f, 0, -0.38f),
        new Vector3(-2.05f, 0, 0), new Vector3(-2.05f, 0, 0.38f), new Vector3(-2.05f, 0, 0.75f), new Vector3(-2.05f, 0, 1.15f), new Vector3(-2.05f, 0, 1.5f),
        new Vector3(-2.05f, 0, 2.05f), new Vector3(-1.5f, 0, 2.05f), new Vector3(-1.15f, 0, 2.05f), new Vector3(-0.75f, 0, 2.05f), new Vector3(-0.38f, 0, 2.05f),
        new Vector3(0, 0, 2.05f), new Vector3(0.38f, 0, 2.05f), new Vector3(0.75f, 0, 2.05f), new Vector3(1.15f, 0, 2.05f), new Vector3(1.5f, 0, 2.05f),
        new Vector3(2.05f, 0, 2.05f), new Vector3(2.05f, 0, 1.5f), new Vector3(2.05f, 0, 1.15f), new Vector3(2.05f, 0, 0.75f), new Vector3(2.05f, 0, 0.38f),
        new Vector3(2.05f, 0, 0), new Vector3(2.05f, 0, -0.38f), new Vector3(2.05f, 0, -0.75f), new Vector3(2.05f, 0, -1.15f), new Vector3(2.05f, 0, -1.5f)
    };
*/