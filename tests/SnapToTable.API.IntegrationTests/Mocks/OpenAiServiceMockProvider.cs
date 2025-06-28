using System.Text.Json;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using Moq;
using SnapToTable.Infrastructure.DTOs;

namespace SnapToTable.API.IntegrationTests.Mocks;

public class OpenAiServiceMockProvider
{
    public Mock<IOpenAIService> MockOpenAiService { get; }
    
    private readonly Mock<IChatCompletionService> _mockChatCompletionService;

    public OpenAiServiceMockProvider()
    {
        MockOpenAiService = new Mock<IOpenAIService>();
        _mockChatCompletionService = new Mock<IChatCompletionService>();
        
        MockOpenAiService.Setup(s => s.ChatCompletion).Returns(_mockChatCompletionService.Object);
    }
    
    public void SetupSuccessResponse(RawRecipesDto rawRecipes)
    {
        var mockResponse = new ChatCompletionCreateResponse
        {
            Choices =
            [
                new()
                {
                    Message = new ChatMessage(StaticValues.ChatMessageRoles.Assistant,
                        JsonSerializer.Serialize(rawRecipes))
                }
            ]
        };

        _mockChatCompletionService
            .Setup(s => s.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResponse);
    }
    
}