using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSystemRepository.Models
{
    public class MonitorInformationsModel 
    {
        public required string HostName { get; set; }
        public required string ManufacturerName { get; set; }
        public required string UserFriendlyName { get; set; }
        public required string SerialNumberID { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj is MonitorInformationsModel other)
            {
                return HostName == other.HostName &&
                       ManufacturerName == other.ManufacturerName &&
                       UserFriendlyName == other.UserFriendlyName &&
                       SerialNumberID == other.SerialNumberID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            // HashCode'i doğru bir şekilde oluşturmayı unutmayın
            return HashCode.Combine(HostName, ManufacturerName, SerialNumberID, UserFriendlyName);
        }
    }
}
