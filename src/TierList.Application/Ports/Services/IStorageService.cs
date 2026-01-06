namespace TierList.Application.Ports.Services;

public interface IStorageService
{
    Task<string> UploadPdfAsync(byte[] pdfData, string fileName, CancellationToken cancellationToken = default);
    Task<byte[]> DownloadPdfAsync(string fileUrl, CancellationToken cancellationToken = default);
    Task DeletePdfAsync(string fileUrl, CancellationToken cancellationToken = default);
}
