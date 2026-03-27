# Admin Panel UI — Implementation Plan

## Overview

Build a fully functional, production-ready Admin Panel UI using **Blazor Server (.NET 10)** and **DaisyUI** (loaded from local CDN files in `wwwroot/assets/daisyui`) for managing `ChatAgent` and `ChatClient` entities.

---

## Codebase Context

- **Framework:** Blazor Server, .NET 10, `InteractiveServer` render mode
- **Data layer:** JSON file-based persistence via `IJsonChatAgentRepository` / `IJsonChatClientRepository`
- **Service layer:** `IChatAgentService` and `IChatClientService` (both expose `GetAll`, `Get`, `Create`, `Update`, `Delete`)
- **DI registration:** Services registered as `Transient`, repositories as `Singleton` in `Program.cs`
- **DaisyUI:** CSS + Tailwind browser plugin already loaded in `App.razor`
- **Current layout:** `MainLayout.razor` is essentially empty — needs full update

---

## Models

### `ChatAgent`
| Property | Type | Notes |
|---|---|---|
| `Id` | `required string` | Human-readable slug (e.g., "Default") |
| `Name` | `string?` | Optional display name |
| `Description` | `string?` | Optional description |
| `ConversationId` | `string?` | Optional |
| `Instructions` | `string?` | System prompt / instructions (textarea) |
| `Temperature` | `float?` | Range 0–2 |
| `MaxOutputTokens` | `int?` | Range 1–int.MaxValue |
| `TopP` | `float?` | Range 0–1 |
| `TopK` | `int?` | Positive integer |
| `FrequencyPenalty` | `float?` | Optional float |
| `PresencePenalty` | `float?` | Optional float |
| `Seed` | `long?` | Optional long |
| `ModelId` | `string?` | Optional model override |
| `StopSequences` | `List<string>?` | Tag/chip input UI |
| `AllowMultipleToolCalls` | `bool?` | Nullable checkbox/toggle |
| `AllowBackgroundResponses` | `bool?` | Nullable checkbox/toggle |

### `ChatClient`
| Property | Type | Notes |
|---|---|---|
| `Id` | `required string` | Human-readable slug |
| `Description` | `string` | Default: empty string |
| `Provider` | `required string` | Required text input |
| `Model` | `string` | Default: "openai/gpt-4o-mini" |
| `Endpoint` | `string` | Default: "https://openrouter.ai/api/v1" |
| `OrganizationId` | `string` | Optional |
| `ProjectId` | `string` | Optional |
| `MaxRetries` | `int?` | Optional, positive integer |
| `NetworkTimeout` | `TimeSpan?` | Input in seconds, converted to/from TimeSpan |
| `EnableDistributedTracing` | `bool?` | Nullable checkbox/toggle |

---

## Validation Rules

### `ChatAgent`
- `Id` → `[Required]`
- `Temperature` → `[Range(0, 2)]` if provided
- `TopP` → `[Range(0, 1)]` if provided
- `MaxOutputTokens` → `[Range(1, int.MaxValue)]` if provided

### `ChatClient`
- `Id` → `[Required]`
- `Provider` → `[Required]`
- `MaxRetries` → `[Range(0, int.MaxValue)]` if provided

---

## Pages & Routing

| Page | Route | Description |
|---|---|---|
| ChatAgents List | `/admin/chat-agents` | Table of all agents; Create/Edit via modals; Delete via confirm modal |
| ChatAgents Detail | `/admin/chat-agents/{id}` | Read-only view of a single agent |
| ChatClients List | `/admin/chat-clients` | Table of all clients; Create/Edit via modals; Delete via confirm modal |
| ChatClients Detail | `/admin/chat-clients/{id}` | Read-only view of a single client |

---

## File Structure

```
MafPlayground/
└── Components/
    ├── Layout/
    │   └── MainLayout.razor              ← UPDATE: DaisyUI sidebar + top navbar
    ├── Shared/
    │   ├── Toast.razor                   ← NEW: reusable alert/notification (auto-dismiss)
    │   ├── ConfirmDeleteModal.razor      ← NEW: reusable DaisyUI delete confirmation modal
    │   └── TagInput.razor                ← NEW: reusable chip/tag input for StopSequences
    └── Pages/
        └── Admin/
            ├── ChatAgents/
            │   ├── Index.razor           ← NEW: list page + Create/Edit modals
            │   └── Detail.razor          ← NEW: detail page /admin/chat-agents/{id}
            └── ChatClients/
                ├── Index.razor           ← NEW: list page + Create/Edit modals
                └── Detail.razor          ← NEW: detail page /admin/chat-clients/{id}
```

---

## Component Architecture

```mermaid
graph TD
    App[App.razor] --> Routes[Routes.razor]
    Routes --> MainLayout[MainLayout.razor - updated]
    MainLayout --> Sidebar[DaisyUI Sidebar nav]
    MainLayout --> Body[@Body]
    Body --> AgentIndex[ChatAgents/Index.razor]
    Body --> AgentDetail[ChatAgents/Detail.razor]
    Body --> ClientIndex[ChatClients/Index.razor]
    Body --> ClientDetail[ChatClients/Detail.razor]
    AgentIndex --> CreateModal[DaisyUI dialog - Create]
    AgentIndex --> EditModal[DaisyUI dialog - Edit]
    AgentIndex --> ConfirmDelete[ConfirmDeleteModal.razor]
    AgentIndex --> TagInput[TagInput.razor - StopSequences]
    AgentIndex --> Toast[Toast.razor - feedback]
    ClientIndex --> CreateModal2[DaisyUI dialog - Create]
    ClientIndex --> EditModal2[DaisyUI dialog - Edit]
    ClientIndex --> ConfirmDelete2[ConfirmDeleteModal.razor]
    ClientIndex --> Toast2[Toast.razor - feedback]
```

---

## UI/UX Decisions

| Concern | Decision |
|---|---|
| **DaisyUI Theme** | Default theme (light) |
| **Layout** | Left sidebar with nav links; top navbar with page title |
| **Create** | DaisyUI `<dialog>` modal with `EditForm` inside |
| **Edit** | DaisyUI `<dialog>` modal with `EditForm` inside; `Id` field is read-only |
| **Delete** | DaisyUI `<dialog>` confirmation modal (reusable `ConfirmDeleteModal` component) |
| **Detail** | Dedicated page showing all properties in a read-only card layout |
| **Notifications** | DaisyUI `alert` component shown at top of page, auto-dismiss after 3 seconds |
| **Loading states** | DaisyUI `loading` spinner shown during async operations |
| **StopSequences** | Custom `TagInput.razor` chip component — type + Enter to add, click × to remove |
| **NetworkTimeout** | Number input (seconds) — converted to/from `TimeSpan` on save/load |
| **Nullable booleans** | Three-state select: Unset / True / False |
| **Render mode** | `@rendermode InteractiveServer` on each page component |

---

## Implementation Checklist

- [ ] Update `MainLayout.razor` with DaisyUI sidebar navigation and top navbar
- [ ] Create `Components/Shared/Toast.razor` — reusable alert/notification component
- [ ] Create `Components/Shared/ConfirmDeleteModal.razor` — reusable delete confirmation modal
- [ ] Create `Components/Shared/TagInput.razor` — reusable chip/tag input for StopSequences
- [ ] Add DataAnnotations validation attributes to `ChatAgent` model
- [ ] Add DataAnnotations validation attributes to `ChatClient` model
- [ ] Create `Components/Pages/Admin/ChatClients/Index.razor` — list page with Create/Edit modals and delete confirmation
- [ ] Create `Components/Pages/Admin/ChatClients/Detail.razor` — read-only detail page at `/admin/chat-clients/{id}`
- [ ] Create `Components/Pages/Admin/ChatAgents/Index.razor` — list page with Create/Edit modals and delete confirmation
- [ ] Create `Components/Pages/Admin/ChatAgents/Detail.razor` — read-only detail page at `/admin/chat-agents/{id}`
- [ ] Update `_Imports.razor` if any new namespaces are needed for shared components
- [ ] Verify routing works correctly for all admin pages

---

## Notes

- No search, filtering, sorting, or pagination required
- No bulk operations or real-time updates required
- No authentication or authorization required
- Use existing `IChatAgentService` and `IChatClientService` — do not create new services
- Follow existing code conventions: primary constructors, `internal` for implementations, `I` prefix for interfaces
