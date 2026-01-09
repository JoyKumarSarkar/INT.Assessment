using INT.Assessment.BLL.Signatures;
using INT.Assessment.ENTITY;
using INT.Assessment.LOGGER;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace INT.Assessment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTemplateController : ControllerBase
    {
        private readonly IBLLCommon _BLLCommon;
        private readonly string _apiKey = Environment.GetEnvironmentVariable("INT_GroqApiKey")!;
        private readonly LogAssembly LogAssembly = LogAssembly.API;
        private readonly HttpClient _httpClient;
        private readonly IFileLogger _FileLogger;

        public EmailTemplateController(IBLLCommon BLLCommon, IFileLogger FileLogger)
        {
            _BLLCommon = BLLCommon;
            _FileLogger = FileLogger;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        [HttpPost]
        [Route("Generate")]
        public async Task<ApiResponse<AiResponse>> Generate([FromBody] EmailTemplatePayload request)
        {
            try
            {
                _FileLogger.Info("START :: Generate.", LogAssembly);

                var RequestBodyBLL = _BLLCommon.CreateAiPromptRequestBody(request);

                if (!RequestBodyBLL.Success)
                {
                    _FileLogger.Warning(RequestBodyBLL.Message, LogAssembly);
                    return new ApiResponse<AiResponse> { Data = null!, Success = false, Message = RequestBodyBLL.Message };
                }

                var RequestBody = RequestBodyBLL.Data;

                if (string.IsNullOrWhiteSpace(RequestBody.messages[1].content))
                {
                    _FileLogger.Warning("Prompt generation failed.", LogAssembly);
                    return new ApiResponse<AiResponse> { Data = null!, Success = false, Message = "Prompt generation failed." };
                }

                var sw = Stopwatch.StartNew();

                var response = await _httpClient.PostAsJsonAsync(
                    "https://api.groq.com/openai/v1/chat/completions",
                    RequestBody
                );

                response.EnsureSuccessStatusCode();

                using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

                var aiText = json.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrWhiteSpace(aiText))
                {
                    _FileLogger.Warning("Empty AI response.", LogAssembly);
                    return new ApiResponse<AiResponse> { Data = null!, Success = false, Message = "Empty AI response." };
                }

                AiResponse aiResponse = null;

                try
                {
                    aiResponse = JsonConvert.DeserializeObject<AiResponse>(aiText);

                }
                catch
                {
                    _FileLogger.Warning("Failed to deserialize AI response. Because, it can't write an email that promotes hate or discrimination.", LogAssembly);
                    return new ApiResponse<AiResponse> { Data = null!, Success = false, Message = "Failed to deserialize AI response. Because, it can't write an email that promotes hate or discrimination." };
                }

                if (aiResponse == null)
                {
                    _FileLogger.Warning("Invalid AI response format.", LogAssembly);
                    return new ApiResponse<AiResponse> { Data = null!, Success = false, Message = "Invalid AI response format." };
                }

                aiResponse.AiResponseTimeMs = sw.ElapsedMilliseconds;

                _FileLogger.Info("END :: Generate.", LogAssembly);
                return new ApiResponse<AiResponse> { Data = aiResponse, Success = true, Message = "Email Template generated successfully!" };
            }
            catch (System.Text.Json.JsonException ex)
            {
                _FileLogger.Error("Exception in " + MethodBase.GetCurrentMethod(), LogAssembly, ex);
                return new ApiResponse<AiResponse> { Data = null!, Success = false, Message = "AI returned invalid JSON." };
            }
            catch (Exception ex)
            {
                _FileLogger.Error("Exception in " + MethodBase.GetCurrentMethod(), LogAssembly, ex);
                return new ApiResponse<AiResponse> { Data = null!, Success = false, Message = "Internal Server Error occurred!" };
            }
        }
    }
}
