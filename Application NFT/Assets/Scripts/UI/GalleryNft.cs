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
    public int nftId;
    public Text title;
    public Image image;
    
    [Header("Currencies")]
    public Text btc;
    public Text eth;
    public Text ltc;
    public Text eur;
    
    [HideInInspector] public float btcVal;
    [HideInInspector] public float ethVal;
    [HideInInspector] public float ltcVal;
    [HideInInspector] public float eurVal;
    
    [Header("Indicators")]
    public Text btcInd;
    public Text ethInd;
    public Text ltcInd;
    public Text eurInd;

    public void UpdateGalleryNft(int id)
    {
        bdd = FindObjectOfType<SqliteSetBdd>();
        gcd = FindObjectOfType<GetCryptoData>();
        
        nftId = id;
        title.text   = bdd.nftList[id].title;
        image.material = bdd.nftMaterials[id];
        
        DisplayValues(id);
        DisplayIndicators(id);
    }
    
        void DisplayValues(int id)
    {
        btc.text = bdd.nftList[id].btc.ToString("0.000") + " BTC";
        eth.text = bdd.nftList[id].eth.ToString("0.000") + " ETH";
        ltc.text = bdd.nftList[id].ltc.ToString("0.000") + " LTC";
        eur.text = bdd.nftList[id].eur.ToString("0.000") + " EUR";
        
        btcVal = bdd.nftList[id].btc;
        ethVal = bdd.nftList[id].eth;
        ltcVal = bdd.nftList[id].ltc;
        eurVal = bdd.nftList[id].eur;
        
        switch (bdd.nftList[id].currency)
        {
            case "BTC":
                btc.text = bdd.nftList[id].btc.ToString("0.000") + " BTC";
                eth.text = bdd.BtcToEth(bdd.nftList[id].btc).ToString("0.000") + " ETH";
                ltc.text = bdd.BtcToLtc(bdd.nftList[id].btc).ToString("0.000") + " LTC";
                eur.text = bdd.BtcToEur(bdd.nftList[id].btc).ToString("0.000") + " EUR";
                break;
            case "ETH":
                btc.text = bdd.EthToBtc(bdd.nftList[id].eth).ToString("0.000") + " BTC";
                eth.text = bdd.nftList[id].eth.ToString("0.000") + " ETH";
                ltc.text = bdd.EthToLtc(bdd.nftList[id].eth).ToString("0.000") + " LTC";
                eur.text = bdd.EthToEur(bdd.nftList[id].eth).ToString("0.000") + " EUR";
                break;
            case "LTC":
                btc.text = bdd.LtcToBtc(bdd.nftList[id].ltc).ToString("0.000") + " BTC";
                eth.text = bdd.LtcToEth(bdd.nftList[id].ltc).ToString("0.000") + " ETH";
                ltc.text = bdd.nftList[id].ltc.ToString("0.000") + " LTC";
                eur.text = bdd.LtcToEur(bdd.nftList[id].ltc).ToString("0.000") + " EUR";
                break;
            case "EUR":
                btc.text = bdd.EurToBtc(bdd.nftList[id].eur).ToString("0.000") + " BTC";
                eth.text = bdd.EurToEth(bdd.nftList[id].eur).ToString("0.000") + " ETH";
                ltc.text = bdd.EurToLtc(bdd.nftList[id].eur).ToString("0.000") + " LTC";
                eur.text = bdd.nftList[id].eur.ToString("0.000") + " EUR";
                break;
        }
    }
    
    void DisplayIndicators(int id)
    {
        btcInd.text = "";
        ethInd.text = "";
        ltcInd.text = "";
        eurInd.text = "";

        btcInd.color = Color.gray;
        ethInd.color = Color.gray;
        ltcInd.color = Color.gray;
        eurInd.color = Color.gray;
        
        switch (bdd.nftList[id].currency)
        {
            case "BTC":
                btcInd.text = "SOURCE";
                ethInd.text = GetPercent(gcd.cryptoList[1].value, gcd.cryptoList[0].value, bdd.nftList[id].btc, bdd.BtcToEth(bdd.nftList[id].btc)).ToString("0.0") + " %";
                ltcInd.text = GetPercent(gcd.cryptoList[2].value, gcd.cryptoList[0].value, bdd.nftList[id].btc, bdd.BtcToLtc(bdd.nftList[id].btc)).ToString("0.0") + " %";
                eurInd.text = GetPercent(gcd.cryptoList[3].value, gcd.cryptoList[0].value, bdd.nftList[id].btc, bdd.BtcToEur(bdd.nftList[id].btc)).ToString("0.0") + " %";
                btcInd.color = Color.blue;
                break;
            case "ETH":
                btcInd.text = GetPercent(gcd.cryptoList[0].value, gcd.cryptoList[1].value, bdd.nftList[id].eth, bdd.EthToBtc(bdd.nftList[id].eth)).ToString("0.0") + " %";
                ethInd.text = "SOURCE";
                ltcInd.text = GetPercent(gcd.cryptoList[2].value, gcd.cryptoList[1].value, bdd.nftList[id].eth, bdd.EthToLtc(bdd.nftList[id].eth)).ToString("0.0") + " %";
                eurInd.text = GetPercent(gcd.cryptoList[3].value, gcd.cryptoList[1].value, bdd.nftList[id].eth, bdd.EthToEur(bdd.nftList[id].eth)).ToString("0.0") + " %";
                ethInd.color = Color.blue;
                break;
            case "LTC":
                btcInd.text = GetPercent(gcd.cryptoList[0].value, gcd.cryptoList[2].value, bdd.nftList[id].ltc, bdd.LtcToBtc(bdd.nftList[id].ltc)).ToString("0.0") + " %";
                ethInd.text = GetPercent(gcd.cryptoList[1].value, gcd.cryptoList[2].value, bdd.nftList[id].ltc, bdd.LtcToEth(bdd.nftList[id].ltc)).ToString("0.0") + " %";
                ltcInd.text = "SOURCE";
                eurInd.text = GetPercent(gcd.cryptoList[3].value, gcd.cryptoList[2].value, bdd.nftList[id].ltc, bdd.LtcToEur(bdd.nftList[id].ltc)).ToString("0.0") + " %";
                ltcInd.color = Color.blue;
                break;
            case "EUR":
                btcInd.text = GetPercent(gcd.cryptoList[0].value, gcd.cryptoList[3].value, bdd.nftList[id].eur, bdd.EurToBtc(bdd.nftList[id].eur)).ToString("0.0") + " %";
                ethInd.text = GetPercent(gcd.cryptoList[1].value, gcd.cryptoList[3].value, bdd.nftList[id].eur, bdd.EurToEth(bdd.nftList[id].eur)).ToString("0.0") + " %";
                ltcInd.text = GetPercent(gcd.cryptoList[2].value, gcd.cryptoList[3].value, bdd.nftList[id].eur, bdd.EurToLtc(bdd.nftList[id].eur)).ToString("0.0") + " %";
                eurInd.text = "SOURCE";
                eurInd.color = Color.blue;
                break;
        }
    }

    float GetPercent(float nowTarget, float nowRef, float valRef, float valTarget)
    {
        return (((nowTarget / nowRef) * valRef) - valTarget) / valTarget;
    }
}
