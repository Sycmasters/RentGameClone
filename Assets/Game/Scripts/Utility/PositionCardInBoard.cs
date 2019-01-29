using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCardInBoard : MonoBehaviour
{
    [ContextMenu("Move card left")]
    private void MoveCardLeft ()
    {
        transform.localPosition += Vector3.right * 0.001558f;
    }
    
    [ContextMenu("Move card right")]
    private void MoveCardRight ()
    {
        transform.localPosition += Vector3.left * 0.001558f;
    }

    [ContextMenu("Move card up")]
    private void MoveCardUp ()
    {
        transform.localPosition += Vector3.up * 0.001558f;
    }
    
    [ContextMenu("Move card down")]
    private void MoveCardDown ()
    {
        transform.localPosition += Vector3.down * 0.001558f;
    }
}
