using INT.Assessment.BLL.Signatures;
using INT.Assessment.ENTITY;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace INT.Assessment.BLL.Implementations
{
    public partial class BLLCommon : IBLLCommon
    {
        public BLLOutput<AiPrompt> CreateAiPromptRequestBody(EmailTemplatePayload payload)
        {
            try
            {
                _FileLogger.Info("START :: CreateAiPromptRequestBody().", LogAssembly);

                var prompt =
                    "Write a " + payload.Tone + " email.\n\n" +
                    "Return the response in the following JSON format ONLY:\n" +
                    "{\n" +
                    "  \"to\": \"\",\n" +
                    "  \"subject\": \"\",\n" +
                    "  \"body\": \"\",\n" +
                    "  \"signature\": \"\"\n" +
                    "  \"aiResponseTimeMs\": \"\"\n" +
                    "}\n\n" +
                    "Recipient name: " + payload.RecipientName + "\n" +
                    "Purpose: " + payload.Purpose;

                var requestBody = new AiPrompt
                {
                    model = "llama-3.1-8b-instant",
                    messages =
                    [
                        new AiPromptMessage { role = "system", content = "You are a helpful email writing assistant." },
                        new AiPromptMessage { role = "user", content = prompt }
                    ],
                    temperature = 0.7
                };

                _FileLogger.Info("END :: CreateAiPromptRequestBody().", LogAssembly);
                return new BLLOutput<AiPrompt> { Data = requestBody, Success = true };
            }
            catch (Exception ex)
            {
                _FileLogger.Error("Exception in " + MethodBase.GetCurrentMethod(), LogAssembly, ex);
            }
            return new BLLOutput<AiPrompt> { Data = null!, Message = "Internal Error occurred", Success = false };
        }
    }
}
