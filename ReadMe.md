# NotificationService

A lightweight ASP.NET Core microservice for sending email notifications via RabbitMQ and MassTransit. 

---

##  Features

- **Email Notifications**: Send welcome and custom emails.
- **Messaging**: Powered by MassTransit + RabbitMQ.
- **Logging**: Serilog integration with console and Seq sinks.
- **Traceability**: Request correlation IDs for easier debugging.

---

##  Project Layout

```plaintext
NotificationService/
├── Consumers/               # MassTransit consumers
├── Controllers/             # API controllers
├── Dtos/                    # Data Transfer Objects
├── Interfaces/              # Service interfaces
├── Middleware/              # Custom middleware (CorrelationId)
├── Models/                  # Email models and enums
├── Services/                # Email sending service
├── appsettings.json         # Config file
├── Program.cs               # Entry point
├── Dockerfile               # Docker build file
└── docker-compose.yml       # Compose for RabbitMQ + service








##  Setup & Run

### Run locally

1. Clone the repository:

   ```bash
   git clone https://github.com/YourUsername/NotificationService.git
   cd NotificationService

2. Configuration:
	
	```json
{
  "AllowedHosts": "*",
  "Serilog": {
    "Using": [ "Serilog.Sinks.Seq", "Serilog.Sinks.Console" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "Seq", "Args": { "serverUrl": "http://localhost:5341" } }
    ],
    "Enrich": [ "WithMachineName", "WithThreadId" ]
  },
  "Email": {
    "From": "your-email@example.com",
    "Host": "sandbox.smtp.mailtrap.io",
    "Port": 2525,
    "Username": "your-username",
    "Password": "your-password"
  },
  "MessageBroker": {
    "Host": "rabbitmq://localhost",
    "Username": "guest",
    "Password": "guest"
  }
}


3. Build and run the application:

 ```bash
    dotnet build
    dotnet run

4. Access the API documentation at:

 ```bash
    https://localhost:{PORT}/scalar-api-reference

Note: This project uses Scalar API Reference instead of Swagger for API documentation and testing.



5. Run with Docker Compose
Build and start containers:

```bash
    docker-compose up --build

The NotificationService will be accessible on http://localhost:5000.

RabbitMQ management UI available at http://localhost:15672 (default guest/guest credentials).



##  API

Send Custom Email:

```bash
    POST /api/content/custom
    Content-Type: application/json

Body:

```json
    {
      "to": "recipient@example.com",
      "subject": "Your subject here",
      "body": "Email body content",
      "emailType": "Message"
    }