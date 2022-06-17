using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wallet : MonoBehaviour
{
    private SqliteSetBdd bdd;
    
    [Header("Manipulated")]
    public Text userName;
    
    [Header("Wallet")]
    public Text btc;
    public Text eth;
    public Text ltc;
    public Text eur;
    
    [Header("Wealth")]
    public Text btcW;
    public Text ethW;
    public Text ltcW;
    public Text eurW;

    public void UpdateUserData()
    {
        if (!userName || !btc || !eth || !ltc || !eur) return;

        bdd = FindObjectOfType<SqliteSetBdd>();

        userName.text = bdd.ud.pseudo;
        btc.text      = bdd.ud.btc.ToString("0.000") + " BTC";
        eth.text      = bdd.ud.eth.ToString("0.000") + " ETH";
        ltc.text      = bdd.ud.ltc.ToString("0.000") + " LTC";
        eur.text      = bdd.ud.eur.ToString("0.000") + " EUR";
        
        // WEALTH DATA
        if (!btcW || !ethW || !ltcW || !eurW) return;

        float btcValue = 0, ethValue = 0, ltcValue = 0, eurValue = 0;
        foreach (nft n in bdd.nftList)
            if (n.owner == bdd.ud.pseudo)
            {
                btcValue += n.btc;
                ethValue += n.eth;
                ltcValue += n.ltc;
                eurValue += n.eur;
            }
        
        btcW.text = btcValue.ToString("0.000") + " BTC";
        ethW.text = ethValue.ToString("0.000") + " ETH";
        ltcW.text = ltcValue.ToString("0.000") + " LTC";
        eurW.text = eurValue.ToString("0.000") + " EUR";
    }
}
