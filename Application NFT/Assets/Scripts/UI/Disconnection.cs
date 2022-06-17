using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Disconnection : MonoBehaviour
{
    private SqliteSetBdd bdd;

    [Header("Manipulated")]
    public GameObject dashboardScreen;
    public GameObject loginScreen;

    public void Trigger()
    {
        bdd = FindObjectOfType<SqliteSetBdd>();

        bdd.ud = new userData();
        dashboardScreen.SetActive(false);
        loginScreen.SetActive(true);
    }
}
