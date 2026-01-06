using Minio;
using Minio.DataModel.Args;
using TierList.Application.Ports.Services;

namespace TierList.Infrastructure.Services;

public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public MinioStorageService(IMinioClient minioClient, string bucketName)
    {
        _minioClient = minioClient;
        _bucketName = bucketName;
    }

    public async Task<string> UploadPdfAsync(byte[] pdfData, string fileName, CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(_bucketName);
        var bucketExists = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

        if (!bucketExists)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(_bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }

        using var stream = new MemoryStream(pdfData);
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(pdfData.Length)
            .WithContentType("application/pdf");

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

        return $"{_bucketName}/{fileName}";
    }

    public async Task<byte[]> DownloadPdfAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        var parts = fileUrl.Split('/');
        var bucket = parts[0];
        var fileName = parts[1];

        using var memoryStream = new MemoryStream();
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream));

        await _minioClient.GetObjectAsync(getObjectArgs, cancellationToken);

        return memoryStream.ToArray();
    }

    public async Task DeletePdfAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        var parts = fileUrl.Split('/');
        var bucket = parts[0];
        var fileName = parts[1];

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName);

        await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
    }
}
