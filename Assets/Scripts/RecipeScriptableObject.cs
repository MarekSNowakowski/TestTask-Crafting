using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Recipe")]
public class RecipeScriptableObject : ScriptableObject
{
    public List<Item> Ingridients;
    public Item Result;
}
