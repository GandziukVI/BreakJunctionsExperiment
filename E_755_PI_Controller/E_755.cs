using Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Ports;
using System.Globalization;

namespace E_755_PI_Controller
{
    public class E_755
    {
        #region E_755 settings

        private COM_Device _COM_Device;
        private IExperimentalDevice _TheDevice;

        #endregion

        #region Constructor

        public E_755(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;
            _COM_Device = __TheDevice as COM_Device;

            _COM_Device.COM_Port.DataReceived += COM_Port_DataReceived;
        }

        void COM_Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var motionPort = sender as SerialPort;
            var responce = motionPort.ReadExisting();
        }

        #endregion

        #region Functionality

        public void RequestMotionStatus()
        {
            _TheDevice.SendCommandRequest("#5");
        }

        public string GetReadyStatus()
        {
            return _TheDevice.RequestQuery("#7");
        }

        public void StopAllAxes()
        {
            _TheDevice.SendCommandRequest("#24");
        }

        public void GetID()
        {
            _TheDevice.SendCommandRequest("*IDN?");
        }

        public void AutoPiezoGainCalibration(GainIDs __GainID)
        {
            var command = String.Format("APG [{0}]", (int)__GainID);
            _TheDevice.SendCommandRequest(command);
        }

        public void ReadAutoPiezoGainCalibrationState(GainIDs __GainID)
        {
            var command = String.Format("APG? [{0}]", (int)__GainID);
            _TheDevice.SendCommandRequest(command);
        }

        public void SetBaudRate(BaudRates __BaudRate)
        {
            var command = String.Format("BDR {0}", (int)__BaudRate);
            _TheDevice.SendCommandRequest(command);
        }

        public BaudRates GetBaudRate()
        {
            var query = "BDR?";
            var responce = 57600;

            int.TryParse(_TheDevice.RequestQuery(query), out responce);
            return (BaudRates)responce;
        }

        public void ChangeActiveCommandLevel(CommandLevels __CommandLevel)
        {
            var command = "";
            switch (__CommandLevel)
            {
                case CommandLevels.Default:
                    {
                        command = String.Format("CCL {0}", (int)__CommandLevel);
                        _TheDevice.SendCommandRequest(command);
                    } break;
                case CommandLevels.Advanced:
                    {
                        command = String.Format("CCL {0} [{1}]", (int)__CommandLevel, "advanced");
                        _TheDevice.SendCommandRequest(command);
                    } break;
                case CommandLevels.Highest:
                    throw new Exception("Nott allover for usual user!");
                default:
                    {
                        command = String.Format("CCL {0}", (int)__CommandLevel);
                        _TheDevice.SendCommandRequest(command);
                    } break;
            }
        }

        public CommandLevels GetActiveCommandLevel()
        {
            var query = "CCL?";
            var responce = 0;

            int.TryParse(_TheDevice.RequestQuery(query), out responce);
            return (CommandLevels)responce;
        }

        public void ClearAxisStatus(AxisIdentifier __AxisID)
        {
            var command = String.Format("CLR [{0}]", (int)__AxisID);
            _TheDevice.SendCommandRequest(command);
        }

        public string GetAssignmentOfStagesToAxes(AxisIdentifier __AxesID)
        {
            var query = String.Format("CST? {0}", (int)__AxesID);
            return _TheDevice.RequestQuery(query);
        }

        public string GetCommandSyntaxVersion()
        {
            var query = "CSV?";
            return _TheDevice.RequestQuery(query);
        }

        public void SetDaisyChainConfiguration(ProtocolType __ProtocolType, ProtocolOptions __ProtocolOptions)
        {
            var command = String.Format("DCC {0} {1}", (int)__ProtocolType, (int)__ProtocolOptions);
            _TheDevice.SendCommandRequest(command);
        }

        public string GetDaisyChainConfiguration(ProtocolType __ProtocolType)
        {
            var query = String.Format("DCC [{0}]", (int)__ProtocolType);
            return _TheDevice.RequestQuery(query);
        }

        public void DefineHome(AxisIdentifier __AxisID)
        {
            var command = String.Format("DFH [{0}]", (int)__AxisID);
            _TheDevice.SendCommandRequest(command);
        }

        public string GetHomePositions(AxisIdentifier __AxisID)
        {
            var query = String.Format("DFH? [{0}]", (int)__AxisID);
            return _TheDevice.RequestQuery(query);
        }

        //===========================================================================================================================

        public void FastMoveToNegativeLimit(AxisIdentifier __AxisID)
        {
            var command = String.Format("FNL [{0}]", (int)__AxisID);
            _TheDevice.SendCommandRequest(command);
        }

        public void FastMoveToPositiveLimit(AxisIdentifier __AxisID)
        {
            var command = String.Format("FPL [{0}]", (int)__AxisID);
            _TheDevice.SendCommandRequest(command);
        }

        public void GoHome(AxisIdentifier __AxisID)
        {
            var command = String.Format("GOH [{0}]", (int)__AxisID);
            _TheDevice.SendCommandRequest(command);
        }

        public void SetServoControlMode(AxisIdentifier __AxisID, ServoControlModes __State)
        {
            var command = String.Format("SVO {0} {1}", (int)__AxisID, (int)__State);
            _TheDevice.SendCommandRequest(command);
        }

        public void MoveAbsolute(AxisIdentifier __AxisID, double __Position)
        {
            var command = String.Format("MOV {0} {1}", (int)__AxisID, __Position.ToString(NumberFormatInfo.InvariantInfo));
            _TheDevice.SendCommandRequest(command);
        }

        public void GetTargetPosition(AxisIdentifier __AxisID)
        {
            var command = String.Format("MOV? {0}", (int)__AxisID);
            _TheDevice.SendCommandRequest(command);
        }

        public void GetOnTargetStatus(AxisIdentifier __AxisID)
        {
            var command = String.Format("ONT? {0}", (int)__AxisID);
            _TheDevice.SendCommandRequest(command);
        }

        public void GetCurrentPosition(AxisIdentifier __AxisID)
        {
            var command = String.Format("POS? {0}", (int)__AxisID);
            _TheDevice.SendCommandRequest(command);
        }

        #endregion
    }
}
