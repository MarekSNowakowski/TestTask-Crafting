using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingStation : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _ingredientsList = null;
    [SerializeField]
    private TextMeshPro _result = null;

    private HashSet<Item> _items = new HashSet<Item>();

    private void Awake()
    {
        RefreshStatus();
    }

    private void RefreshStatus()
    {
        if (_items.Count > 0)
        {
            string ingridientList = "";
            
            foreach (Item item in _items)
            {
                ingridientList += ($" - {item.Name}\n");
            }

            _ingredientsList.text = ingridientList;
        }
        else
        {
            _ingredientsList.text = "Waiting for ingredients...";
            _result.text = "Not enough items";     
        }
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
}
