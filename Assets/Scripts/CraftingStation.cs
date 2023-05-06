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
    private List<RecipeScriptableObject> recipes;

    private HashSet<Item> _items = new HashSet<Item>();
    private float _minimumIngredients;

    private void Awake()
    {
        RefreshStatus();
        _minimumIngredients = recipes.Min(x => x.Ingridients.Count);
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
            foreach (RecipeScriptableObject recipe in recipes)
            {
                if (_items.Count == recipe.Ingridients.Count)
                {
                    foreach (Item item in recipe.Ingridients)
                    {
                        if (_items.All(x => x.Name != item.Name))
                        {
                            break;
                        }

                        return recipe.Result;
                    }
                }
            }
        }

        return null;
    }
}
