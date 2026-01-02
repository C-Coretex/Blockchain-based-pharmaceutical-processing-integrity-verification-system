using PharmaBlockchainBackend.Domain.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PharmaBlockchainBackend.Domain.Helpers
{
    public static class HashHelpers
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never
        };
        public static byte[] CalculateHash(ProtocolType protocolType, int stepNumber, Guid packageCode, object? additionalData)
        {
            var additionalDataString = additionalData is null
                ? string.Empty
                : JsonSerializer.Serialize(additionalData, _jsonOptions);

            var rawData = $"{protocolType}|{stepNumber}|{additionalDataString}|{packageCode}";
            var bytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rawData));

            return bytes;
        }
    }
}
