using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class HouseManager : MonoBehaviour
{
    public Animator anim;
    public int currHouses;
    public int housePrice = 50;
    public int[] sameOfAKind;
    public bool cantClickBuy;
    
    public void BuyHouses ()
    {
        if(!DoesPlayerHaveThemAll() || cantClickBuy)
        {
            return;
        }

        this.tt("@ClickBuy").Reset().Add(() =>
        {
            if (currHouses < 5)
            {
                currHouses++;
                anim.SetInteger("HouseNumber", currHouses);
            }
            cantClickBuy = true;
        }).Add(0.5f, () => { cantClickBuy = false; }).Immutable();
    }

    public void TakeOffHouses ()
    {
        if (!DoesPlayerHaveThemAll())
        {
            return;
        }

        this.tt("@ClickTakeOff").Reset().Add(() =>
        {
            if (currHouses > 0)
            {
                currHouses--;
                anim.SetInteger("HouseNumber", currHouses);
            }
            cantClickBuy = true;
        }).Add(0.5f, () => { cantClickBuy = false; }).Immutable();
    }

    public bool DoesPlayerHaveThemAll ()
    {
        bool haveThem = true;
        for(int i = 0; i < sameOfAKind.Length; i++)
        {
            if(!Game.Instance.CurrentPlayer.propertiesOwned.Contains(sameOfAKind[i]))
            {
                haveThem = false;
                break;
            }
        }

        return haveThem;
    }

    public bool DoesPlayerHaveThemAll(int playerIndex)
    {
        bool haveThem = true;
        for (int i = 0; i < sameOfAKind.Length; i++)
        {
            if (!Game.Instance.properties.playerDisplay[playerIndex].propertiesOwned.Contains(sameOfAKind[i]))
            {
                haveThem = false;
                break;
            }
        }

        return haveThem;
    }

    public bool OthersHaveHouses (int currentProp)
    {
        bool haveThem = false;
        for (int i = 0; i < sameOfAKind.Length; i++)
        {
            if(sameOfAKind[i] == currentProp)
            {
                continue;
            }
            else if (Game.Instance.board.boardCardHouses[sameOfAKind[i]].currHouses > 0) 
            {
                haveThem = true;
                break;
            }
        }

        return haveThem;
    }
}
