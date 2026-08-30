# Learning path — start to end

Use this order if you are new to C#. Skip a row only if you can teach it to
someone else without opening the file.

Each step is `dotnet run --project src/CsharpLearnDemo -- <key>`.

## Week-shaped path (intensity, not a calendar)

### Foundations (the language still sits on these)

| Step | Key | You can stop when you can explain |
| --- | --- | --- |
| 1 | `types` | value vs reference, var vs dynamic, const vs readonly |
| 2 | `flow` | why switch expressions are safer than if-chains |
| 3 | `methods` | ref / out / in / params and why tuples replaced many outs |
| 4 | `oop` | interface vs abstract class, composition over inheritance |
| 5 | `memory` | stack vs heap, boxing, using/IDisposable, throw vs throw ex |

Read [PRINCIPLES_AND_STANDARDS.md](PRINCIPLES_AND_STANDARDS.md) naming section
in parallel — names are part of the type system humans compile.

### The version story (why the language kept changing)

Do **not** memorize feature lists. Memorize the *pressure* that created them.

| Key | Pressure |
| --- | --- |
| `csharp1` | Need a CLR language with properties/events for Windows components |
| `csharp2` | ArrayList boxing and casts were unsafe and slow |
| `csharp3` | Objects, XML, and SQL needed one query model (LINQ) |
| `csharp4` | COM/Office and dynamic languages were hostile to static C# |
| `csharp5` | Threads blocked on I/O; callbacks were unreadable |
| `csharp6` | Roslyn made many small ceremony-killers cheap to ship |
| `csharp7` | Multiple returns + F# patterns + zero-copy buffers |
| `csharp8` | Null was a billion-dollar mistake; APIs needed to evolve |
| `csharp9` | Immutable DTOs and tiny programs for one .NET |
| `csharp10` | Every file wasted space on usings and namespace braces |
| `csharp11` | JSON/SQL strings, required init, one math library |
| `csharp12` | DI constructors and collection syntax were still noisy |
| `csharp13` | params still allocated arrays; locking and extensions still evolving |

Companion essay: [WHY_VERSIONS_CHANGED.md](WHY_VERSIONS_CHANGED.md).

### How professionals write C#

| Key | Focus |
| --- | --- |
| `solid` | Five principles with a better design next to each |
| `cleancode` | DRY / KISS / YAGNI, guards, async rules, EditorConfig |
| `patterns` | Strategy, decorator, factory, repository — and the BCL equivalents |

### Interview week

| Key | Focus |
| --- | --- |
| `interview` | Spoken answers (CLR, string, async deadlock, IQueryable) |
| `problems` | Reverse, palindrome, FizzBuzz, TwoSum, Fib, anagrams, LINQ |

Then:

```bash
dotnet test
```

Close `InterviewAlgorithms.cs`, rewrite one method, run tests again.

Full question bank: [INTERVIEW_PREP.md](INTERVIEW_PREP.md).

## After you finish the repo

Build something small that *forces* the features:

1. Console notes app — files (`IDisposable`), records, LINQ grouping.
2. Tiny ASP.NET Core API — DI (DIP), async endpoints, nullable DTOs.
3. Add xUnit tests first for a new algorithm (red/green/refactor).

That is the end of "learning C# the language" and the start of "shipping C#".
