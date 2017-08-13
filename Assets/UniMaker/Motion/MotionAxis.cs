using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionAxis : MonoBehaviour
{
    // Enum
    public enum AxesDirectionOrder
    {
        H_X_X,
        X_H_X,
        X_X_H,
        V_X_X,
        X_V_X,
        X_X_V,
        H_V_X,
        H_X_V,
        V_H_X,
        V_X_H,
        X_H_V,
        X_V_H
    }

    [Header("Settings")]
    public float forceBoost = 1;
    public bool smoothMovement;
    public bool allowHorizontal = true, allowVertical = true, allowInput = true;
    public AxesDirectionOrder axisOrder;

    private float horizontalAxis, verticalAxis;

    [Header("Data")]
    public Vector3 movementForce;
    public Vector3 movementForceOverride;

    void Update ()
    {
        if(!allowInput)
        {
            return;
        }

        // Change this with your favorite Input mapping
        horizontalAxis = Input.GetAxis("Horizontal") * forceBoost;
        verticalAxis = Input.GetAxis("Vertical") * forceBoost;

        // Tell if we have to smooth the movemnt
        horizontalAxis = smoothMovement ? horizontalAxis * Time.deltaTime : horizontalAxis;
        verticalAxis = smoothMovement ? verticalAxis * Time.deltaTime : verticalAxis;

        // Now we have to know if we are allowed to use any axes
        horizontalAxis = allowHorizontal ? horizontalAxis : 0;
        verticalAxis = allowVertical ? verticalAxis : 0;

        // Move movementForce with axes unless we need to override its movement from outside
        SetAxisOrder();
    }

    /// <summary>
    /// Sets the axis order.
    /// </summary>
    void SetAxisOrder()
    {
        switch (axisOrder)
        {
            case AxesDirectionOrder.H_X_X:
                movementForce = new Vector3(horizontalAxis, 0, 0);
                allowHorizontal = true;
                allowVertical = false;
                break;

            case AxesDirectionOrder.X_H_X:
                movementForce = new Vector3(0, horizontalAxis, 0);
                allowHorizontal = true;
                allowVertical = false;
                break;

            case AxesDirectionOrder.X_X_H:
                movementForce = new Vector3(0, 0, horizontalAxis);
                allowHorizontal = true;
                allowVertical = false;
                break;

            case AxesDirectionOrder.V_X_X:
                movementForce = new Vector3(verticalAxis, 0, 0);
                allowHorizontal = false;
                allowVertical = true;
                break;

            case AxesDirectionOrder.X_V_X:
                movementForce = new Vector3(0, verticalAxis, 0);
                allowHorizontal = false;
                allowVertical = true;
                break;

            case AxesDirectionOrder.X_X_V:
                movementForce = new Vector3(0, 0, verticalAxis);
                allowHorizontal = false;
                allowVertical = true;
                break;

            case AxesDirectionOrder.H_V_X:
                movementForce = new Vector3(horizontalAxis, verticalAxis, 0);
                allowHorizontal = true;
                allowVertical = true;
                break;

            case AxesDirectionOrder.H_X_V:
                movementForce = new Vector3(horizontalAxis, 0, verticalAxis);
                allowHorizontal = true;
                allowVertical = true;
                break;

            case AxesDirectionOrder.V_H_X:
                movementForce = new Vector3(verticalAxis, horizontalAxis, 0);
                allowHorizontal = true;
                allowVertical = true;
                break;

            case AxesDirectionOrder.V_X_H:
                movementForce = new Vector3(verticalAxis, 0, horizontalAxis);
                allowHorizontal = true;
                allowVertical = true;
                break;

            case AxesDirectionOrder.X_H_V:
                movementForce = new Vector3(0, horizontalAxis, verticalAxis);
                allowHorizontal = true;
                allowVertical = true;
                break;

            case AxesDirectionOrder.X_V_H:
                movementForce = new Vector3(0, verticalAxis, horizontalAxis);
                allowHorizontal = true;
                allowVertical = true;
                break;
        }
    }

    public Vector3 GetMovement ()
    {
        // We want to control movement, do not return input axis but set vector
        if(movementForceOverride.sqrMagnitude > 0 || !allowInput)
        {
            return movementForceOverride * forceBoost;
        }

        // Here we want to return the vector with player input
        return movementForce;
    }
}



