**Overview**

**INT.Assessment Email Template API** is an ASP.NET Core Web API that generates structured email templates using Groq LLM (OpenAI-compatible API).
The application accepts email intent details (purpose, recipient name, tone), sends a prompt to the AI model, and returns a JSON-formatted email template along with response time metrics.

The solution follows a layered architecture:
**INT.Assessment**
├── INT.Assessment.API        → Controllers & API endpoints
├── INT.Assessment.BLL        → Business logic and prompt creation
├── INT.Assessment.ENTITY     → DTOs, enums, and models
├── INT.Assessment.LOGGER     → Custom file logger

Groq API Key (i.e., _INT_GroqApiKey_) is set in the environment variable using the command
setx INT_GroqApiKey "your_api_key"

Run the following commands to start the **INT.Assessment** Application
dotnet build
dotnet run

If the application in run by **https**, the API will be available at:
https://localhost:7121

_Swagger UI (Development only):_
https://localhost:7121/swagger

**Generate Email Template**
POST :: /api/EmailTemplate/Generate

Request Body
{
  "purpose": "Follow up on job application",
  "recipientName": "HR Manager",
  "tone": "professional"
}

| Field         | Type   | Description                                 |
| ------------- | ------ | ------------------------------------------- |
| purpose       | string | Reason for writing the email                |
| recipientName | string | Recipient's name                            |
| tone          | string | Email tone (formal, friendly, professional) |

**Success Response (200 OK)**
{
  "data": {
    "to": "hr.manager@example.com",
    "subject": "Follow-up on Job Application for [Position]",
    "body": "Dear HR Manager,\n\nI am writing to follow up on the status of my application for the [Position] role at [Company Name]. It has been [number] days since I submitted my application, and I am eager to learn about the progress of the hiring process.\n\nIf there is any additional information I can provide to support my application, please let me know. I would appreciate any update you can provide regarding the current status of my application.\n\nThank you for your time and consideration. I look forward to hearing from you soon.\n\nBest regards,\n[Your Name]",
    "signature": "Best regards,\n[Your Name]",
    "aiResponseTimeMs": 849
  },
  "message": "Email Template generated successfully!",
  "success": true
}

**Prompt Generation Failure**
{
  "data": null,
  "message": "Prompt generation failed.",
  "success": false
}

**AI Returned Invalid JSON**
{
  "data": null,
  "message": "AI returned invalid JSON.",
  "success": false
}

**Internal Server Error**
{
  "data": null,
  "message": "Internal Server Error occurred!",
  "success": false
}


**Logging**

Logs are written to daily rolling files
Thread-safe file writing

Log levels supported:
- INFO
- WARNING
- ERROR

_Log File Format_
<ApplicationName>.LOG-dd-MMM-yyyy.txt

_Sample Log Entry_
09-Jan-2026 22:45:10.123 [API] [INFO] START :: Generate.



**AI Prompt Design – Brief Explanation**

The AI prompt was designed to ensure consistent, structured, and machine-readable output from the language model. Instead of allowing free-form text, the prompt explicitly instructs the AI to return the response only in a predefined JSON format, matching the AiResponse model used by the application.

User inputs such as email purpose, recipient name, and tone are dynamically injected into the prompt, allowing flexibility while maintaining control over the output structure. A system role message sets the AI’s behavior as a helpful email-writing assistant, while the user role message contains the detailed instructions and constraints.

By enforcing:
- a fixed JSON schema
- clear field names (to, subject, body, signature)
- and tone-based context

The prompt minimizes parsing errors, simplifies deserialization, and ensures predictable API responses suitable for production use.
