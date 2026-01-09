Overview

INT.Assessment Email Template API is an ASP.NET Core Web API that generates structured email templates using Groq LLM (OpenAI-compatible API).
The application accepts email intent details (purpose, recipient name, tone), sends a prompt to the AI model, and returns a JSON-formatted email template along with response time metrics.

The solution follows a layered architecture:

API Layer

Business Logic Layer (BLL)

Entity Layer

Custom File-based Logger
