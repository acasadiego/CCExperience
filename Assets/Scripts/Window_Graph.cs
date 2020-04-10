using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using CodeMonkey.Utils;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    private List<double> datosGrafica;

    private GameObject graphicObjects;
    private float yMaximum;

    public static Window_Graph window_Graph;

    private void Awake() {
        window_Graph = this;
        graphicObjects = new GameObject("graphicObjects"); /* Se crea un objeto PADRE donde se almacenaran como hijos todos los objetos de la grafica*/
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        graphicObjects.transform.SetParent(graphContainer,false); /* El objeto graphic objects sera hijo de graphContainer*/
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        
    }

    private GameObject CreateCircle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphicObjects.transform,false); /*El circulo creado se hace hijo de graphicObjects*/
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(6, 6);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<double> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        if (getAxisLabelX == null) {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null) {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        float graphHeight = graphContainer.sizeDelta.y;
        float xSize = graphContainer.sizeDelta.x/valueList.Count;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i * xSize;
            float yPosition = (float)((valueList[i] / yMaximum) * graphHeight);
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.transform.SetParent(graphicObjects.transform,false); /*El label del eje X creado se hace hijo de graphicObjects*/
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i*10);
            labelX.GetComponent<Text>().fontSize = 15;

            
            
            RectTransform dashX = Instantiate(dashTemplateY); /*El dash del eje X creado se hace hijo de graphicObjects*/
            dashX.transform.SetParent(graphicObjects.transform,false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, 200);
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++) {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.transform.SetParent(graphicObjects.transform,false); /*El label del eje Y creado se hace hijo de graphicObjects*/
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-23f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = Math.Round((normalizedValue * yMaximum),2).ToString();
            labelY.GetComponent<Text>().fontSize = 15;
            
            RectTransform dashY = Instantiate(dashTemplateX); 
            dashY.transform.SetParent(graphicObjects.transform,false); /*El dash del eje Y creado se hace hijo de graphicObjects*/
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(340, normalizedValue * graphHeight);
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphicObjects.transform,false); /*La conexion entre dos puntos creada se hace hija de graphicObjects*/
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }


    public void setvalueList(List<double> datosGrafica,int año,double mayor)  /*Se inicializa una nueva grafica, pasando sus datos por parametro y el año inicial*/
    {

        this.datosGrafica = datosGrafica;
        yMaximum = (float)mayor;
        ShowGraph(datosGrafica, (int _i) => ""+(_i+año), (float _f) => Mathf.RoundToInt(_f).ToString());
        
    }


    public void cleanGraphic() /*Elimina los elementos de la grafica*/
    {
        Destroy(graphicObjects); /*Se destruye el objeto padre "graphicObjects" que contenia todos los elementos de la grafica, para asi destruirlso tambien*/
        graphicObjects = new GameObject("graphicObjects"); /*Se crea un nuevo objeto padre, que contendrá luego nuevos elementos*/
        graphicObjects.transform.SetParent(graphContainer,false); /*El objeto creado se hace hijo de graphContainer, tal cual como en el metodo "Awake()" de esta misma clase*/
        
    }
}

