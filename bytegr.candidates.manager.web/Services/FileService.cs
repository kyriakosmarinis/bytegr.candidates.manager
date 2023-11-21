using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace bytegr.candidates.manager.web.Services
{
	public static class FileService
	{
        private static async Task<byte[]> ToByteArrayAsync(this IFormFile file, ModelStateDictionary modelState, long maxSize = 2097152) {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            if (memoryStream.Length <= maxSize) {
                return memoryStream.ToArray();
            }
            else {
                modelState.AddModelError("File", "The file is too large.");
                return Array.Empty<byte>();
            }
        }

        public static IFormFile ToIFormFile(this byte[] byteArray, string fileName, string contentType) {
            using var memoryStream = new MemoryStream(byteArray);
            return new FormFile(memoryStream, 0, byteArray.Length, "CvFile", fileName) {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}

