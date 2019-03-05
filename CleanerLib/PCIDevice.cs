using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;


namespace Cleaner
{
    public class PCIDevice
    {
        public bool Clean;
        public Dictionary<string, string> Parameters;

        public PCIDevice(ManagementObject obj)
        {
            Parameters = new Dictionary<string, string>();

            AddDeviceProperty(obj, "FriendlyName");
            AddDeviceProperty(obj, "HardwareID");
            AddDeviceProperty(obj, "DriverVersion");
            AddDeviceProperty(obj, "DriverDate");
            AddDeviceProperty(obj, "InfName");
            AddDeviceProperty(obj, "Manufacturer");
            AddDeviceProperty(obj, "DriverProviderName");
            AddDeviceProperty(obj, "Signer");
            AddDeviceProperty(obj, "StartMode");
            AddDeviceProperty(obj, "Status");
        }


        public string ID { get { return Parameters["HardwareID"]; } }
        public string Name { get { return Parameters["FriendlyName"]; } }
        public string InfName { get { return Parameters["InfName"]; } }


        private void AddDeviceProperty(ManagementObject obj, string property)
        {
            Parameters.Add(property, Convert.ToString(obj[property]));
        }
    }
}
