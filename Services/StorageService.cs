using Minio;
using Minio.DataModel.Args;

namespace Mottu.Backend.Services
{
    public class StorageService
    {
        private readonly IMinioClient _minio;
        private readonly string _bucketName;

        public StorageService(IConfiguration config)
        {
            var endpoint = config["MinIO:Endpoint"] ?? "localhost:9000";
            var accessKey = config["MinIO:AccessKey"] ?? "minioadmin";
            var secretKey = config["MinIO:SecretKey"] ?? "minioadmin";
            _bucketName = config["MinIO:BucketName"] ?? "mottu-cnh-bucket";

            _minio = new MinioClient()
                .WithEndpoint(endpoint.Replace("http://", "").Replace("https://", ""))
                .WithCredentials(accessKey, secretKey)
                .WithSSL(false)
                .Build();
        }

        private async Task EnsureBucketAsync()
        {
            var exists = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!exists)
                await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
        }

        public async Task UploadFileAsync(string objectName, Stream data, string contentType, long? size = null)
        {
            await EnsureBucketAsync();
            var put = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithObjectSize(size ?? (data.CanSeek ? data.Length : -1))
                .WithContentType(contentType);
            await _minio.PutObjectAsync(put);
        }

        public async Task<string> GetFileUrlAsync(string objectName)
        {
            await EnsureBucketAsync();
            return await _minio.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithExpiry(3600));
        }
    }
}
