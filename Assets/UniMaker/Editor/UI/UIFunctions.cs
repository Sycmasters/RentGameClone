
using UnityEngine;
using UnityEditor;
using System.Collections;

public class UIFunctions : Editor
{
    [MenuItem("Tools/UI/Set Anchors")] //"Tools/Set UI Anchors"
    public static void SetAnchorsToCorners(MenuCommand menuCommand)
    {
        GameObject[] o = Selection.gameObjects;
        for (int i = 0; i < o.Length; i++)
        {
            if (o[i] != null && o[i].GetComponent<RectTransform>() != null)
            {
                RectTransform r = o[i].GetComponent<RectTransform>();
                RectTransform p = o[i].transform.parent.GetComponent<RectTransform>();

                Vector2 offsetMin = r.offsetMin;
                Vector2 offsetMax = r.offsetMax;
                Vector2 _anchorMin = r.anchorMin;
                Vector2 _anchorMax = r.anchorMax;

                float parent_width = p.rect.width;
                float parent_height = p.rect.height;

                Vector2 anchorMin = new Vector2(_anchorMin.x + (offsetMin.x / parent_width),
                    _anchorMin.y + (offsetMin.y / parent_height));
                Vector2 anchorMax = new Vector2(_anchorMax.x + (offsetMax.x / parent_width),
                    _anchorMax.y + (offsetMax.y / parent_height));

                r.anchorMin = anchorMin;
                r.anchorMax = anchorMax;

                r.offsetMin = new Vector2(0, 0);
                r.offsetMax = new Vector2(0, 0);
                r.pivot = new Vector2(0.5f, 0.5f);
            }
        }
    }
}