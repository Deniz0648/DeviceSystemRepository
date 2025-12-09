using System;

namespace DeviceSystemRepository.Models
{
    internal class NetworkInformationsModel : IEquatable<NetworkInformationsModel>
    {
        public required string HostName { get; set; }
        public required string IPAdress { get; set; }
        public required string MacAdress { get; set; }
        public required string ConnectionType { get; set; }

        // IEquatable interface implementation
        public bool Equals(NetworkInformationsModel? other)
        {
            if (other is null)
                return false;

            return HostName == other.HostName &&
                   IPAdress == other.IPAdress &&
                   MacAdress == other.MacAdress &&
                   ConnectionType == other.ConnectionType;
        }

        // Override Equals for object
        public override bool Equals(object? obj)
        {
            return obj is NetworkInformationsModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HostName, IPAdress, MacAdress, ConnectionType);
        }
    }
}
