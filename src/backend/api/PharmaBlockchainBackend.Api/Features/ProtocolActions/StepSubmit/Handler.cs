namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.StepSubmit
{
    public class Handler()
    {
        public async Task Handle(Request request, CancellationToken ct)
        {
            List<byte[]> packageHashes = [];
            foreach (var packageCode in request.PackageCodes)
            {
                var hash = CalculateHash(request);
                packageHashes.Add(hash);
            }
            

            //Send data to blockchain (hashes in one batch)
            var timestamp = DateTime.UtcNow; //We will get it from Blockchain

            //Save data to database


            await Task.Delay(100, ct); // Simulate some asynchronous work.
            return;
        }

        private static byte[] CalculateHash(Request request)
        {
            var additionalDataString = request.AdditionalData is null
                ? string.Empty
                : System.Text.Json.JsonSerializer.Serialize(request.AdditionalData);

            var packageCodesString = string.Join(",", request.PackageCodes.Order());

            var rawData = $"{request.ProtocolType}|{request.StepNumber}|{additionalDataString}|{packageCodesString}";
            var bytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rawData));

            return bytes;
        }
    }
}
