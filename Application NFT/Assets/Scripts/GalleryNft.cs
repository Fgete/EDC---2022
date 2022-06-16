using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryNft : MonoBehaviour
{
    private SqliteSetBdd bdd;
    private GetCryptoData gcd;

    [Header("Manipulated")]
    public Text title;
    public Image image;
    
    [Header("Currencies")]
    public Text btc;
    public Text eth;
    public Text ltc;
    public Text eur;
    
    [Header("Indicators")]
    public Text btcInd;
    public Text ethInd;
    public Text ltcInd;
    public Text eurInd;

    public void UpdateMarketNft(int id)
    {
        bdd = FindObjectOfType<SqliteSetBdd>();
        gcd = FindObjectOfType<GetCryptoData>();
        
        title.text   = bdd.nftList[id].title;
        image.sprite = bdd.nftImages[id];
        btc.text     = bdd.nftList[id].btc.ToString("0.000") + " BTC";
        eth.text     = bdd.nftList[id].eth.ToString("0.000") + " ETH";
        ltc.text     = bdd.nftList[id].ltc.ToString("0.000") + " LTC";
        eur.text     = bdd.nftList[id].eur.ToString("0.000") + " EUR";
        
        switch (bdd.nftList[id].currency)
        {
            case "BTC":
                btcInd.text = "ACTUAL";
                ethInd.text = GetPercent(gcd.cryptoList[1].value, gcd.cryptoList[0].value, bdd.nftList[id].btc, bdd.nftList[id].eth).ToString("0.0") + " %";
                ltcInd.text = GetPercent(gcd.cryptoList[2].value, gcd.cryptoList[0].value, bdd.nftList[id].btc, bdd.nftList[id].ltc).ToString("0.0") + " %";
                eurInd.text = GetPercent(gcd.cryptoList[3].value, gcd.cryptoList[0].value, bdd.nftList[id].btc, bdd.nftList[id].eur).ToString("0.0") + " %";
                break;
            case "ETH":
                btcInd.text = GetPercent(gcd.cryptoList[0].value, gcd.cryptoList[1].value, bdd.nftList[id].eth, bdd.nftList[id].btc).ToString("0.0") + " %";
                ethInd.text = "ACTUAL";
                ltcInd.text = GetPercent(gcd.cryptoList[2].value, gcd.cryptoList[1].value, bdd.nftList[id].eth, bdd.nftList[id].ltc).ToString("0.0") + " %";
                eurInd.text = GetPercent(gcd.cryptoList[3].value, gcd.cryptoList[1].value, bdd.nftList[id].eth, bdd.nftList[id].eur).ToString("0.0") + " %";
                break;
            case "LTC":
                btcInd.text = GetPercent(gcd.cryptoList[0].value, gcd.cryptoList[2].value, bdd.nftList[id].ltc, bdd.nftList[id].btc).ToString("0.0") + " %";
                ethInd.text = GetPercent(gcd.cryptoList[1].value, gcd.cryptoList[2].value, bdd.nftList[id].ltc, bdd.nftList[id].eth).ToString("0.0") + " %";
                ltcInd.text = "ACTUAL";
                eurInd.text = GetPercent(gcd.cryptoList[3].value, gcd.cryptoList[2].value, bdd.nftList[id].ltc, bdd.nftList[id].eur).ToString("0.0") + " %";
                break;
            case "EUR":
                btcInd.text = GetPercent(gcd.cryptoList[0].value, gcd.cryptoList[3].value, bdd.nftList[id].eur, bdd.nftList[id].btc).ToString("0.0") + " %";
                ethInd.text = GetPercent(gcd.cryptoList[1].value, gcd.cryptoList[3].value, bdd.nftList[id].eur, bdd.nftList[id].eth).ToString("0.0") + " %";
                ltcInd.text = GetPercent(gcd.cryptoList[2].value, gcd.cryptoList[3].value, bdd.nftList[id].eur, bdd.nftList[id].ltc).ToString("0.0") + " %";
                eurInd.text = "ACTUAL";
                break;
        }
    }

    float GetPercent(float nowTarget, float nowRef, float valRef, float valTarget)
    {
        return (((nowTarget / nowRef) * valRef) - valTarget) / valTarget;
    }
}
