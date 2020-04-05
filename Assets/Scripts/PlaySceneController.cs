using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaySceneController : MonoBehaviour
{
    private Scene scene;

    public static PlaySceneController playSceneController;
    public GameObject mapa, estadisticas, tienda, info;
    public TextAsset ChinaTxt, EstadosuTxt, AlemaniaTxt, ArabiasTxt, BrasilTxt;
    private bool isShowingMapa, isShowingEstadisticas, isShowingTienda, isShowingInfo;

    public Text txtUserName, txtAñoActual;

    private int añosAvanzados, añoActual;

    // Start is called before the first frame update
    void Start()
    {
        playSceneController = this;
        añoActual = 2020;
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

    public void AvanzarAñoAction() //Avanza un año, independientemente de si el objeto estadisticas este activado o no.
    {
        añoActual++;
        txtAñoActual.text = "Año: " + añoActual;
        añosAvanzados++;

    }

    public int getAñosAvanzados()
    {
        return añosAvanzados;
    }

    public void ResetearAñosAvanzados() /*Vuelve a 0 los años avanzados una vez han sido actualizados los valores en estadisticas*/
    {
        añosAvanzados = 0;
    }

    private void ResetearDatosEstadisticas()
    {
        /*Si estadisticas se encuentra desactivado, no tiene sentido resetear datos,
        puesto que no se ha ejecutado ninguna simulacion*/
        if(isShowingEstadisticas) 
        {
            EstadisticasController.estadisticasController.ResetearDatosAction();
        }
    }
}
