using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MotionAxis))]
[CanEditMultipleObjects]
public class MotionAxisEditor : Editor
{
    // Script properties
    private SerializedProperty allowInput;
    private SerializedProperty axisOrder;
    private SerializedProperty forceBoost;
    private SerializedProperty smoothMovement; 
    private SerializedProperty movementForceOverride; 

    // Textures for custom editor
    private Texture2D axisTexture;
    private Sprite[] axisSprites;

    void OnEnable()
    {
        // Reference to properties
        allowInput = serializedObject.FindProperty("allowInput");
        axisOrder = serializedObject.FindProperty("axisOrder");
        forceBoost = serializedObject.FindProperty("forceBoost");
        smoothMovement = serializedObject.FindProperty("smoothMovement");
        movementForceOverride = serializedObject.FindProperty("movementForceOverride");

        // Textures for buttons 
        axisTexture = (Texture2D)Resources.Load("EditorTextures/Axis/Atlas");
        axisSprites = Resources.LoadAll<Sprite>("EditorTextures/Axis/Atlas");
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        /***** PLAYER INPUT *****/
        EditorGUILayout.PropertyField(allowInput, new GUIContent("Input allowed"));

        /***** AXIS DIRECTION *****/
        if (allowInput.boolValue)
        {
            GUILayout.Label("Axis:");

            // X Y Z - H
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 0) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[0]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 0;
            }
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 1) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[1]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 1;
            }
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 2) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[2]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 2;
            }
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 3) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[3]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 3;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            // X Y Z - V
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 4) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[4]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 4;
            }
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 5) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[5]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 5;
            }
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 6) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[6]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 6;
            }
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 7) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[7]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 7;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            // XY - HV / XZ - HV / XY VH
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 8) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[8]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 8;
            }
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 9) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[9]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 9;
            }
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 10) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[10]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 10;
            }
            GUI.color = new Color(1, 1, 1, (axisOrder.intValue == 11) ? 1 : 0.5f);
            if (GUILayout.Button(AssetPreview.GetAssetPreview(axisSprites[11]), GUILayout.Width(80), GUILayout.Height(80)))
            {
                axisOrder.intValue = 11;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        // Recover normal colors for rest of the UI
        GUI.color = new Color(1, 1, 1, 1);

        // Motion internal speed boost
        EditorGUILayout.PropertyField(forceBoost, new GUIContent("Force boost"));

        // Smooth movement
        EditorGUILayout.PropertyField(smoothMovement, new GUIContent("Smooth movement"));

        // Override motion for external control or not input control
        EditorGUILayout.PropertyField(movementForceOverride, new GUIContent("Override movement"));

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.*/
        serializedObject.ApplyModifiedProperties();
    }
}