using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        dropdown.options = new List<TMP_Dropdown.OptionData>();
        foreach(Resolution res in Screen.resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(res.width + " x " + res.height);
            dropdown.options.Add(option);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
