using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MealQuest
{
    public string questName;
    public List<string> requiredItems; // List of item names
    public bool isCompleted;
}
