using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData playerData;

    void Awake()
    {
        playerData = this;
    }
    public void saveUsername(string username) //Guarda el nombre de usuario al crear una nueva partida
    {
        PlayerPrefs.SetString("username", username);
    }

    public string getUsername()
    {
        string username = PlayerPrefs.GetString("username", "");

        return username;
    }

    public void deleteAllData() //Elimina todos los datos existentes de la partida
    {
        PlayerPrefs.DeleteAll();
    }
}
