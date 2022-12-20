using Hero.Server.Core.Configuration;
using Hero.Server.Core.Exceptions;
using Hero.Server.Messages.Requests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Minio;

namespace Hero.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly MinioOptions options;

        public UploadController(IOptions<MinioOptions> options)
        {
            this.options = options.Value;
        }

        private string GetExtensionFromBase64(string base64)
        {
            string extension = String.Empty;
            switch (base64[0])
            {
                case '/': extension = "jpg"; break;
                case 'i': extension = "png"; break;
                case 'R': extension = "gif"; break;
                case 'U': extension = "webp"; break;
            }
            return extension;
        }

        [HttpPost]
        public async Task<IActionResult> SaveImage([FromBody]UploadRequest request)
        {
            
            using (MemoryStream stream = new(Convert.FromBase64String(request.Data)))
            {
                string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
                string extension = this.GetExtensionFromBase64(request.Data);
                string fileWithExtension = Path.ChangeExtension(Path.GetRandomFileName(), extension);
                string url = Path.Combine($"https://{this.options.Endpoint}", this.options.Bucket, fileWithExtension);

                MinioClient client = new MinioClient()
                    .WithEndpoint(options.Endpoint)
                    .WithCredentials(this.options.AccessKey, this.options.SecretKey)
                    .WithSSL()
                    .Build();

                if (String.IsNullOrEmpty(extension))
                {
                    throw new ArgumentException("The given filetype is not supported");
                }

                PutObjectArgs args = new PutObjectArgs()
                    .WithBucket(this.options.Bucket)
                    .WithObject(fileWithExtension)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType("application/octet-stream");
                try
                {
                    await client.PutObjectAsync(args);
                }
                catch (Exception ex)
                {
                    throw new HeroException("File upload failed");
                }

                return this.Ok(new {Url = url});
            }
        }
    }
}
