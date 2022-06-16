using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class Gallery : MonoBehaviour
{
    private SqliteSetBdd bdd;

    [Header("Prefab")]
    public GameObject tilePrefab;

    [Header("Manipulated")] 
    public GameObject grid;
    public GameObject galleryScreen;
    public GameObject galleryNftScreen;

    public void UpdateGallery()
    {
        if (!grid) return;

        bdd = FindObjectOfType<SqliteSetBdd>();

        // --- CLEAR GRID
        foreach (Transform t in grid.transform)
            Destroy(t.gameObject);
        
        // --- FILL GRID
        foreach (nft sample in bdd.nftList)
        {
            if (sample.owner == bdd.ud.pseudo)
            {
                GameObject newTile = Instantiate(tilePrefab, grid.transform, true);
                Tile tileComponent = newTile.GetComponent<Tile>();

                tileComponent.id = sample.id;
                tileComponent.title.text = sample.title;
                tileComponent.image.sprite = bdd.nftImages[sample.id];

                switch (sample.currency)
                {
                    case "BTC": tileComponent.price.text = sample.btc + " BTC"; break;
                    case "ETH": tileComponent.price.text = sample.eth + " ETH"; break;
                    case "LTC": tileComponent.price.text = sample.ltc + " LTC"; break;
                    case "EUR": tileComponent.price.text = sample.eur + " EUR"; break;
                }
            }
        }
    }
    
    public void GoToNft(int id)
    {
        galleryScreen.SetActive(false);
        galleryNftScreen.SetActive(true);
        galleryNftScreen.GetComponent<MarketNft>().UpdateMarketNft(id);
    }
}
