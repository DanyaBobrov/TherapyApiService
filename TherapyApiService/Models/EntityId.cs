using TherapyApiService.Heplers;

namespace TherapyApiService.Models
{
    //TODO
    //6.4.3 - проверка коллизии

    public struct EntityId
    {
        private const int IdLenght = 13;

        private readonly byte[] data;
        private char checkSum;

        public byte[] Data => data;
        public char CheckSum => char.ToLower(checkSum);

        public string Id
        {
            get
            {
                var dataAsBase32 = Crockford.ToBase32String(data).ToLower();

                var a = dataAsBase32[0..6];
                var b = dataAsBase32[6..11];
                var c = dataAsBase32[11..16];
                var d = dataAsBase32[16..21];
                return $"{a}-{b}-{c}-{d}{char.ToLower(checkSum)}";
            }
        }

        public EntityId(byte[] payload)
        {
            ArgumentNullException.ThrowIfNull(payload);

            if (payload.Length != IdLenght)
                throw new InvalidOperationException("TODO");

            data = payload;
            checkSum = Crockford.CalculateChecksum(data);
        }

        public static EntityId NewEntityId()
        {
            var buffer = new byte[IdLenght];
            Random.Shared.NextBytes(buffer);
            return new EntityId(buffer);
        }

        public override string ToString() => Id;

        public override bool Equals(object? obj)
        {
            if (obj is not EntityId entityId)
                return false;

            return entityId.ToString() == this.ToString();
        }

        public override int GetHashCode() => data.GetHashCode();

        public static implicit operator string(EntityId id) => id.ToString();
    }
}