using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData playerData;

    void Start()
    {
        playerData = this;
    }
    public void saveUsername(string username)
    {
        PlayerPrefs.SetString("username", username);
    }

    public string getUsername()
    {
        string username = PlayerPrefs.GetString("username", "");

        return username;
    }

    public void deleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }
}
