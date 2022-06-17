using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencySelect : MonoBehaviour
{
    public string[] currencies = {"BTC", "ETH", "LTC", "EUR"};
    [Range(0, 3)]
    public int currency;
    public Text text;

    private void Awake()
    {
        if (text)
            text.text = currencies[currency] + " ▼";
    }

    public void ChangeCurrency()
    {
        if (currency < currencies.Length - 1)
            currency++;
        else
            currency = 0;
        
        text.text = currencies[currency] + " ▼";
    }
}
