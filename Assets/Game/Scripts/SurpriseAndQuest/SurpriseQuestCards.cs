using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript;
using matnesis.TeaTime;

public class SurpriseQuestCards : MonoBehaviour
{
    public GameObject[] cards;
    public int currCardIndex;

    public Animator cardAnim;
    public TextMesh cardDescription;
    public int cardLimit = 1;
    public bool shown;

    [SerializeField]
    private int lastCardIndex;

    private void Start()
    {
        lastCardIndex = currCardIndex;

        // Randomly set the order of cards
        for (int i = 0; i < cards.Length; i++)
        {
            GameObject temp = cards[i];
            int randomIndex = Random.Range(i, cards.Length);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowCard();
        }
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

    private void OnTouch (object sender, PointerEventArgs e)
    {
        for(int i = 0; i < e.Pointers.Count; i++)
        {
            if(shown)
            {
                this.tt("@HideCard").Add(() =>
                {
                    cardAnim.SetBool("Show", false);
                }).Add(0.5f, () =>
                {
                    // Accomplish function
                    cards[lastCardIndex].SendMessage("PerformCard", SendMessageOptions.DontRequireReceiver);
                    lastCardIndex = currCardIndex;
                    shown = false;
                }).Immutable();
            }
        }
    }

    public void ShowCard()
    {
        // Get Description
        cards[currCardIndex].SendMessage("SetDescription", cardDescription, SendMessageOptions.DontRequireReceiver);

        // Show card
        this.tt("@ShowCard").Add(() =>
        {
            cardAnim.SetBool("Show", true);
        }).Add(1.2f, () =>
        {
            shown = true;
        }).Immutable();

        // Next card
        // If is not the last one
        if (currCardIndex < cardLimit)
        {
            currCardIndex++;
        }
        else
        {
            // Repeat the cycle
            currCardIndex = 0;
        }
    }
}