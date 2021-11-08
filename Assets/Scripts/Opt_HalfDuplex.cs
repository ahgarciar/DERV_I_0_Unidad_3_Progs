using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UnityEngine;

public class Opt_HalfDuplex : MonoBehaviour
{
    SerialPort arduino;

    [SerializeField]
    TextMeshProUGUI estado;
    [SerializeField]
    TextMeshProUGUI txt_boton;

    [SerializeField]
    bool estadoLed = false;


    [SerializeField]
    public int valSensor1;
    [SerializeField]
    public int valSensor2;
    [SerializeField]
    public int valSensor3;

    [SerializeField]
    [TextArea(5, 10)]
    public string valor = "";

    public void conectar(string ncom) {         

        if (arduino == null)  //conectar
        {
            arduino = new SerialPort("COM" + ncom, 9600);  //Ej: COM2
            arduino.ReadTimeout = 100; //100ms
            arduino.Open();
            estado.text = "CONECTADO";
            txt_boton.text = "DESCONECTAR";
        }
        else if (!arduino.IsOpen)  //reconectar
        {
            arduino.Open();
            estado.text = "RECONECTADO";
            txt_boton.text = "DESCONECTAR";
        }
        else {  //desconectar
            arduino.Close();
            estado.text = "DESCONECTADO";
            txt_boton.text = "RECONECTAR";
        }

    }



    public void escribir_datos() {

        if (arduino != null)
        {
            if (arduino.IsOpen)
            {
                if (estadoLed)
                {
                    arduino.WriteLine("1"); //envia 3 caracteres ... 1\n\r 
                }
                else {
                    arduino.WriteLine("0"); //   0\n\r
                }
                estadoLed = !estadoLed;
            }
        }
    }


    public void leer_datos() {
        StopAllCoroutines();
        StartCoroutine("leer_datos_arduino");
    }

    IEnumerator leer_datos_arduino() {
        while (true)
        {
            if (arduino !=null)
            {
                if (arduino.IsOpen)
                {
                    valor += arduino.ReadExisting(); 
                    //Debug.Log(valor);
                    //H768R266R809T

                    //CUIDADO CUANDO UNA TRAMA SE LEE DESDE EL COMIENZO TRUNCADA! 

                    if (valor[0] == 'H') /// AQUI SE SOLUCIONAR�A LO DE CUIDADO
                    {
                        //Debug.Log("la Trama inicia correctamente");
                        if (valor[valor.Length-1] == 'T')
                        {
                            Debug.Log("La Trama esta completa!");

                            //1.- quitar una trama de la cola

                            //TAREA INDIVIDUAL PARA EL 9 DE NOVIEMBRE  
                            // :  IDEAR Y PROBAR UN ALGORITMO QUE PERMITA QUITAR UNA TRAMA DE
                            //          UNA CONJUNTO(COLA) DE TRAMAS

                            //2.-procesarla

                            //PROCESAMIENTO:
                            /*
                            string temp = valor.Substring(1, valor.IndexOf('R')-1);
                            valSensor1 = Convert.ToInt32(temp); 
                            Debug.Log(temp);

                            //Trama sin el valor del primer sensor
                            temp = valor.Substring(valor.IndexOf('R') + 1); 
                            Debug.Log(temp);

                            /////////////////////////////////

                            //obtenci�n del valor del segundo sendor
                            string aux;   //                         
                            aux = temp.Substring(0, temp.IndexOf('R'));
                            valSensor2 = Convert.ToInt32(aux);
                            Debug.Log(aux);

                            //trama sin el valor del segundo sensor
                            temp = temp.Substring(temp.IndexOf('R') + 1);
                            Debug.Log(temp);

                            //obtencion del valor del tercer sensor
                            temp = temp.Substring(0, temp.Length - 1);
                            valSensor3 = Convert.ToInt32(temp);

                            /////////////////////////////
                            ///  
                            */


                        }
                    }///

                }
            }
            yield return new WaitForSeconds(.01f);
        }
    }

}
