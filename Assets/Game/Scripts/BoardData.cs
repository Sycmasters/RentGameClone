using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript;

public class BoardData : MonoBehaviour
{
    public Transform[] boardPositions;
    public Sprite[] cardsSprite;

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
                Debug.Log(e.Pointers[i].GetPressData().Target.name);
            }
        }
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