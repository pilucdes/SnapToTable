namespace SnapToTable.Application.Constants;

public static class FileValidationConstants
{
    public static readonly string[] ValidImageTypes = ["image/jpeg", "image/png","image/webp"];
    public const int MaxImageSizeInBytes = 1 * 1024 * 1024; // 1MB
    public static int MaxImageSizeInMegabytes => MaxImageSizeInBytes / 1024 / 1024;
}
