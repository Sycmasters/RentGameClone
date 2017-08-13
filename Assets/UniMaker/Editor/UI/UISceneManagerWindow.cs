using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public enum UItem
{
    Text,
    Images
}

public class UISceneManagerWindow : EditorWindow
{
    private Vector2 scrollPos;
    private UItem item;    

    // Add menu to the Window menu
    [MenuItem("Window/UniMaker/UIManagement")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        UISceneManagerWindow window = (UISceneManagerWindow)EditorWindow.GetWindow(typeof(UISceneManagerWindow));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(); 

        item = (UItem)EditorGUILayout.EnumPopup(new GUIContent("Select UI element:"), item);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox, GUILayout.Width(150), GUILayout.Height(200));

        // Show a list of UI Elements
        ShowUIElement();

        EditorGUILayout.EndScrollView();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    void ShowUIElement ()
    {
        if (item == UItem.Text)
        {
            Text[] objs = SceneView.FindObjectsOfType<Text>();
            for (int i = 0; i < objs.Length; i++)
            {
                if(GUILayout.Button(objs[i].name, EditorStyles.centeredGreyMiniLabel))
                {
                    Selection.activeObject = objs[i].gameObject;
                    EditorGUIUtility.PingObject(Selection.activeObject);
                    SceneView view = SceneView.lastActiveSceneView;

                    if (view != null)
                    {
                        view.FrameSelected();
                    }
                }
            }
        }
    }
}
