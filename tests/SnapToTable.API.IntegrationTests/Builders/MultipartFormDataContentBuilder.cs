using SnapToTable.API.DTOs;
using SnapToTable.API.IntegrationTests.Factories;

namespace SnapToTable.API.IntegrationTests.Builders;

public class MultipartFormDataContentBuilder
{
    private readonly MultipartFormDataContent _content = new();

    public MultipartFormDataContentBuilder WithFile(string fileContent, string contentType, string fileName)
    {
        var streamContent = TestFileContentFactory.CreateFakeFileContent(fileContent, contentType);
        _content.Add(streamContent, nameof(CreateRecipeAnalysisRequestDto.Images), fileName);
        return this;
    }

    public MultipartFormDataContentBuilder WithValidImage(string fileName = "test.jpg")
    {
        return WithFile("This is a valid test image", "image/jpeg", fileName);
    }

    public MultipartFormDataContentBuilder WithInvalidImageContentType(string fileName = "test.pdf")
    {
        return WithFile("This is an invalid file type", "application/pdf", fileName);
    }

    public MultipartFormDataContentBuilder WithLargeImage(long sizeInBytes, string fileName = "large.jpg")
    {
        var streamContent = TestFileContentFactory.CreateFakeFileContentOfSize(sizeInBytes, "image/jpeg");
        _content.Add(streamContent, nameof(CreateRecipeAnalysisRequestDto.Images), fileName);
        return this;
    }

    public MultipartFormDataContent Build()
    {
        return _content;
    }
}