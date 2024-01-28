using System.Globalization;
using System.Numerics;
using System.Text;

namespace TherapyApiService.Heplers
{
    public static class Crockford
    {
        private static class SymbolCollection
        {
            private record SymbolDefinition(int Value, char EncodeSymbol, char[] DecodeSymbols);

            private readonly static SymbolDefinition[] Items =
            [
                new SymbolDefinition(Value: 0, EncodeSymbol: '0', DecodeSymbols: ['0', 'O', 'o']),
                new SymbolDefinition(Value: 1, EncodeSymbol: '1', DecodeSymbols: ['1', 'I', 'i', 'L', 'l']),
                new SymbolDefinition(Value: 2, EncodeSymbol: '2', DecodeSymbols: ['2']),
                new SymbolDefinition(Value: 3, EncodeSymbol: '3', DecodeSymbols: ['3']),
                new SymbolDefinition(Value: 4, EncodeSymbol: '4', DecodeSymbols: ['4']),
                new SymbolDefinition(Value: 5, EncodeSymbol: '5', DecodeSymbols: ['5']),
                new SymbolDefinition(Value: 6, EncodeSymbol: '6', DecodeSymbols: ['6']),
                new SymbolDefinition(Value: 7, EncodeSymbol: '7', DecodeSymbols: ['7']),
                new SymbolDefinition(Value: 8, EncodeSymbol: '8', DecodeSymbols: ['8']),
                new SymbolDefinition(Value: 9, EncodeSymbol: '9', DecodeSymbols: ['9']),
                new SymbolDefinition(Value: 10, EncodeSymbol: 'A', DecodeSymbols: ['A', 'a']),
                new SymbolDefinition(Value: 11, EncodeSymbol: 'B', DecodeSymbols: ['B', 'b']),
                new SymbolDefinition(Value: 12, EncodeSymbol: 'C', DecodeSymbols: ['C', 'c']),
                new SymbolDefinition(Value: 13, EncodeSymbol: 'D', DecodeSymbols: ['D', 'd']),
                new SymbolDefinition(Value: 14, EncodeSymbol: 'E', DecodeSymbols: ['E', 'e']),
                new SymbolDefinition(Value: 15, EncodeSymbol: 'F', DecodeSymbols: ['F', 'f']),
                new SymbolDefinition(Value: 16, EncodeSymbol: 'G', DecodeSymbols: ['G', 'g']),
                new SymbolDefinition(Value: 17, EncodeSymbol: 'H', DecodeSymbols: ['H', 'h']),
                new SymbolDefinition(Value: 18, EncodeSymbol: 'J', DecodeSymbols: ['J', 'j']),
                new SymbolDefinition(Value: 19, EncodeSymbol: 'K', DecodeSymbols: ['K', 'k']),
                new SymbolDefinition(Value: 20, EncodeSymbol: 'M', DecodeSymbols: ['M', 'm']),
                new SymbolDefinition(Value: 21, EncodeSymbol: 'N', DecodeSymbols: ['N', 'n']),
                new SymbolDefinition(Value: 22, EncodeSymbol: 'P', DecodeSymbols: ['P', 'p']),
                new SymbolDefinition(Value: 23, EncodeSymbol: 'Q', DecodeSymbols: ['Q', 'q']),
                new SymbolDefinition(Value: 24, EncodeSymbol: 'R', DecodeSymbols: ['R', 'r']),
                new SymbolDefinition(Value: 25, EncodeSymbol: 'S', DecodeSymbols: ['S', 's']),
                new SymbolDefinition(Value: 26, EncodeSymbol: 'T', DecodeSymbols: ['T', 't']),
                new SymbolDefinition(Value: 27, EncodeSymbol: 'V', DecodeSymbols: ['V', 'v']),
                new SymbolDefinition(Value: 28, EncodeSymbol: 'W', DecodeSymbols: ['W', 'w']),
                new SymbolDefinition(Value: 29, EncodeSymbol: 'X', DecodeSymbols: ['X', 'x']),
                new SymbolDefinition(Value: 30, EncodeSymbol: 'Y', DecodeSymbols: ['Y', 'y']),
                new SymbolDefinition(Value: 31, EncodeSymbol: 'Z', DecodeSymbols: ['Z', 'z']),
            ];

            private readonly static SymbolDefinition[] CheckItems =
            [
                new SymbolDefinition(Value: 32, EncodeSymbol: '*', DecodeSymbols: ['*']),
                new SymbolDefinition(Value: 33, EncodeSymbol: '~', DecodeSymbols: ['~']),
                new SymbolDefinition(Value: 34, EncodeSymbol: '$', DecodeSymbols: ['$']),
                new SymbolDefinition(Value: 35, EncodeSymbol: '=', DecodeSymbols: ['=']),
                new SymbolDefinition(Value: 36, EncodeSymbol: 'U', DecodeSymbols: ['U', 'u']),
            ];

            public static Dictionary<int, char> ValueEncodeSymbolPairs => Items
                .ToDictionary(x => x.Value, x => x.EncodeSymbol);

            public static Dictionary<char, int> DecodeSymbolValuePairs => Items
                .SelectMany(x => x.DecodeSymbols.Select(y => new { Value = x.Value, DecodeSymbol = y }))
                .ToDictionary(x => x.DecodeSymbol, x => x.Value);

            public static Dictionary<int, char> ValueEncodeSymbolWithCheckItemPairs => Items
                .Union(CheckItems)
                .ToDictionary(x => x.Value, x => x.EncodeSymbol);
        }

        public static string ToBase32String(byte[] buffer)
            => ToBase32String(buffer, 0, buffer.Length);

        public static string ToBase32String(byte[] buffer, int offset, int length)
        {
            ArgumentNullException.ThrowIfNull(buffer);
            ArgumentOutOfRangeException.ThrowIfNegative(offset);
            ArgumentOutOfRangeException.ThrowIfNegative(length);

            int base32StringLenght = (int)Math.Ceiling(length * 8 / 5m);
            var builder = new StringBuilder(capacity: base32StringLenght);

            int bits = 0; ushort shift = 0;
            for (int i = offset; i < length; i++)
            {
                Encode(buffer[i], ref bits, ref shift, builder);
            }
            Flush(ref bits, ref shift, builder);

            return builder.ToString();
        }

        public static byte[] FromBase32String(string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value.Length == 0)
                return Array.Empty<byte>();

            var hyphenCount = value.Where(x => x == '-').Count();
            var buffer = new byte[(int)Math.Floor((value.Length - hyphenCount) * 5 / 8m)];

            int bits = 0; ushort shift = 0; int index = 0;
            for (int i = 0; i < value.Length; i++)
            {
                Decode(value[i], ref bits, ref shift, ref index, buffer);
            }
            return buffer;
        }

        public static char CalculateChecksum(byte[] buffer)
        {
            BigInteger value = BigInteger.Parse(Convert.ToHexString(buffer), NumberStyles.HexNumber);
            return SymbolCollection.ValueEncodeSymbolWithCheckItemPairs[Math.Abs((int)(value % 37))];
        }

        private static void Encode(byte value, ref int bits, ref ushort shift, StringBuilder output)
        {
            shift |= (ushort)(value << (8 - bits));
            bits += 8;
            while (bits >= 5)
            {
                output.Append(SymbolCollection.ValueEncodeSymbolPairs[(int)(shift >> 11)]);
                shift <<= 5;
                bits -= 5;
            }
        }

        private static void Flush(ref int bits, ref ushort shift, StringBuilder output)
        {
            if (bits > 0)
                output.Append(SymbolCollection.ValueEncodeSymbolPairs[(int)(shift >> 11)]);
        }

        private static void Decode(char value, ref int bits, ref ushort shift, ref int index, byte[] output)
        {
            if (value == '-')
                return;

            if (!SymbolCollection.DecodeSymbolValuePairs.TryGetValue(value, out int byteDecode))
                throw new InvalidOperationException($"'{value}' is an invalid symbol");

            shift |= (ushort)(byteDecode << (11 - bits));
            bits += 5;
            while (bits >= 8)
            {
                output[index++] = (byte)(shift >> 8);
                shift <<= 8;
                bits -= 8;
            }
        }
    }
}