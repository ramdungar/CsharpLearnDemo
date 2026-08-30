# Principles, standards, and practices

C# is the language. **These rules are how teams survive a 200 kLOC solution.**
Run `solid`, `cleancode`, and `patterns` for live examples.

## Design principles

### SOLID (Robert C. Martin)

| Letter | Meaning | C# smell when you violate it |
| --- | --- | --- |
| **S**ingle Responsibility | One reason to change | `UserService` saves SQL *and* sends email *and* logs metrics |
| **O**pen/Closed | Extend without editing shipped logic | 40-case `switch` on `NotificationType` in a core assembly |
| **L**iskov Substitution | Subtypes honor the contract | `Square : Rectangle` breaks `SetWidth` |
| **I**nterface Segregation | Small role interfaces | `IMachine` with Fax() that throws `NotImplementedException` |
| **D**ependency Inversion | Depend on abstractions | `new SqlConnection()` inside a domain class |

**DIP in ASP.NET Core:** register in `Program.cs`, inject through constructors.
The composition root is the only place that knows concrete types.

### DRY, KISS, YAGNI

- **DRY:** duplicate *knowledge* (the same business rule) is the bug. Two similar
  loops are not automatically a crime.
- **KISS:** the design a teammate can change on Friday afternoon.
- **YAGNI:** do not add a generic plugin host for one customer.

### Other principles you will hear

- **Separation of Concerns** — UI, application, domain, infrastructure.
- **Composition over inheritance** — "has a" + interface beats deep `Base*` trees.
- **Command-Query Separation** — mutators do not double as getters when it
  surprises the caller.
- **Fail fast** — guard clauses (`ArgumentNullException.ThrowIfNull`) at the edge.
- **Immutability** — records, `init`, `with`; fewer race conditions.
- **Least privilege API surface** — return `IReadOnlyList<T>`, not `List<T>`,
  if the caller must not mutate.

## Microsoft standards (the ones reviewers cite)

Official source: *Framework Design Guidelines* and the [Runtime coding style](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md).

- **PascalCase** public types and members; **camelCase** parameters and locals.
- Interfaces start with **`I`**.
- Async methods end with **`Async`** and return `Task` / `Task<T>` / `ValueTask`.
- Do not prefix fields with `_` in the BCL; many product teams *do* use `_field`
  (this repo does). Pick one in `.editorconfig` and keep it.
- Prefer properties over `GetX()`/`SetX()` unless the work is expensive or can fail.
- Throw **specific** exceptions (`ArgumentOutOfRangeException`), not `Exception`.
- **Do not swallow exceptions**. Log and rethrow (`throw;`) or handle fully.
- **IDisposable:** if you own it, you dispose it (`using` / `await using`).
- **Do not** create a finalizer unless you wrap unmanaged memory and you
  understand the rules.

This repo's on-disk standard is [../.editorconfig](../.editorconfig) plus
[../Directory.Build.props](../Directory.Build.props) (`Nullable`, `LangVersion`).

## Async / concurrency practice

1. I/O methods are async; CPU loops are sync (or `Task.Run` at the edge, not
   inside libraries).
2. **Async all the way** — no `.Result` / `.Wait()` on ASP.NET or UI.
3. `async void` only for event handlers.
4. `CancellationToken` on public I/O APIs.
5. `lock` a private object (or C# 13 `Lock`), never `this`, a `string`, or a
   public type.
6. Immutable snapshots or concurrent collections (`ConcurrentDictionary`)
   instead of "I will remember to lock".

## Error handling practice

- Use exceptions for *exceptional* paths; `bool TryParse` / `Result<T>` when
  failure is normal (user input).
- Filter with `catch (X) when (...)` (C# 6) instead of catch-all + `if`.
- Application exceptions carry context (`nameof(param)`, ids). Do not dump
  secrets into messages.

## Testing practice (this solution)

- **xUnit**, **AAA**, **`[Theory]`** for data tables.
- Test behavior of `InterviewAlgorithms` (pure functions), not `Console.WriteLine`.
- Smoke-run every lesson in CI so examples cannot silently start throwing.
- One logical assert per test when you can; `Theory` when only the data changes.

```bash
dotnet test
```

## Architecture practice (when you leave the console)

A common C# layout that follows DIP + SoC:

```
MyApp.Domain          entities, records, domain services (no EF, no HTTP)
MyApp.Application     use cases, interfaces (IUserRepository)
MyApp.Infrastructure  EF Core, SMTP, file system — implements interfaces
MyApp.Api             ASP.NET, composition root
MyApp.Tests
```

Names vary (Clean Architecture, Onion, Ports and Adapters). The rule does not:
**inner layers do not reference outer ones.**

## Security hygiene (asked more often now)

- Never build SQL with `$"...{userInput}"` — use parameters / EF LINQ.
- Never store passwords; use ASP.NET Identity / a proper hasher.
- `HttpClient` is typed and typed-clients / `IHttpClientFactory`, not `new` per call.
- Treat warnings as errors on CI; nullable exists to kill NREs.

## What "good C#" looks like in review

1. The type system tells the truth (`string?` if it can be missing).
2. Dependencies are injected; tests swap fakes.
3. LINQ is readable; hot paths do not hide accidental `O(n^2)` or client-eval.
4. Names match guidelines; files match namespaces/folders.
5. No commented-out code; history lives in git.
6. New features use the current language (collection expressions, records)
   unless the team's `LangVersion` says otherwise.
