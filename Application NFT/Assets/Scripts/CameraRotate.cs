using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float velocity = 1f;

    private GameObject mNft;
    private GameObject gNft;

    private void Start()
    {
        mNft = Resources.FindObjectsOfTypeAll<MarketNft>()[0].gameObject;
        gNft = Resources.FindObjectsOfTypeAll<GalleryNft>()[0].gameObject;
    }

    private void Update()
    {
        if (mNft.activeSelf || gNft.activeSelf)
            transform.eulerAngles += new Vector3(0, Time.deltaTime * velocity, 0);
    }
}
