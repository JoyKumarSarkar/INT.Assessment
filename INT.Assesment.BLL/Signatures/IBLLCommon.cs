using INT.Assessment.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INT.Assessment.BLL.Signatures
{
    public interface IBLLCommon
    {
        BLLOutput<AiPrompt> CreateAiPromptRequestBody(EmailTemplatePayload payload);

    }
}
