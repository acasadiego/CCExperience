using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    public void JugarAction(){
        SceneManager.LoadScene(scene.buildIndex+1);
    }

    public void CargarAction(){

    }

    public void SobreAction(){
        SceneManager.LoadScene(scene.buildIndex+3);

    }

      public void SalirAction(){
        Application.Quit();
    }
    


}
