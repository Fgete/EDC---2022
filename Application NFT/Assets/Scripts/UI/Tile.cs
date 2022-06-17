using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [Header("Manipulated")]
    public int id;
    public Text title;
    public Image image;
    public Text price;

    public void GoToNft()
    {
        if (transform.parent.parent.GetComponent<Market>())
            GoToMarketNft();
        else if (transform.parent.parent.GetComponent<Gallery>())
            GoToGalleryNft();
    }

    void GoToMarketNft()
    {
        Market m = FindObjectOfType<Market>();
        if (m)
            m.GoToNft(id);
    }
    
    void GoToGalleryNft()
    {
        Gallery g = FindObjectOfType<Gallery>();
        if (g)
            g.GoToNft(id);
    }
}
