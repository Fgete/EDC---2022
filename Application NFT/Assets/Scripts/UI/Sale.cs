using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Sale : MonoBehaviour
{
    private SqliteSetBdd bdd;
    private GalleryNft gNft;
    public CurrencySelect cs;

    [Header("Manipulated")]
    public GameObject galleryNftScreen;
    public GameObject marketScreen;

    private void Start()
    {
        bdd = FindObjectOfType<SqliteSetBdd>();
        gNft = FindObjectOfType<GalleryNft>();
    }

    public void Trigger()
    {
        switch (cs.currencies[cs.currency])
        {
            case "BTC":
                    bdd.SaleNft(gNft.nftId, cs.currencies[cs.currency], gNft.btcVal);
                    GoToMarket();
                break;
            case "ETH":
                    bdd.SaleNft(gNft.nftId, cs.currencies[cs.currency], gNft.ethVal);
                    GoToMarket();
                break;
            case "LTC":
                    bdd.SaleNft(gNft.nftId, cs.currencies[cs.currency], gNft.ltcVal);
                    GoToMarket();
                break;
            case "EUR":
                    bdd.SaleNft(gNft.nftId, cs.currencies[cs.currency], gNft.eurVal);
                    GoToMarket();
                break;
        }
    }

    void GoToMarket()
    {
        galleryNftScreen.SetActive(false);
        marketScreen.SetActive(true);
        marketScreen.GetComponent<Market>().UpdateMarket();
    }
}