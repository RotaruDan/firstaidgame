using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class Flags
{

    private static Dictionary<string, Boolean> flags = new Dictionary<string, bool>();

    public static void Reininciar()
    {
        Agregar("AT", false);
        Agregar("Ayuda", false);
        Agregar("BotonIntermitente", false);
        Agregar("ComproboRespiracion", false);
        Agregar("DT", false);
        Agregar("DefibriladorAbierto", false);
        Agregar("DesfibriladorColocado", false);
        Agregar("ElectrodosConectados", false);
        Agregar("Estimulado", false);
        Agregar("HablandoTelefono", false);
        Agregar("INC", false);
        Agregar("InicioTos", false);
        Agregar("Llamo112", false);
        Agregar("LuzIntermitente", false);
        Agregar("PosicionDeSeguridad", false);
        Agregar("ProponeEsperar", false);
        Agregar("RealizoHeimlich", false);
        Agregar("Respira", false);
        Agregar("Sentado", false);
        Agregar("TelEscondido", false);
        Agregar("VieneAmbulancia", false);        
    }


    public static void IniciarAleatoriedad()
    {
        Agregar("OrdenOpciones1", UnityEngine.Random.value > 0.5f);
        Agregar("OrdenOpciones2", UnityEngine.Random.value > 0.5f);
        Agregar("Desfibrilador", UnityEngine.Random.value > 0.5f);
    }

    public static bool ValorDe(string flag)
    {
        Boolean temp = false;
        if (flags.TryGetValue(flag, out temp))
        {
            return temp;
        }
        else {
            return false;
        }
    }

    public static void Agregar(string flag, bool valor)
    {
        flags.Add(flag, valor);
    }
}
