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

    private List<int> datosGrafica;

    private GameObject graphicObjects;
    private float yMaximum;

    public static Window_Graph window_Graph;

    private void Awake() {
        window_Graph = this;
        graphicObjects = new GameObject("graphicObjects");
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        graphicObjects.transform.SetParent(graphContainer,false);
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        
    }

    private GameObject CreateCircle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphicObjects.transform,false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(6, 6);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<int> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
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
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.transform.SetParent(graphicObjects.transform,false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i*10);
            labelX.GetComponent<Text>().fontSize = 15;

            
            
            RectTransform dashX = Instantiate(dashTemplateY);
            dashX.transform.SetParent(graphicObjects.transform,false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, 200);
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++) {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.transform.SetParent(graphicObjects.transform,false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-14f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(normalizedValue * yMaximum);
            labelY.GetComponent<Text>().fontSize = 15;
            
            RectTransform dashY = Instantiate(dashTemplateX);
            dashY.transform.SetParent(graphicObjects.transform,false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(340, normalizedValue * graphHeight);
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphicObjects.transform,false);
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


    public void setvalueList(List<int> datosGrafica,int año)  
    {

        this.datosGrafica = datosGrafica;
        setyMaximum();
        ShowGraph(datosGrafica, (int _i) => ""+(_i+año), (float _f) => Mathf.RoundToInt(_f).ToString());
        
    }

    public void setyMaximum()
    {
        yMaximum = datosGrafica[0];

        for(int i=0;i<datosGrafica.Count-1;i++)
        {
            if(datosGrafica[i] < datosGrafica[i+1])
            {
                yMaximum = datosGrafica[i+1] + 100;
            }
        }
    }

    public void cleanGraphic()
    {
        Destroy(graphicObjects);
        graphicObjects = new GameObject("graphicObjects");
        graphicObjects.transform.SetParent(graphContainer,false);
        
    }
}

