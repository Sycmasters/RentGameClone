using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GetAllNeededReferences : Editor
{
    [MenuItem("Tools/Unimaker/GetNeededReferences")]
    public static void GetNeededReferences ()
    {
        GameObject[] objs = (GameObject[]) FindObjectsOfType(typeof(GameObject));

        for(int i = 0; i < objs.Length; i++)
        {
            objs[i].SendMessage("Init", SendMessageOptions.DontRequireReceiver);
        }
    }
}
