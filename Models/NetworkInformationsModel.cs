using System;
using System.Collections.Generic;

namespace DeviceSystemRepository.Models
{
    internal class NetworkInformationsModel : IEquatable<NetworkInformationsModel>
    {
        public required string HostName { get; set; }
        public required string IPAdress { get; set; }
        public required string MacAdress { get; set; }
        public required string ConnectionType { get; set; }

        public bool Equals(NetworkInformationsModel other)
        {
            if (other == null)
                return false;

            return HostName == other.HostName &&
                   IPAdress == other.IPAdress &&
                   MacAdress == other.MacAdress &&
                   ConnectionType == other.ConnectionType;
        }

        public override bool Equals(object obj) => Equals(obj as NetworkInformationsModel);

        public override int GetHashCode()
        {
            return HashCode.Combine(HostName, IPAdress, MacAdress, ConnectionType);
        }
    }
}
