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

    private double timer; //Variable que corresponderá al tiempo que durará la alerta de "nombre de usuario erroneo" en escena

    void Start()
    {
        timer = 0;
        scene = SceneManager.GetActiveScene();

    }

    void Update()
    {
        if(alerta.activeSelf == true)
        {
            timer += Time.deltaTime; /*Si la alerta esta activada en escena, empieza a correr el tiempo en segundos*/
        }

        if(timer > 4) /*Cuando hayan pasado 4 segundos, la alerta desaparecerá*/
        {
            timer = 0;
            alerta.SetActive(false);
        }

    
        //Segunda forma de salirse de volver al menu principal
        if (Input.GetKey("escape"))
        {
            VolverAction();
        }

        //Segunda forma de continuar a la escena de juego.
         if (Input.GetKey("return"))
         {
            SiguienteAction();
         }
    
    }

    public void SiguienteAction()
    {
        if(txtNombre.text != "" && txtNombre.text.Length > 3) /*El nombre de usuario debe tener mas de 3 caracteres, de lo contrario, se activará un mensaje de alerta*/
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

        
    