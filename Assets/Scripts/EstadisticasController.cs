﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EstadisticasController : MonoBehaviour
{

    public static EstadisticasController estadisticasController;

    //Declaracion de niveles, flujos y variables auxiliares del modelo como variables de C#.


    //Variables correspondientes a text.
    public Text txtCo2Atmosfera,txtYear,txtPoblacion,txtCo2Ant,txtArboles, txtEjeY;
    public InputField inpYearSimulacion;


//Variables del modelo dinamico sistemico (Sujetas a simulacion)
    private double year,yearSimulacion,trillon,billon,millon;
    private double poblacion,muertes,nacimientos,tasaDeNatalidad,tasaDeMortalidad,efectoSobreMuertes,capacidadCargaMuertes;
    private double emisionCo2Ant,industria,tasaIndustrial,trabajadorIndustria,industriaPersonas,co2Industria,co2Persona,tasaCompra,vehiculoCombustible,co2Vehiculo,plasticoQuemado,tasaQuema,co2PlasticoQuemado,co2AntAtm,tasaCo2AntAtm;

    private double co2Atmosfera,co2AH2CO3,absorcionCO2PlanMar,co2AC,co2CapturadoArbol;

    private double cLitosfera,cACO2,cO2ExpSuperficie;

    private double h2CO3Hidrosfera,h2CO3ACO2,tasaCO2ExpAgua;

    private double arboles,nacimientosArb,tasaNatalidadArb,tasaPlantacion,plantacion,tasaTala,capacidadCargaIncendios,tasaIncendios,deforestacion;


    private bool simulacion; //Variable que indica cuando se está realizando una simulación, para que el TEXT del año actual no se vea alterado por ello.
    private int lapsoSimulacionGrafica, espacioAños; /*Variable "espacioAños" : Espacio entre años del eje X de 
            la grafica (Ej: la grafica solo mostrará años de 10 en 10, luego espacioAños = 10)*/

    private string variableGrafica,ultimoAñoSimulacion;

    


//Lista donde se almacenan los datos que se mostrarán en la gráfica.
     private List<int> datosGrafica;



//Variables del modelo dinamico sistemico (Que se encargan de almacenar los valores del AÑO ACTUAL)
 private double yearACT, poblacionACT, co2AtmosferaACT, muertesACT, nacimientosACT, industriaACT, vehiculoCombustibleACT, plasticoQuemadoACT,
                    emisionCo2AntACT, co2AntAtmACT, arbolesACT, nacimientosArbACT, plantacionACT, deforestacionACT, cLitosferaACT, h2CO3HidrosferaACT, co2ACACT,
                        cACO2ACT, co2AH2CO3ACT, h2CO3ACO2ACT;

    void Start()
    {
        estadisticasController = this;
        this.trillon = 1000000000000;
        this.billon = 1000000000;
        this.millon = 1000000;
        this.simulacion = false;
        this.lapsoSimulacionGrafica = 100;
        this.espacioAños = 10;
        variableGrafica = "co2Atmosfera";


        InicializarVariablesModelo();
        MantenerDatosActuales();
        ActualizarGrafica();
        ActualizarValores();
        

    }

    void Update()
    {
        if(PlaySceneController.playSceneController.getAñosAvanzados() > 0) /*Averigua si se avanzaron años mientras el objeto estadisticas estaba desactivado, para luego actualizar los valores*/
        {
            int añosAvanzados = PlaySceneController.playSceneController.getAñosAvanzados();

            for(int i = 0; i<añosAvanzados; i++)
            {
                AvanzarAñoActual();
            }

            PlaySceneController.playSceneController.ResetearAñosAvanzados();
        }
    }
    private void InicializarVariablesModelo()  //Valores iniciales de las variables del modelo para el año 2020.
    {

       
        year = 2020;
        yearSimulacion = year;
        poblacion = 7500000000;
        co2Atmosfera = 224000000000000;
        capacidadCargaMuertes = 324000000000000;
        capacidadCargaIncendios = 224000000000000;
        tasaDeNatalidad = 15;
        tasaDeMortalidad= 7.8;
        EfectoMuertes();
        muertes = (tasaDeMortalidad*poblacion*efectoSobreMuertes)/1000;
        nacimientos = (tasaDeNatalidad*poblacion)/1000;
        emisionCo2Ant = 0;
        tasaIndustrial = 0.02;
        trabajadorIndustria = 80;
        industriaPersonas = tasaIndustrial/trabajadorIndustria;
        industria = industriaPersonas * poblacion;
        co2Industria = 1000000;
        co2Persona = 0.328;
        tasaCompra = 0.13;
        vehiculoCombustible = poblacion*tasaCompra;
        co2Vehiculo = 2.336;
        tasaQuema = 0.21;
        plasticoQuemado = poblacion*tasaQuema;
        co2PlasticoQuemado = 0.0086;
        tasaCo2AntAtm = 0.98;
        co2AntAtm = emisionCo2Ant * tasaCo2AntAtm;
        arboles= 3000000000000;
        tasaNatalidadArb = 0.0009;
        tasaPlantacion = 0.213;
        nacimientosArb=arboles*tasaNatalidadArb;
        plantacion = 0;
        tasaTala = 0.315;
        TasaIncendios();
        deforestacion = poblacion*tasaTala*tasaIncendios;
        absorcionCO2PlanMar = 0.001;
        co2AH2CO3 = co2Atmosfera*absorcionCO2PlanMar;
        co2CapturadoArbol = 0.7;
        co2AC = co2CapturadoArbol*arboles;
        cLitosfera = 560000000000000000;
        cO2ExpSuperficie=0.000001;
        cACO2= cLitosfera*cO2ExpSuperficie;
        h2CO3Hidrosfera = 5600000000000000;
        tasaCO2ExpAgua = 0.000001;
        h2CO3ACO2 = h2CO3Hidrosfera*tasaCO2ExpAgua;
    }

    private void ActualizarGrafica() //Actualiza la gráfica de la variable seleccionada.
    {
   

        datosGrafica = new List<int>();
        int yearGrafica = (int)year; //Se guarda la variable "año" en una auxiliar "año grafica", ya que la primera cambiará en las siguientes lineas debido a la simulación.
        double cantidad = DeterminarUnidad(DeterminarVariableModelo()); 

        for(int i = 0;i<lapsoSimulacionGrafica;i++)
        {
            AvanzarAño();

            if(i%espacioAños==0)
            { 
              datosGrafica.Add((int)(DeterminarVariableModelo()/cantidad));
            }

        }

        string texto = "Toneladas";

        switch(variableGrafica)
        {
            case "poblacion":
                texto = "Personas";
                break;
            case "arboles":        
                texto = "Arboles";
                break;
        }
        

        txtEjeY.text = texto + " (" + DeterminarUnidadTexto(cantidad) + ")";
        Window_Graph.window_Graph.cleanGraphic();
        Window_Graph.window_Graph.setvalueList(datosGrafica,yearGrafica); /* Se envia la variable año grafica, la cual representa el año inicial de la simulación en la grafica*/
        datosGrafica.Clear();

        RecuperarDatosActuales(); //Se reestablecen los datos actuales luego de graficar 
        

         
    }

    public void VerResultadosAction() //Muestra en la escena una simulación de las variables en un año especifico.
    {
        
        
        datosGrafica = new List<int>();
        MantenerDatosActuales();
        
        //¡MOMENTANEO! (Cambiar luego para mejora del código) Se hace un try catch en caso de que el usuario ingrese un valor NO númerico en la text box.
        try
        { this.yearSimulacion = double.Parse(inpYearSimulacion.text);}
        catch{ simulacion = false;} //Si el usuario ingreso un NUEVO valor NO númerico en el input field, la simulacion anterior termina
        
        double tiempo = yearSimulacion - yearACT;

        if (tiempo>0) /* Se pone esta condición porque para que se pueda realizar una simulacion, el año ingresado debe ser mayor que el año actual*/
        {
            this.simulacion = true;


            for(int i = 0;i<tiempo;i++)
            {
              AvanzarAño();
              
            }

            
        }
        else
        {   //Si el usuario ingresa un NUEVO valor en el input field equivalente al AÑO actual o menor del mismo, la simulación termina.
            simulacion = false;
        }

        datosGrafica.Clear();
        
        ActualizarValores();
        ActualizarGrafica();

        /*Guarda el ultimo año de simulacion antes de resetear el texto de el input field, para luego utilizarlo
            en caso de que se quiera ver la grafica de otra variable del modelo para el mismo año*/
        ultimoAñoSimulacion = inpYearSimulacion.text;
        inpYearSimulacion.text = "";
        
    }
    

    private void MantenerDatosActuales()  //Guarda los valores de las variables en el AÑO ACTUAL, para que no se pierdan al momento de realizar alguna simulación.
    {

       
        yearACT = year;
        poblacionACT = poblacion;
        co2AtmosferaACT = co2Atmosfera;
        muertesACT = muertes;
        nacimientosACT = nacimientos;
        industriaACT = industria;
        vehiculoCombustibleACT = vehiculoCombustible;
        plasticoQuemadoACT = plasticoQuemado;
        emisionCo2AntACT = emisionCo2Ant;
        co2AntAtmACT = co2AntAtm;
        arbolesACT = arboles;
        nacimientosArbACT = nacimientosArb;
        plantacionACT = plantacion;
        deforestacionACT = deforestacion;
        cLitosferaACT = cLitosfera; 
        h2CO3HidrosferaACT = h2CO3Hidrosfera;
        co2ACACT = co2AC;
        cACO2ACT = cACO2;
        co2AH2CO3ACT = co2AH2CO3;
        h2CO3ACO2ACT = h2CO3ACO2;

        
    }


    private void RecuperarDatosActuales() //Trae de vuelta los valores de las variables en el AÑO ACTUAL que hayan sido alterados por alguna simulación.
    {

        
        year = yearACT;
        poblacion = poblacionACT;
        co2Atmosfera = co2AtmosferaACT;
        muertes = muertesACT;
        nacimientos = nacimientosACT;
        industria = industriaACT;
        vehiculoCombustible = vehiculoCombustibleACT;
        plasticoQuemado = plasticoQuemadoACT;
        emisionCo2Ant = emisionCo2AntACT;
        co2AntAtm = co2AntAtmACT;
        arboles = arbolesACT;
        nacimientosArb = nacimientosArbACT;
        plantacion = plantacionACT;
        deforestacion = deforestacionACT;
        cLitosfera = cLitosferaACT;
        h2CO3Hidrosfera = h2CO3HidrosferaACT;
        co2AC = co2ACACT;
        cACO2 = cACO2ACT;
        co2AH2CO3 = co2AH2CO3ACT;
        h2CO3ACO2 = h2CO3ACO2ACT;

    }
    
    private void AvanzarAñoActual() /*Avanza el año ACTUAL del juego, el cual no puede ser retrocedido*/
    {
        simulacion = false; /*Al avanzar el año actual, se cancela toda simulacion existente*/
        AvanzarAño(); 
        MantenerDatosActuales();
        ActualizarGrafica();
        ActualizarValores(); 
        inpYearSimulacion.placeholder.GetComponent<Text>().text = yearACT.ToString(); /* El placeholder del inputField "Año Simulacion" pasa a contener el texto del año actual. */
        
    }
    
    private void AvanzarAño() /*Avanza un año, este metodo se utilzia tanto para avanzar un año actual como para avanzar un año en la simulacion*/
    {

        //Ecuaciones de los niveles y flujos del modelo.
        year++;
        poblacion = poblacion + nacimientos - muertes;
        co2Atmosfera = co2Atmosfera + cACO2 + co2AntAtm + h2CO3ACO2 - co2AC - co2AH2CO3;
        EfectoMuertes();
        muertes = (tasaDeMortalidad*poblacion*efectoSobreMuertes)/1000;
        nacimientos = (tasaDeNatalidad*poblacion)/1000;
        industria = industriaPersonas * poblacion;
        vehiculoCombustible = poblacion*tasaCompra;
        plasticoQuemado = poblacion*tasaQuema;
        emisionCo2Ant = emisionCo2Ant + (co2Persona*poblacion)+(co2Vehiculo*vehiculoCombustible)+(plasticoQuemado*co2PlasticoQuemado)+(co2Industria*industria)-co2AntAtm;
        co2AntAtm = emisionCo2Ant*tasaCo2AntAtm;
        if(arboles>deforestacion){arboles= arboles + plantacion + nacimientos - deforestacion;}
        else{arboles= arboles + plantacion - nacimientos;}
        TasaIncendios();
        nacimientosArb = tasaNatalidadArb*arboles;
        plantacion = tasaPlantacion*poblacion;
        deforestacion = poblacion * tasaTala * tasaIncendios;
        cLitosfera = cLitosfera + co2AC - cACO2; 
        h2CO3Hidrosfera = h2CO3Hidrosfera + co2AH2CO3 - h2CO3ACO2 ;
        if(co2Atmosfera > co2CapturadoArbol * arboles){co2AC= co2CapturadoArbol * arboles;}
        else{co2AC = 0;}
        cACO2 = cLitosfera * cO2ExpSuperficie;
        co2AH2CO3 = co2Atmosfera * absorcionCO2PlanMar;
        h2CO3ACO2 = h2CO3Hidrosfera * tasaCO2ExpAgua;


    }

    private void ActualizarValores()//Actualiza los TEXT de las estadisticas en la escena.
    {

        

        double cantidad = 1;
        string unidad = "";

        cantidad = DeterminarUnidad(co2Atmosfera);
        unidad = DeterminarUnidadTexto(cantidad);
        txtCo2Atmosfera.text = "Toneladas de CO2 en la atmosfera: " + (float)(co2Atmosfera/cantidad) + unidad;

        cantidad = DeterminarUnidad(poblacion);
        unidad = DeterminarUnidadTexto(cantidad);
        txtPoblacion.text = "Poblacion: " + (float)(poblacion/cantidad) + unidad;

        cantidad = DeterminarUnidad(emisionCo2Ant);
        unidad = DeterminarUnidadTexto(cantidad);
        txtCo2Ant.text = "Toneladas de CO2 generado por humanos: " + (float)(emisionCo2Ant/cantidad) + unidad;

        cantidad = DeterminarUnidad(arboles);
        unidad = DeterminarUnidadTexto(cantidad);
        txtArboles.text = "Arboles: " + (float)(arboles/cantidad) + unidad;
        
    }

    private double DeterminarUnidad(double valor) /* Determina la unidad (millon, billon o trillon) de un numero pasado por parametro, y devuelve el valor de dicha unidad*/
    {

        //Determina la unidad (millon, billón o trillón) en DOUBLE de un valor pasado por parametro.
        double cantidad = 1;

        if(valor >= millon && valor <billon )
        {
            cantidad = millon;
        }
        else if(valor >= billon && valor<trillon)
        {
            cantidad = billon;
        }
        else if(valor >= trillon)
        {
            cantidad = trillon;
        }

        return cantidad;
    }

    private string DeterminarUnidadTexto(double cantidad)/*Determina la unidad (millon, billon o trillon) de un numero pasado por parametro, y devuelve el TEXTO del nombre de dicha unidad*/
    {

        //Determina la unidad (millón, billón o trillón) en STRING de un valor pasado por parametro. 
        string unidad = "";

        if(cantidad == millon)
        {
            unidad = " Mill";
        }
        else if (cantidad == billon)
        {
            unidad = " Bill";
        }
        else if (cantidad == trillon)
        {
            unidad = " Trill";
        }
            
        return unidad;
    }
    


    private void EfectoMuertes() //Metodo que representa el comportamiento del Lookup efecto sobre las muertes del modelo.
    {
        double oper = co2Atmosfera / capacidadCargaMuertes;
        
         efectoSobreMuertes = 0.9 + (oper*0.2);
        

    }


  
    private void TasaIncendios() //Metodo que representa el comportamiento del Lookup tasa de incendios forestales del modelo.  
    {
        double oper = co2Atmosfera / capacidadCargaIncendios;
       
        tasaIncendios = 1.05 + (oper*0.05);
        

    }

    public void ResetearDatosAction() //Cancela toda simulacion existente y retorna las variables y las graficas a sus valores del año actual.
    {
        simulacion = false;
        variableGrafica = "co2Atmosfera";
        RecuperarDatosActuales();
        ActualizarGrafica();
        ActualizarValores();

    }

    public void CambiarGraficaCo2AtmAction() //Muestra en la grafica los valores de la variable "Co2Atmosfera"
    {
        variableGrafica = "co2Atmosfera";
        conocerSimulacion();
    }

    public void CambiarGraficaCo2AntAction() //Muestra en la grafica los valores de la variable "emisionCo2Ant"
    {
        variableGrafica = "emisionCo2Ant";
        conocerSimulacion();
    }

    public void CambiarGraficaPoblacionAction() //Muestra en la grafica los valores de la variable "poblacion"
    {
        variableGrafica = "poblacion";
        conocerSimulacion();
    }

    public void CambiarGraficaArbolesAction() //Muestra en la grafica los valores de la variable "arboles"
    {
        variableGrafica = "arboles";
        conocerSimulacion();
    }


    private void conocerSimulacion() /*Metodo para saber si la sección de estadisticas está en modo simulacion o no, para saber qué datos se deben graficar al cambiar de variable*/
    { 

        if(simulacion)
        {
            inpYearSimulacion.text = ultimoAñoSimulacion;
            VerResultadosAction();
        }
        else{
            ActualizarGrafica();
        }
    }

    private double DeterminarVariableModelo() /*Determina que variable del modelo se ha seleccionado actualmente, para saber cual de ellas se debe graficar*/
    {
        double variableModelo = co2Atmosfera;

        switch(variableGrafica)
        {
            case "emisionCo2Ant":
                variableModelo = emisionCo2Ant;
                break;
            case "poblacion":
                variableModelo = poblacion;
                break;
            case "arboles":        
                variableModelo = arboles;
                break;
        }

        return variableModelo;
    }

}


