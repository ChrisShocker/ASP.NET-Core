using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        // field to hold the injected content type provider,
        // used to determine MIME types based on file extensions (e.g., .pdf -> application/pdf)
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        // call constructor to inject the FileExtensionContentTypeProvider for content types from file extensions
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider =
                fileExtensionContentTypeProvider
                ?? throw new ArgumentNullException(nameof(FileExtensionContentTypeProvider));
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var pathToFile = "HelloWorld.txt";

            if (pathToFile == null)
            {
                return NotFound();
            }

            // check if the file exists, if not return 404 Not Found
            var bytes = System.IO.File.ReadAllBytes(pathToFile);

            if (
                !_fileExtensionContentTypeProvider.TryGetContentType(
                    pathToFile,
                    out var contentType
                )
            )
            {
                // if the content type is not found, default to application/octet-stream
                contentType = "application/octet-stream";
            }

            // dynamically get content and path to the file
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
