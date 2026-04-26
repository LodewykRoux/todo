using Google.GenAI;

namespace Todo.AI;

public static class Generate {
    public static async Task<string> GenerateTodoItems(string prompt) {
        var client = new Client();
        var response = await client.Models.GenerateContentAsync(
            model: "gemini-3-flash-preview", contents: "Explain how AI works in a few words" + prompt
        );
        return response.Candidates[0].Content.Parts[0].Text;
    }
}