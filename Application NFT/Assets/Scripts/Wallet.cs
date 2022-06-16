using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wallet : MonoBehaviour
{
    private SqliteSetBdd bdd;
    
    [Header("Manipulated")]
    public Text userName;
    public Text btc;
    public Text eth;
    public Text ltc;
    public Text eur;

    public void UpdateUserData()
    {
        if (!userName || !btc || !eth || !ltc || !eur) return;

        bdd = FindObjectOfType<SqliteSetBdd>();

        userName.text = bdd.ud.pseudo;
        btc.text      = bdd.ud.btc.ToString("0.000") + " BTC";
        eth.text      = bdd.ud.eth.ToString("0.000") + " ETH";
        ltc.text      = bdd.ud.ltc.ToString("0.000") + " LTC";
        eur.text      = bdd.ud.eur.ToString("0.000") + " EUR";
    }
}
