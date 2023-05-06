using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class CraftingStation : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _ingredientsList = null;
    [SerializeField]
    private TextMeshPro _result = null;
    
    [SerializeField]
    private List<RecipeScriptableObject> _recipes = null;

    private HashSet<Item> _items = new HashSet<Item>();
    private float _minimumIngredients;

    private void Awake()
    {
        RefreshStatus();
        _minimumIngredients = _recipes.Min(x => x.Ingredients.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Item item))
        {
            _items.Add(item);
            RefreshStatus();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Item item))
        {
            _items.Remove(item);
            RefreshStatus();
        }
    }
    
    private void RefreshStatus()
    {
        if (_items.Count == 0)
        {
            _ingredientsList.text = "Waiting for ingredients...";
            _result.text = "";
        }
        else
        {
            // List ingredients
            string ingredientList = "";
            
            foreach (Item item in _items)
            {
                ingredientList += ($" - {item.Name}\n");
            }

            _ingredientsList.text = ingredientList;

            // Check for recipes if there is enough ingredients
            if (_items.Count < _minimumIngredients)
            {
                _result.text = "Not enough items";
            }
            else
            {
                Item result = CheckRecipes();

                if (result != null)
                {
                    _result.text = result.Name;
                }
                else
                {
                    _result.text = "Wrong items";    
                }
            }
        }
    }

    private Item CheckRecipes()
    {
        if (_items.Count > 0)
        {
            foreach (RecipeScriptableObject recipe in _recipes)
            {
                if (_items.Count == recipe.Ingredients.Count)
                {
                    // Check if items in crafting stations match ingredients in each recipe
                    IEnumerable<Item> matchingItems = recipe.Ingredients.Where(x => _items.Any(y => y.Name == x.Name));

                    if (matchingItems.Count() == recipe.Ingredients.Count)
                    {
                        return recipe.Result;
                    }
                }
            }
        }

        return null;
    }
}
