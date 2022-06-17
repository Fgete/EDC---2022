using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [Header("Inputs")]
    public InputField inputLogin;
    public InputField inputPassword;

    [Header("Manipulated")]
    public GameObject loginScreen;
    public GameObject dashboardScreen;
    public Text feedback;

    public void Trigger()
    {
        // --- BREAK IF MISSING INPUT
        if (!inputLogin || !inputPassword) return;
        
        // --- GET BDD MANAGER
        SqliteSetBdd bdd = FindObjectOfType<SqliteSetBdd>();
        
        // --- TRY TO LOG
        if (bdd.GetUserByLogin(inputLogin.text, inputPassword.text))
        {
            if (!loginScreen || !dashboardScreen)
                return;
            loginScreen.SetActive(false);
            dashboardScreen.SetActive(true);
            dashboardScreen.GetComponent<Dashboard>().UpdateUserData();
        }
        else
        {
            if (!feedback)
                return;
            feedback.text = "LOGIN or PASSWORD not corresponding...";
        }
    }
}
