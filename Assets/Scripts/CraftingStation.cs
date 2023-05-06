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
    private Button button;
    [SerializeField]
    private Transform craftedItemAnchor;
    
    [SerializeField]
    private List<RecipeScriptableObject> _recipes = null;

    private HashSet<Item> _items = new HashSet<Item>();
    private float _minimumIngredients;
    private Item _currentRecipe;

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

    private void OnEnable()
    {
        button.ButtonPressed += OnButtonPressed;
    }

    private void OnDisable()
    {
        button.ButtonPressed -= OnButtonPressed;
    }

    private void RefreshStatus()
    {
        if (_items.Count == 0)
        {
            _ingredientsList.text = "Waiting for ingredients...";
            _result.text = "";
            _currentRecipe = null;
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
                _currentRecipe = null;

            }
            else
            {
                Item result = CheckRecipes();

                if (result != null)
                {
                    _result.text = result.Name;
                    _currentRecipe = result;
                }
                else
                {
                    _result.text = "Wrong items";
                    _currentRecipe = null;
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

    private void OnButtonPressed()
    {
        if (_currentRecipe != null)
        {
            foreach (Item item in _items)
            {
                Destroy(item.gameObject);
            }

            Instantiate(_currentRecipe, craftedItemAnchor.position, Quaternion.identity);
            
            _items.Clear();
            RefreshStatus();
        }
    }
}
