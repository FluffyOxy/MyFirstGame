using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TranslatePair
{
    public string from;
    public string to;
}
public class Translater : MonoBehaviour
{
    public static Translater instance;
    [SerializeField] private List<TranslatePair> translateInfo;
     Dictionary<string, string> dictionary = new Dictionary<string, string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (TranslatePair pair in translateInfo)
        {
            dictionary.Add(pair.from, pair.to);
        }
    }

    public string Translate(string _eng)
    {
        if (dictionary.TryGetValue(_eng, out string trans))
        {
            return trans;
        }
        else
        {
            return _eng;
        }
    }
}
