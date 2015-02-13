using Devices;
using Keithley_4200.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200
{
    public class Keithley_4200
    {
        #region Keithley_4200 settings

        private IExperimentalDevice _TheDevice;

        private Keithley_4200_SMU[] _SMU_Channels;
        public Keithley_4200_SMU[] SMU_Channels { get { return _SMU_Channels; } }

        #endregion

        #region Constructor

        public Keithley_4200(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;
            _TheDevice.InitDevice();

            _SMU_Channels = new Keithley_4200_SMU[] 
            {
                new Keithley_4200_SMU(ref _TheDevice, SMUs.SMU1),
                new Keithley_4200_SMU(ref _TheDevice, SMUs.SMU2),
                new Keithley_4200_SMU(ref _TheDevice, SMUs.SMU3),
                new Keithley_4200_SMU(ref _TheDevice, SMUs.SMU4),
                new Keithley_4200_SMU(ref _TheDevice, SMUs.SMU5),
                new Keithley_4200_SMU(ref _TheDevice, SMUs.SMU6),
                new Keithley_4200_SMU(ref _TheDevice, SMUs.SMU7),
                new Keithley_4200_SMU(ref _TheDevice, SMUs.SMU8)
            };
        }

        #endregion

        #region Functionality

        //public void EnableSMUs(params SMUs[] __SelectedSMUs)
        //{
        //    foreach (var smu in __SelectedSMUs)
        //    {
        //        if (smu == SMUs.SMU1)
        //            _SMU_Channels[0].InitDevice();
        //        else if (smu == SMUs.SMU2)
        //            _SMU_Channels[1].InitDevice();
        //        else if (smu == SMUs.SMU3)
        //            _SMU_Channels[2].InitDevice();
        //        else if (smu == SMUs.SMU4)
        //            _SMU_Channels[3].InitDevice();
        //        else if (smu == SMUs.SMU5)
        //            _SMU_Channels[4].InitDevice();
        //        else if (smu == SMUs.SMU6)
        //            _SMU_Channels[5].InitDevice();
        //        else if (smu == SMUs.SMU7)
        //            _SMU_Channels[6].InitDevice();
        //        else if (smu == SMUs.SMU8)
        //            _SMU_Channels[7].InitDevice();
        //    }
        //}

        #endregion
    }
}
