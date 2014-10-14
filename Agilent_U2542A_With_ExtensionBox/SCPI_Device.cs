using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agilent_U2542A_With_ExtensionBox.Interfaces
{
    public interface SCPI_Device
    {
         bool Open();
         bool WriteString(string WhatToWrite);
         string ReadString();
         void Close();
    }
}
