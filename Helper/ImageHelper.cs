using RestSharp;

namespace EBISX_POS.API.Helper
{
    public static class ImageHelper
    {
        public static async Task<string> DownloadAndSaveImageAsync(string imageUrl, string saveFolder)
        {
            // Check if the folder exists; create it only if it doesn't.
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            // Get the file name from the URL.
            var fileName = Path.GetFileName(new Uri(imageUrl).AbsolutePath);
            var filePath = Path.Combine(saveFolder, fileName);

            // Create a RestSharp client and request.
            var client = new RestClient();
            var request = new RestRequest(imageUrl, Method.Get);

            // Execute the request.
            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful && response.RawBytes != null)
            {
                await File.WriteAllBytesAsync(filePath, response.RawBytes);
                return filePath;
            }
            else
            {
                throw new Exception($"Failed to download image. Status: {response.StatusCode}");
            }
        }
    }
}
