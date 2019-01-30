using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardInformation : MonoBehaviour
{
    public BoardGameSet gameSet;
    public SpriteRenderer[] sectionRenderer;
    public TextMeshPro[] sectionName;
    public TextMeshPro[] sectionPrice;

    // Start is called before the first frame update
    void Start()
    {
        if(gameSet != null)
        {
            for(int i = 0; i < sectionRenderer.Length; i++)
            {
                // Set sprites on the board
                gameSet.SetSprite(sectionRenderer[i], i);
                gameSet.SetName(sectionName[i], i);
                gameSet.SetPrice(sectionPrice[i], i);
            }
        }
    }
}
