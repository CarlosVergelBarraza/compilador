using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiador.Typos
{
    public enum TokenType
    {
        TipoDato,
        Identificador,
        Entero,
        Cadena,
        Igual,
        Suma,
        Resta,
        Multiplicacion,
        Division,
        ParentesisIzquierdo,
        ParentesisDerecho,
        PuntoComa,
        FinDeLinea
    }
}
