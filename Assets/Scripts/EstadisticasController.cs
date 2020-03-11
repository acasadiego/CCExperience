using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EstadisticasController : MonoBehaviour
{


    public Text txtCo2Atmosfera;
    public Text txtYear,txtPoblacion,txtCo2Ant,txtArboles;

    double year,trillon,billon;
    double poblacion,muertes,nacimientos,tasaDeNatalidad,tasaDeMortalidad,efectoSobreMuertes,capacidadCargaMuertes;
    double emisionCo2Ant,industria,tasaIndustrial,trabajadorIndustria,industriaPersonas,co2Industria,co2Persona,tasaCompra,vehiculoCombustible,co2Vehiculo,plasticoQuemado,tasaQuema,co2PlasticoQuemado,co2AntAtm,tasaCo2AntAtm;

    double co2Atmosfera,co2AH2CO3,absorcionCO2PlanMar,co2AC,co2CapturadoArbol;

    double cLitosfera,cACO2,cO2ExpSuperficie;

    double h2CO3Hidrosfera,h2CO3ACO2,tasaCO2ExpAgua;

    double arboles,nacimientosArb,tasaNatalidadArb,tasaPlantacion,plantacion,tasaTala,capacidadCargaIncendios,tasaIncendios,deforestacion;
    void Awake()
    {
        trillon = 1000000000000;
        billon = 1000000000;


        InicializarVariablesModelo();
        ActualizarValores();
        

    }

    // Update is called once per frame

    public void InicializarVariablesModelo()
    {
        year = 2020;
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

    public void VerResultados()
    {
        for(int i = 0;i<=2000;i++)
        {
            AvanzarAño();
        }
    }
    public void AvanzarAño()
    {
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
        ActualizarValores();

    }

    void ActualizarValores()
    {
        txtCo2Atmosfera.text = "Toneladas de CO2 en la atmosfera: " + (long)(co2Atmosfera/trillon) + "T";
        txtPoblacion.text = "Poblacion: " + (long)(poblacion/billon) + "B";
        txtCo2Ant.text = "Toneladas de CO2 generado por humanos: " + (long)(emisionCo2Ant/trillon) + "T";
        txtArboles.text = "Arboles: " + (long)(arboles/trillon) + "T";
        txtYear.text = "Año: " + year;
    }
    

    void EfectoMuertes()
    {
        double oper = co2Atmosfera / capacidadCargaMuertes;
        
         efectoSobreMuertes = 0.9 + (oper*0.2);
        

    }

    void TasaIncendios()
    {
        double oper = co2Atmosfera / capacidadCargaIncendios;
       
        tasaIncendios = 1.05 + (oper*0.05);
        

    }
}
