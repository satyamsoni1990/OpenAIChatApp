using OpenAI.Chat;
using Azure;
using Azure.AI.OpenAI;

var endpoint = new Uri("Yourendpoint");
var deploymentName = "gpt-4";
var apiKey = "<your-api-key>";
Console.WriteLine("Ask your Question.");
string question=Console.ReadLine();
AzureOpenAIClient azureClient = new(
    endpoint,
    new AzureKeyCredential(apiKey));
ChatClient chatClient = azureClient.GetChatClient(deploymentName);

List<ChatMessage> messages = new List<ChatMessage>()
{
    new SystemChatMessage("To ensure clarity, please provide more information about the specific task, question, or goal you'd like to address. With more details, I can craft a system prompt that includes steps, examples, tools, and a summary tailored to your requirements! For example, are you trying to brainstorm, explain a concept, code, summarize, provide creative input, or something else."),
    new UserChatMessage(question),
};

var response = chatClient.CompleteChatStreaming(messages);

foreach (StreamingChatCompletionUpdate update in response)
{
    foreach (ChatMessageContentPart updatePart in update.ContentUpdate)
    {
        System.Console.Write(updatePart.Text);
    }
}
System.Console.WriteLine("");
Console.ReadLine();