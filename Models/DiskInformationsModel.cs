using System;
using System.Collections.Generic;

namespace DeviceSystemRepository.Models
{
    internal class DiskInformationsModel : IEquatable<DiskInformationsModel>
    {
        public required string HostName { get; set; }
        public int DiskNumber { get; set; }
        public required string Status { get; set; }
        public long DiskCapacity { get; set; }
        public required string MediaType { get; set; }

        public bool Equals(DiskInformationsModel other)
        {
            if (other == null)
                return false;

            return HostName == other.HostName &&
                   DiskNumber == other.DiskNumber &&
                   Status == other.Status &&
                   DiskCapacity == other.DiskCapacity &&
                   MediaType == other.MediaType;
        }

        public override bool Equals(object obj) => Equals(obj as DiskInformationsModel);

        public override int GetHashCode()
        {
            return HashCode.Combine(HostName, DiskNumber, Status, DiskCapacity, MediaType);
        }
    }
}
