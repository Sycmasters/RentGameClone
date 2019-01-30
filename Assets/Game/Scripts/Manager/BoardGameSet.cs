using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New set", menuName = "Game Set")]

public class BoardGameSet : ScriptableObject
{
    public string setName;
    public PropertyInfo[] sections;   
    public Sprite[] cardsSprite;
    public string currencyString;

    public void SetSprite (SpriteRenderer render, int sectionIndex)
    {
        if(render != null)
            render.sprite = sections[sectionIndex].image;
    } 

    public void SetName (TextMeshPro name, int sectionIndex)
    {
        if(name != null)
            name.text = sections[sectionIndex].name;
    } 

    public void SetPrice (TextMeshPro price, int sectionIndex)
    {
        if(price != null)
            price.text = currencyString + sections[sectionIndex].price.ToString();
    } 
}

[System.Serializable]
public struct PropertyInfo
{
    public string name;
    public int price;
    public Sprite image;
}