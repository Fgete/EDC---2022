using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    private SqliteSetBdd bdd;

    [Header("Prefab")]
    public GameObject tilePrefab;

    [Header("Manipulated")] 
    public GameObject grid;
    public GameObject marketScreen;
    public GameObject marketNftScreen;

    public void UpdateMarket()
    {
        if (!grid) return;

        bdd = FindObjectOfType<SqliteSetBdd>();

        // --- CLEAR GRID
        foreach (Transform t in grid.transform)
            Destroy(t.gameObject);
        
        // --- FILL GRID
        foreach (nft sample in bdd.nftList)
        {
            if (sample.owner == "")
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
        marketScreen.SetActive(false);
        marketNftScreen.SetActive(true);
        marketNftScreen.GetComponent<MarketNft>().UpdateMarketNft(id);
    }
}
