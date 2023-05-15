using System.Collections.Generic;
using UnityEngine;

namespace TestTaskCrafting.Crafting
{
    [CreateAssetMenu(menuName = "My Assets/Recipe")]
    public class RecipeScriptableObject : ScriptableObject
    {
        public List<Item> Ingredients;
        public Item Result;
    }
}
