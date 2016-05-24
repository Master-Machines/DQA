using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Card
{
    public string name;
    public string description;
    public string imageResourceName;
    public List<TriggerEffectPair> triggers;

    public Card(string Name, string Description, string ImageResourceName) {
        name = Name;
        description = Description;
        imageResourceName = ImageResourceName;
    }

    public bool Equals(Card other)
    {
        return true;
    }
}

