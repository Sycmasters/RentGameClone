using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using matnesis.TeaTime;

public class TokenController : MonoBehaviour
{
    [Header("Settings")]
    public int tokenIndex = 0;
    public float moveDuration = 10;
    public float jumpPower = 1;

    [Header("Reference")]
    public Rigidbody rbody;

    [Header("Data")]
    public int currentTargetIndex = 0;

    private Vector3 rotate, translate;
    private TeaTime moveNextSection;

    // Start is called before the first frame update
    void Start()
    {
        if(rbody != null) rbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void MoveTo (Vector3[] target, int targetIndex)
    {
        moveNextSection = this.tt("@MoveSequence").Reset();

        for(int i = 0; i < target.Length; i++)
        {
            {
                Vector3 section = target[i];
                moveNextSection.Add(() => 
                {                
                    rotate = Quaternion.LookRotation(section - transform.position).eulerAngles;
                    rotate.x = transform.eulerAngles.x;

                    translate = section;
                    translate.y = transform.position.y;

                    transform.DOJump(translate, jumpPower, 1, moveDuration);
                    transform.DORotate(rotate, moveDuration);
                })
                .Add(moveDuration);
            }
        }
        moveNextSection.Immutable();

        currentTargetIndex = targetIndex;
    }
}
