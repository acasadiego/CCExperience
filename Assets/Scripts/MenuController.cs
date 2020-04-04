using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject botonContinuar;
    private Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        
        scene = SceneManager.GetActiveScene();
        ActivarContinuar();

        
    }

    // Update is called once per frame
    void Update()
    {
        //Segunda forma de salirse de la app.
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    public void NuevaPartidaAction(){
        PlayerData.playerData.deleteAllData();
        SceneManager.LoadScene(scene.buildIndex+1);
        
    }

    public void CotinuarAction(){
        SceneManager.LoadScene(scene.buildIndex+2);
    }

    public void SobreAction(){
        SceneManager.LoadScene(scene.buildIndex+3);

    }

      public void SalirAction(){
        Application.Quit();
    }

    private void ActivarContinuar()
    {
        if(PlayerData.playerData.getUsername() == "")
        {
            botonContinuar.SetActive(false);
        }
        else
        {
            botonContinuar.SetActive(true);
        }
    }
    


}
