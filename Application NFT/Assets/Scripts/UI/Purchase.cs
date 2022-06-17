using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Purchase : MonoBehaviour
{
    private SqliteSetBdd bdd;
    private MarketNft mNft;
    public CurrencySelect cs;

    [Header("Manipulated")]
    public GameObject marketNftScreen;
    public GameObject galleryScreen;
    public Text feedback;

    private void Start()
    {
        bdd = FindObjectOfType<SqliteSetBdd>();
        mNft = FindObjectOfType<MarketNft>();
    }

    public void Trigger()
    {
        feedback.text = "You don't have enough of this currency.\nTry to switch currency.";
        
        switch (cs.currencies[cs.currency])
        {
            case "BTC":
                if (mNft.btcVal <= bdd.ud.btc)
                {
                    bdd.BuyNft(mNft.nftId, cs.currencies[cs.currency], mNft.btcVal);
                    GoToGallery();
                }
                break;
            case "ETH":
                if (mNft.ethVal <= bdd.ud.eth)
                {
                    bdd.BuyNft(mNft.nftId, cs.currencies[cs.currency], mNft.ethVal);
                    GoToGallery();
                }
                break;
            case "LTC":
                if (mNft.ltcVal <= bdd.ud.ltc)
                {
                    bdd.BuyNft(mNft.nftId, cs.currencies[cs.currency], mNft.ltcVal);
                    GoToGallery();
                }
                break;
            case "EUR":
                if (mNft.eurVal <= bdd.ud.eur)
                {
                    bdd.BuyNft(mNft.nftId, cs.currencies[cs.currency], mNft.eurVal);
                    GoToGallery();
                }
                break;
        }
    }

    void GoToGallery()
    {
        feedback.text = "";
        marketNftScreen.SetActive(false);
        galleryScreen.SetActive(true);
        galleryScreen.GetComponent<Gallery>().UpdateGallery();
    }
}
