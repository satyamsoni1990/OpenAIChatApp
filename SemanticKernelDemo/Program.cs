using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticKernelDemo;


var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true)
        .Build();

string endpoint = config["AzureOpenAI:Endpoint"];
string apiKey = config["AzureOpenAI:ApiKey"];
string model = config["AzureOpenAI:Model"];

var builder = Kernel.CreateBuilder();

//Services
builder.AddAzureOpenAIChatCompletion(model, endpoint, apiKey);

//Plugins
builder.Plugins.AddFromType<NewsPlugin>();
builder.Plugins.AddFromType<ArchivePlugin>();


Kernel kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();
ChatHistory chatHistroy = new ChatHistory("you are news agent and your work to serve today news ask user to category and ask to save with file name.");

while (true)
{
    Console.WriteLine("Prompt: ");
    chatHistroy.AddUserMessage(Console.ReadLine());

    var chatCompletion = chatService.GetStreamingChatMessageContentsAsync(chatHistroy,

        executionSettings: new OpenAIPromptExecutionSettings()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            Temperature = 1.0f,
            MaxTokens = 4096
        }, kernel: kernel);
    string fullMessage = "";
    await foreach (var content in chatCompletion)
    {
        fullMessage += content;

        // Check if a sentence has ended
        if (fullMessage.EndsWith(".") || fullMessage.EndsWith("!") || fullMessage.EndsWith("?"))
        {
            Console.WriteLine(fullMessage);
            chatHistroy.AddAssistantMessage(fullMessage);
            fullMessage = ""; // Reset for the next sentence
        }
    }
    //chatHistroy.AddAssistantMessage(fullMessage);
    Console.WriteLine(fullMessage);
}


