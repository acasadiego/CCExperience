using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UsernameController : MonoBehaviour
{
    private Scene scene;

    //Se crea un objeto de la clase como público y estatico, para que pueda ser accedido al buscarlo en la PlayScene.
    public static UsernameController usernameController;
    public Text txt_Nombre;
    string userName;
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        
        /*Guarda los valores de la clase para que no se destruyan al cambiar a la PlayScene. Destruye el nuevo objeto de la clase que
        se crea en la PlayScene para que no haya conflictos con el actual*/

        if(usernameController == null)
        {
            usernameController = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
     
     /*Esta linea solo funciona en la PlayScene, pero no en la UserNameScene, debido a que no existe un gameObject "txt_Username"
     en ella. Por lo anterior se utiliza un Try Catch. Esta linea se usa para poner el nombre de usuario que se ingreso en la UserNameScene en el UIText correspondiente de 
     la PlayScene*/
     try{txt_Nombre = GameObject.Find("txt_Username").GetComponent<Text>(); txt_Nombre.text = userName;}
     catch{}
        
    }

    public void SiguienteAction()
    {
        userName = txt_Nombre.text;
        SceneManager.LoadScene(scene.buildIndex+1);
    }

 

  
        
    


}
