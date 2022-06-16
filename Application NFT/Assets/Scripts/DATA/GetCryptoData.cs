using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class crypto
{
    public string name;
    public float value;
}

public class GetCryptoData : MonoBehaviour
{
    private SqliteSetBdd bdd;
    public int refreshTime = 10;
    
    private string uri_ping    = "https://api.coingecko.com/api/v3/ping";
    private string[] currenties = {"btc", "eth", "ltc", "eur"};
    private string uri_getData = "https://api.coingecko.com/api/v3/simple/price?ids=ethereum&vs_currencies=";
    
    [SerializeField]
    public List<crypto> cryptoList = new List<crypto>();
    
    private void Start()
    {
        // --- INITIALIZE COMPONENTS
        bdd = FindObjectOfType<SqliteSetBdd>();
        
        // --- BUILD URL
        string concatCurrenties = "";
        foreach (string s in currenties)
            concatCurrenties += s + ",";
        uri_getData += concatCurrenties;
    }

    IEnumerator Ping(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                Debug.Log("Error: " + webRequest.error);
            else
                StartCoroutine(GetData(uri_getData));
        }
    }
    
    IEnumerator GetData(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                Debug.Log("Error: " + webRequest.error);
            else
            {
                // USE DATA
                ParseData(webRequest.downloadHandler.text);

                // WAIT & RUN AGAIN
                yield return new WaitForSeconds(refreshTime);
                StartCoroutine(GetData(uri_getData));
            }
        }
    }

    void ParseData(string rawData)
    {
        JSONObject jsObj = (JSONObject) JSON.Parse(rawData);

        if (cryptoList.Count == 0)
            for (int i = 0; i < jsObj["ethereum"].Count; i++)
            {
                crypto c = new crypto();
                c.name = currenties[i];
                c.value = jsObj["ethereum"][i];
                cryptoList.Add(c);
            }
        else
            for (int i = 0; i < jsObj["ethereum"].Count; i++)
            {
                cryptoList[i].name = currenties[i];
                cryptoList[i].value = jsObj["ethereum"][i];
            }
        
        if (bdd.nftList.Count == 0)
            bdd.StartInitDatabase();
    }

    public void GetData()
    {
        // --- START BRINGING COINGECKO'S DATA
        StartCoroutine(Ping(uri_ping));
    }
}
