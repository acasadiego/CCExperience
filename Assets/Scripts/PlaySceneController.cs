using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneController : MonoBehaviour
{
    private Scene scene;
    public GameObject mapa, estadisticas, tienda;
    public bool isShowingMapa, isShowingEstadisticas, isShowingTienda;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolverAction(){
        SceneManager.LoadScene(scene.buildIndex-2);
    }

    public void SalirAction(){
        Application.Quit();
    }

    public void MapaAction(){
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
        isShowingMapa = false;
        mapa.SetActive(isShowingMapa);
        isShowingEstadisticas= false;
        estadisticas.SetActive(isShowingEstadisticas);
        isShowingTienda = true;
        tienda.SetActive(isShowingTienda);
    }

    public void SaltarAction(){
        
    }
}
