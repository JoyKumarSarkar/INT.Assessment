using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Assessment.ENTITY
{
    public class EmailTemplatePayload
    {
        public string Purpose { get; set; }
        public string RecipientName { get; set; }
        public string Tone { get; set; } 
    }

    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }

    public class BLLOutput<T>
    {
        public T Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }

    public class AiPrompt
    {
        public string model { get; set; }
        public AiPromptMessage[] messages { get; set; }
        public double temperature { get; set; }
    }

    public class AiPromptMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class AiResponse
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Signature { get; set; }
        public long AiResponseTimeMs { get; set; }
    }
}
