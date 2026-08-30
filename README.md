# CsharpLearnDemo — C# from the first compiler to today

A **runnable .NET 8 course**. You do not only read about C#; you run a lesson and
watch the language feature print its own result. Every example is commented with:

- **what** the syntax does
- **why** that version of C# added it (the problem it solved)
- **which principle or standard** a professional team would apply
- **how an interviewer** usually phrases the question

## What you will learn

| Track | Lesson keys | Outcome |
| --- | --- | --- |
| Fundamentals | `types` `flow` `methods` `oop` `memory` | Types, control flow, methods, OOP, GC, exceptions |
| Versions C# 1 → 14 | `csharp1` … `csharp13` | Every major release and *why* it changed |
| Principles | `solid` `cleancode` `patterns` | SOLID, DRY/KISS/YAGNI, guidelines, patterns |
| Interview | `interview` `problems` | Spoken answers + coded screens with tests |

Deeper reading (same ideas, more prose):

- [docs/LEARNING_PATH.md](docs/LEARNING_PATH.md) — suggested order if you are new
- [docs/WHY_VERSIONS_CHANGED.md](docs/WHY_VERSIONS_CHANGED.md) — one page per C# version
- [docs/PRINCIPLES_AND_STANDARDS.md](docs/PRINCIPLES_AND_STANDARDS.md) — practices teams enforce
- [docs/INTERVIEW_PREP.md](docs/INTERVIEW_PREP.md) — question bank and whiteboard tips

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS)
- A terminal. Visual Studio, Rider, or VS Code + C# Dev Kit are optional.

This repo uses **C# 12** (`<LangVersion>latest</LangVersion>` on .NET 8). C# 13–14
features are taught in the `csharp13` lesson and in the version docs; they need a
newer SDK if you want to compile those exact tokens.

## Run the course

```bash
dotnet run --project src/CsharpLearnDemo -- list     # catalog
dotnet run --project src/CsharpLearnDemo -- types    # one lesson
dotnet run --project src/CsharpLearnDemo -- versions # whole track
dotnet run --project src/CsharpLearnDemo -- all      # everything
dotnet run --project src/CsharpLearnDemo             # interactive menu
```

From the repo root you can also:

```bash
dotnet test
dotnet format
```

`dotnet test` is part of the course: the interview algorithms are locked by xUnit
tests. That is the professional loop — change code, stay green.

## How to study (do not only scroll)

1. Read the **WHY THIS EXISTS** block in the lesson (or the matching section in
   `docs/WHY_VERSIONS_CHANGED.md`).
2. Read the comments in the `.cs` file — they are the textbook.
3. Run the lesson and match each `=>` line to the snippet that produced it.
4. Close the file and explain the feature out loud in two sentences (interview mode).
5. For `problems`, re-implement `InterviewAlgorithms` on paper, then `dotnet test`.

## Project layout

```
CsharpLearnDemo.sln
Directory.Build.props          shared SDK settings (nullable, language version)
.editorconfig                  on-disk coding standard
src/CsharpLearnDemo/
  Program.cs                   thin Main (composition root)
  GlobalUsings.cs              C# 10 global usings, explained
  Infrastructure/              menu, catalog, printer
  Lessons/Fundamentals/
  Lessons/Versions/            csharp1 … csharp13
  Lessons/Principles/
  Lessons/Interview/
tests/CsharpLearnDemo.Tests/   algorithms + smoke-run of every lesson
docs/                          prose companion to the runnable lessons
```

## Practices this repo follows on purpose

- **SDK-style projects**, nullable reference types, implicit usings
- **File-scoped namespaces** (C# 10) and **collection expressions** (C# 12)
- **SOLID** in the host: new lesson = new class; the menu does not change (OCP)
- **Separation of Concerns**: `Program` does not contain teaching text
- **AAA tests**, Theory for parameterized cases
- **Framework Design Guidelines** naming (`ILesson`, `Async` suffix, PascalCase)

## Suggested first hour

```bash
dotnet run --project src/CsharpLearnDemo -- types
dotnet run --project src/CsharpLearnDemo -- oop
dotnet run --project src/CsharpLearnDemo -- csharp3
dotnet run --project src/CsharpLearnDemo -- csharp5
dotnet run --project src/CsharpLearnDemo -- solid
dotnet run --project src/CsharpLearnDemo -- problems
dotnet test
```

Then continue with [docs/LEARNING_PATH.md](docs/LEARNING_PATH.md).
