using System.Collections;
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
    
    [Header("Cards")]
    public Sprite[] cardsSprite;
    public CardInfo[] cardsPrice;

    [Header("Cards References")]
    public GameObject cardWindow;
    public Text cardPrice;
    public Image cardImage;

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