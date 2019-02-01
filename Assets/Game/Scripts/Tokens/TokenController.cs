using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TokenController : MonoBehaviour
{
    [Header("Settings")]
    public int tokenIndex = 0;
    public float moveDuration = 10;

    [Header("Reference")]
    public Rigidbody rbody;

    [Header("Data")]
    public int currentTargetIndex = 0;

    private Vector3 rotate, translate;

    // Start is called before the first frame update
    void Start()
    {
        if(rbody != null) rbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void MoveTo (Vector3 target, int targetIndex)
    {
        rotate = Quaternion.LookRotation(target - transform.position).eulerAngles;
        rotate.x = transform.eulerAngles.x;

        translate = target;
        translate.y = transform.position.y;

        transform.DOMove(translate, moveDuration);
        transform.DORotate(rotate, moveDuration);

        currentTargetIndex = targetIndex;
    }

    // We need to move linear can't jump to another 
    public void MoveTo (Vector3[] target, int targetIndex)
    {
        rotate = Quaternion.LookRotation(target[0] - transform.position).eulerAngles;
        rotate.x = transform.eulerAngles.x;

        translate = target[0];
        translate.y = transform.position.y;

        transform.DOMove(translate, moveDuration);
        transform.DORotate(rotate, moveDuration);

        currentTargetIndex = targetIndex;
    }

    public void ShareSpace ()
    {

    }

    private void OnTriggerEnter (Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TokenController token = other.GetComponent<TokenController>();
            if(token.currentTargetIndex == currentTargetIndex)
            {
                token.ShareSpace();
            }
        }
    }
}
