﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaPresentacion.Utilidades
{
    public class OpcionCombo
    {
        public object Valor { get; set; }
        public string Texto { get; set; }

        public OpcionCombo(object valor, string texto)
        {
            Valor = valor;
            Texto = texto;
        }

        public OpcionCombo()
        {
        }
    }
}
