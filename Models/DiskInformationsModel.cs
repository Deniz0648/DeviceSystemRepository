namespace DeviceSystemRepository.Models
{
    internal class DiskInformationsModel : IEquatable<DiskInformationsModel>
    {
        public required string HostName { get; set; }
        public int DiskNumber { get; set; }
        public required string Status { get; set; }
        public long DiskCapacity { get; set; }
        public required string MediaType { get; set; }

        // Equals metodunun parametre türünü nullable yapıyoruz
        public bool Equals(DiskInformationsModel? other)
        {
            if (other == null)
                return false;

            return HostName == other.HostName &&
                   DiskNumber == other.DiskNumber &&
                   Status == other.Status &&
                   DiskCapacity == other.DiskCapacity &&
                   MediaType == other.MediaType;
        }

        public override bool Equals(object? obj) => obj is DiskInformationsModel other && Equals(other);

        public override int GetHashCode()
        {
            return HashCode.Combine(HostName, DiskNumber, Status, DiskCapacity, MediaType);
        }
    }
}
