using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaySceneController : MonoBehaviour
{
    private Scene scene;
    public GameObject mapa, estadisticas, tienda, info;
    public TextAsset ChinaTxt, EstadosuTxt, AlemaniaTxt, ArabiasTxt, BrasilTxt;
    public bool isShowingMapa, isShowingEstadisticas, isShowingTienda, isShowingInfo;

    public Text txtUserName;

    // Start is called before the first frame update
    void Start()
    {
        txtUserName.text = PlayerData.playerData.getUsername();
        scene = SceneManager.GetActiveScene();

         
    }

    // Update is called once per frame

    public void VolverAction(){
        SceneManager.LoadScene(scene.buildIndex-2);
        ResetearDatosEstadisticas(); /*Se cancela toda simulacion existente al volver al menu principal*/
    }

    public void SalirAction(){
        Application.Quit();
    }

    public void MapaAction(){
        ResetearDatosEstadisticas(); /*Se cancela toda simulacion existente al cambiar de pestaña*/
        isShowingEstadisticas= false;
        estadisticas.SetActive(isShowingEstadisticas);
        isShowingTienda = false;
        tienda.SetActive(isShowingTienda);
        isShowingMapa = true;
        mapa.SetActive(isShowingMapa);
    }

    public void EstadisticasAction(){
         
        isShowingMapa = false;
        mapa.SetActive(isShowingMapa);
        isShowingTienda = false;
        tienda.SetActive(isShowingTienda);
        isShowingEstadisticas = true;
        estadisticas.SetActive(isShowingEstadisticas);
    }

    public void TiendaAction(){
        ResetearDatosEstadisticas(); /*Se cancela toda simulacion existente al cambiar de pestaña*/
        isShowingMapa = false;
        mapa.SetActive(isShowingMapa);
        isShowingEstadisticas= false;
        estadisticas.SetActive(isShowingEstadisticas);
        isShowingTienda = true;
        tienda.SetActive(isShowingTienda);
    }


    public void ChinaAction(){
        isShowingInfo = true;
        info.GetComponent<UnityEngine.UI.Text>().text = ChinaTxt.text;
        info.SetActive(isShowingInfo);
    }

    public void EstadosuAction(){
        isShowingInfo = true;
        info.GetComponent<UnityEngine.UI.Text>().text = EstadosuTxt.text;
        info.SetActive(isShowingInfo);
    }

    public void AlemaniaAction(){
        isShowingInfo = true;
        info.GetComponent<UnityEngine.UI.Text>().text = AlemaniaTxt.text;
        info.SetActive(isShowingInfo);
    }

    public void ArabiasAction(){
        isShowingInfo = true;
        info.GetComponent<UnityEngine.UI.Text>().text = ArabiasTxt.text;
        info.SetActive(isShowingInfo);
    }

    public void BrasilAction(){
        isShowingInfo = true;
        info.GetComponent<UnityEngine.UI.Text>().text = BrasilTxt.text;
        info.SetActive(isShowingInfo);
    }

    public void BackAction(){
        ResetearDatosEstadisticas();
        isShowingInfo = false;
        info.SetActive(isShowingInfo);
    }

    private void ResetearDatosEstadisticas()
    {
        /*Si estadisticas se encuentra desactivado, no tiene sentido resetear datos,
        puesto que no se ha ejecutado ninguna simulacion*/
        if(isShowingEstadisticas==true) 
        {
            EstadisticasController.estadisticasController.ResetearDatos();
        }
    }
}
