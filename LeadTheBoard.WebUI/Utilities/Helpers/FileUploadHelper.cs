namespace LeadTheBoard.WebUI.Utilities.Helpers
{
    public static class FileUploadHelper
    {
        public static async Task<string> UploadFileAsync(IFormFile file, string uploadRootFilePath)
        {
            try
            {
                // Check if the file is null or has no content
                if (file == null || file.Length == 0)
                {
                    throw new Exception("No file selected.");
                }

                // Generate a unique file name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // Set the file path for saving
                string filePath = Path.Combine(uploadRootFilePath, "uploads", fileName);

                // Create the directory if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return the file path
                return "/" + Path.Combine("uploads", fileName);
            }
            catch (Exception ex)
            {
                // Handle any errors
                throw new Exception("File upload failed: " + ex.Message);
            }
        }
    }
}
