using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UsernameController : MonoBehaviour
{
    private Scene scene;


    public Text txtNombre;

    public GameObject alerta;


    bool partidaExistente;

    double timer;

    void Start()
    {
        timer = 0;
        scene = SceneManager.GetActiveScene();

    }

    void Update()
    {
        if(alerta.activeSelf == true)
        {
            timer += Time.deltaTime;
        }

        if(timer > 4)
        {
            timer = 0;
            alerta.SetActive(false);
        }
    }

    public void SiguienteAction()
    {
        if(txtNombre.text != "" && txtNombre.text.Length > 3)
        {
            PlayerData.playerData.saveUsername(txtNombre.text);
            SceneManager.LoadScene(scene.buildIndex+1);
        }
        else{

                alerta.SetActive(true);
        }

    
    }

    public void VolverAction()
    {
        SceneManager.LoadScene(scene.buildIndex-1);
    }


}

        
    