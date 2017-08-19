using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TokenController : MonoBehaviour
{
    public int playerIndex = 0;
    public int boardLocation = 0;
    
    [SerializeField]
    private NavMeshAgent agent;
    private bool moving;

    // Use this for initialization
    [ContextMenu("GetReferences")]
    public void Init ()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log("Init method in " + gameObject.name);
    }

    public void Update()
    {
        // Approximately to destination
        if(agent.hasPath && agent.remainingDistance < 0.3f && moving)
        {
            //Debug.Log("Done");
            Game.Instance.TokenReachedDestination(boardLocation);
            moving = false;
        }
        if (agent.hasPath)
        {
            //Debug.Log(agent.remainingDistance);
        }
    }

    public void MoveToken (int spaces)
    {
        // Set the point at where we have to move to
        SetNextLocation(spaces);

        // Move token with NavMesh
        Vector3 boardPos = Game.Instance.board.boardPositions[boardLocation].position;
        // Need to set the y value of Vector 2 on board pos as new destination of token in Z
        boardPos.y = transform.position.y;
        agent.SetDestination(boardPos);
        // We are moving 
        moving = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player" && agent.remainingDistance > 0 && agent.remainingDistance <= 0.3f)
        {
            // Reset others movement so they won't fight for positions
            collision.gameObject.GetComponent<NavMeshAgent>().ResetPath();
            // Reset my movement to avoid fight for position with others
            agent.ResetPath();
        }
    }

    private void SetNextLocation(int add)
    {
        if ((boardLocation + add) < 39)
        {
            boardLocation += add;
        }
        else
        {
            // We surpassed the start point
            Game.Instance.actions.payment.EnableInitialPayment();
            boardLocation = (boardLocation + add) - 40;
        }
    }
}
