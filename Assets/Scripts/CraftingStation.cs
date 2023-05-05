using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text;

public class CraftingStation : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _ingredientsList = null;
    [SerializeField]
    private TextMeshPro _result = null;

    private void Awake()
    {
        RefreshStatus();
    }

    private void RefreshStatus()
    {
        _ingredientsList.text = "Waiting for ingredients...";
        _result.text = "Not enough items";      
    }
}
