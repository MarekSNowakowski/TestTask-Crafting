using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "My Assets/Recipe")]
public class RecipeScriptableObject : ScriptableObject
{
    public List<Item> Ingredients;
    public Item Result;
}
