# MafPlayground

> A Microsoft Agent Framework Playground — admin UI for configuring and managing AI Chat Agents and Chat Clients backed by OpenRouter.

---

## 🧭 Overview

MafPlayground is a **Blazor Server** (.NET 10) web application that serves as a hands-on playground for the [Microsoft Agent Framework (MAF)](https://github.com/microsoft/Agents-for-net). It provides a fully-featured admin panel for managing the configuration of `ChatAgent` and `ChatClient` entities that drive AI interactions through the OpenRouter API.

The admin panel exposes full CRUD for both entity types via a modal-based UI — no page navigations needed. Data is persisted as JSON files on disk, keeping the setup dependency-free for local experimentation. The `ChatClientFactory` wraps the OpenAI client SDK with a raw logging pipeline policy, and `ChatAgentFactory` bridges model clients to MAF's `ChatClientAgent` abstraction.

The UI is built with **DaisyUI 5** (loaded locally from `wwwroot/assets/`) on top of Tailwind CSS, with a shadcn-inspired design system delivered through a reusable Blazor component library in `Components/Shared/`.

---

## ✨ Features

- **Full CRUD for ChatAgents** — create, view, edit, delete AI agent configurations including sampling parameters (temperature, top-p, top-k, etc.), system instructions, stop sequences, and tool-call behavior
- **Full CRUD for ChatClients** — manage provider connection settings (model, endpoint, org/project IDs, retries, timeout, distributed tracing)
- **Modal-based detail/add/edit UI** — view and edit records inline without page navigation; confirm-delete modal with entity name
- **JSON-backed persistence** — zero external dependencies; data stored in `Data/ChatAgents.json` and `Data/ChatClients.json`
- **DaisyUI 5 component library** — locally bundled CSS/JS; no CDN required at runtime
- **Responsive sidebar layout** — collapsible drawer nav on mobile, persistent sidebar on desktop
- **Reusable Blazor component library** — `Button`, `Modal`, `DataTable`, `PageHeader`, `FormField`, `DetailField`, `DetailSection`, `BadgeList`, `NullableBoolSelect`, `TagInput`, `Toast`, `ConfirmDeleteModal`
- **Toast notifications** — auto-dismissing success/error feedback on all write operations
- **Tag/chip input** — interactive stop-sequence editor (type + Enter to add, × to remove)
- **Nullable bool select** — three-state select (Unset / True / False) for optional boolean model parameters
- **Raw request/response logging** — `RawLoggingPolicy` pipeline policy logs full HTTP bodies to console for debugging

---

## 🛠 Tech Stack

| Technology | Version / Details |
|---|---|
| .NET | 10.0 |
| Blazor Server | Interactive Server render mode |
| DaisyUI | 5 (bundled locally in `wwwroot/assets/`) |
| Microsoft.Agents.AI | `1.0.0-rc4` (Declarative, OpenAI, Workflows) |
| Microsoft.Extensions.AI.OpenAI | `10.3.0` |
| OpenRouter API | Default endpoint: `https://openrouter.ai/api/v1` |
| ModelContextProtocol | `1.1.0` (AspNetCore + Core) |
| SemanticKernel.Connectors.InMemory | `1.74.0-preview` |

---

## 📁 Project Structure

```
MafPlayground/
├── Components/
│   ├── Layout/
│   │   └── MainLayout.razor          # DaisyUI drawer sidebar + sticky navbar
│   ├── Pages/
│   │   └── Admin/
│   │       ├── ChatAgents/
│   │       │   └── Index.razor       # Chat agents list + create/edit/detail modals
│   │       └── ChatClients/
│   │           └── Index.razor       # Chat clients list + create/edit/detail modals
│   └── Shared/                       # Reusable component library (see below)
├── Data/
│   ├── ChatAgents.json               # Persisted chat agent configurations
│   └── ChatClients.json              # Persisted chat client configurations
├── Models/
│   ├── ChatAgent.cs                  # Agent model with sampling & behavior parameters
│   └── ChatClient.cs                 # Client model with provider/connection settings
├── Repositories/
│   ├── IChatAgentRepository.cs
│   ├── IChatClientRepository.cs
│   ├── JsonChatAgentRepository.cs    # JSON file-based CRUD for ChatAgent
│   └── JsonChatClientRepository.cs   # JSON file-based CRUD for ChatClient
├── Services/
│   ├── ChatAgentService.cs           # Business logic layer for ChatAgent (+ interface)
│   ├── ChatClientService.cs          # Business logic layer for ChatClient (+ interface)
│   ├── ChatAgentFactory.cs           # Creates MAF ChatClientAgent instances
│   ├── ChatClientFactory.cs          # Creates OpenAI ChatClient with logging pipeline
│   ├── ChatAgentFactoryExtensions.cs
│   ├── ChatClientFactoryExtensions.cs
│   └── RawLoggingPolicy.cs           # PipelinePolicy that logs raw HTTP req/resp bodies
├── wwwroot/assets/daisyui/           # Locally bundled DaisyUI 5 CSS + Tailwind browser plugin
├── appsettings.json                  # Base configuration (OpenRouter key placeholder)
├── appsettings.Development.json      # ⚠ Gitignored — local API key override (create manually)
└── Program.cs                        # DI registration + middleware pipeline
plans/
├── admin-panel-plan.md               # Original admin panel implementation spec
└── reusable-components-plan.md       # Reusable component architecture spec
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Clone & Run

```shell
git clone <repo-url>
cd MafPlayground
```

Create the local secrets file (see [Configuration](#⚙️-configuration) below), then:

```shell
dotnet run --project MafPlayground
```

The app will be available at `https://localhost:5001` (or the port shown in the terminal). Navigate to `/admin/chat-agents` or `/admin/chat-clients`.

---

## ⚙️ Configuration

### `appsettings.Development.json` — Required, Gitignored

`appsettings.json` ships with a placeholder API key. You **must** create `MafPlayground/appsettings.Development.json` locally with your real [OpenRouter](https://openrouter.ai/) API key. This file is listed in `.gitignore` and is never committed.

```json
{
  "OpenRouter": {
    "ApiKey": "sk-or-v1-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
  }
}
```

### Configuration Reference

| Key | Default | Description |
|---|---|---|
| `OpenRouter:ApiKey` | `"YOUR_API_KEY_HERE"` | OpenRouter API key used by `ChatClientFactory` to authenticate against the OpenRouter API endpoint |
| `Logging:LogLevel:Default` | `"Information"` | Default minimum log level |
| `Logging:LogLevel:Microsoft.AspNetCore` | `"Warning"` | ASP.NET Core framework log level |
| `AllowedHosts` | `"*"` | Allowed host header values |

---

## 🏗 Architecture Notes

The project follows a standard **layered architecture**:

```
Models  →  Repositories (JSON)  →  Services (business logic)  →  Blazor Pages
```

- **Models** (`ChatAgent`, `ChatClient`) are plain C# records with `DataAnnotations` validation attributes.
- **Repositories** (`JsonChatAgentRepository`, `JsonChatClientRepository`) implement `IChatAgentRepository` / `IChatClientRepository` and persist data as indented JSON to files under the output directory's `data/` subfolder. Registered as **Singleton** so the in-process file handle is shared.
- **Services** (`ChatAgentService`, `ChatClientService`) are thin pass-through layers over the repositories. Interfaces are co-located in the same file as implementations. Registered as **Transient**.
- **Factories** (`ChatAgentFactory`, `ChatClientFactory`) handle construction of the Microsoft Agent Framework and OpenAI SDK objects. `ChatClientFactory` injects `ILoggerFactory` and attaches `RawLoggingPolicy` to every client for full HTTP-level debug visibility.
- **Blazor Pages** (`Admin/ChatAgents/Index.razor`, `Admin/ChatClients/Index.razor`) consume `IChatAgentService` / `IChatClientService` via DI and own all UI state — modals, form models, loading flags, and toast references.
- **Shared Components** provide the complete UI building-block library consumed by both admin pages (and any future pages).

---

## 🧩 Component Library

All reusable components live in `MafPlayground/Components/Shared/`:

| Component | Purpose |
|---|---|
| `Button.razor` | Unified DaisyUI `btn` with variant, size, loading spinner, icon, circle, and disabled support |
| `Modal.razor` | Full DaisyUI modal shell with title/subtitle header, scrollable body slot, footer slot, and backdrop close |
| `PageHeader.razor` | Page-level heading block: title, subtitle, and right-aligned action slot |
| `DataTable.razor` | Card-wrapped `<table class="table table-zebra">` with loading spinner, empty-state message, and header/body slots |
| `FormField.razor` | `form-control` wrapper with label, required asterisk, hint text, and column-span support |
| `DetailField.razor` | Read-only label + value pair; optional monospace value rendering; shows `—` when null |
| `DetailSection.razor` | Titled section group inside detail modals; optional divider; configurable grid columns |
| `BadgeList.razor` | Renders a collection of strings as DaisyUI `badge badge-outline` pills; optional monospace |
| `NullableBoolSelect.razor` | Three-state `<select>` for `bool?` properties: Unset / True / False |
| `TagInput.razor` | Interactive chip/tag editor — type + Enter to add, × to remove |
| `Toast.razor` | Auto-dismissing top-right toast notification (success / error / info) |
| `ConfirmDeleteModal.razor` | Reusable delete confirmation modal accepting an entity name and confirm callback |

---

## 📄 License

See [LICENSE](LICENSE).
