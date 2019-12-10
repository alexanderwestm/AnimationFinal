using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomStringFloat
{
    public string key;
    public float value;
}

[System.Serializable]
public class CustomDictionary
{
    public List<CustomStringFloat> list;

    public float this[string key]
    {
        get => GetValue(key);
        set => SetValue(key, value);
    }

    float GetValue(string key)
    {
        foreach(CustomStringFloat custom in list)
        {
            if(custom.key == key)
            {
                return custom.value;
            }
        }
        return 0;
    }

    void SetValue(string key, float value)
    {
        foreach(CustomStringFloat custom in list)
        {
            if(custom.key == key)
            {
                custom.value = value;
            }
        }
    }
}
