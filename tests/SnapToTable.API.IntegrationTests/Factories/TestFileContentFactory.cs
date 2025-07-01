using System.Net.Http.Headers;

namespace SnapToTable.API.IntegrationTests.Factories;

public static class TestFileContentFactory
{
    public static StreamContent CreateFakeFileContent(string content, string contentType)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        var streamContent = new StreamContent(stream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        return streamContent;
    }
    public static StreamContent CreateFakeFileContentOfSize(long sizeInBytes, string contentType)
    {
        var stream = new MemoryStream();
        
        stream.WriteByte(0); 
        stream.Position = sizeInBytes - 1;
        
        stream.WriteByte(0);
        stream.Position = 0;

        var streamContent = new StreamContent(stream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        return streamContent;
    }
}