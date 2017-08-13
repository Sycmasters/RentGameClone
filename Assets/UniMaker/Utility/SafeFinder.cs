using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeFinder : MonoBehaviour
{
    private static List<string> elementCache = new List<string>();
    private static List<object> elementObjectCache = new List<object>();

    public static T GetUIElementByName<T>(string name)
    {
        // Check if we have the object cached
        if(elementCache.Contains(name))
        {
            int index = elementCache.IndexOf(name);
            // Check if the cached object didn´t become null because it isn't on the scene
            if (elementObjectCache[index] != null)
            {
                // We still have it, use the cached item
                return (T) elementObjectCache[index];
            }
            else
            {
                // The object is null we have to look for it again
                GameObject obj = GameObject.Find(name);
                if(obj == null)
                {
                    // Object wasn't found
                    Debug.LogError("There is not \"" + name + "\" object on the scene");
                    return default(T);
                }
                else
                {
                    // Object found, we need to cache it
                    elementCache.Add(name);
                    elementObjectCache.Add(obj.GetComponent<T>());
                    return (T)elementObjectCache[elementObjectCache.Count-1];
                }
            }
        }
        else
        {
            // We have not obj cached, need to look for it
            GameObject obj = GameObject.Find(name);
            if (obj == null)
            {
                // Object wasn't found
                Debug.LogError("There is not \"" + name + "\" object on the scene");
                return default(T);
            }
            else
            {
                // Object found, we need to cache it
                elementCache.Add(name);
                elementObjectCache.Add(obj.GetComponent<T>());
                return (T)elementObjectCache[elementObjectCache.Count - 1];
            }
        }
    }

    public static T GetUIElementByTag<T>(string tag)
    {
        // Check if we have the object cached
        if (elementCache.Contains(tag))
        {
            int index = elementCache.IndexOf(tag);
            // Check if the cached object didn´t become null because it isn't on the scene
            if (elementObjectCache[index] != null)
            {
                // We still have it, use the cached item
                return (T)elementObjectCache[index];
            }
            else
            {
                // The object is null we have to look for it again
                GameObject obj = GameObject.FindGameObjectWithTag(tag);
                if (obj == null)
                {
                    // Object wasn't found
                    Debug.LogError("There is not object with \"" + tag + "\" on the scene");
                    return default(T);
                }
                else
                {
                    // Object found, we need to cache it
                    elementCache.Add(tag);
                    elementObjectCache.Add(obj.GetComponent<T>());
                    return (T)elementObjectCache[elementObjectCache.Count - 1];
                }
            }
        }
        else
        {
            // We have not obj cached, need to look for it
            GameObject obj = GameObject.FindGameObjectWithTag(tag);
            if (obj == null)
            {
                // Object wasn't found
                Debug.LogError("There is not \"" + tag + "\" object on the scene");
                return default(T);
            }
            else
            {
                // Object found, we need to cache it
                elementCache.Add(tag);
                elementObjectCache.Add(obj.GetComponent<T>());
                return (T)elementObjectCache[elementObjectCache.Count - 1];
            }
        }
    }
}
