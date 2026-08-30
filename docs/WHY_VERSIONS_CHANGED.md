# Why each C# version exists

Languages do not add features for fun. Each C# release is a response to
**pain that showed up in real Microsoft and customer code**. Use this page
as a timeline; use the `csharpN` lessons as the live lab.

C# is designed by the language team (historically Anders Hejlsberg) and
ships with a **.NET runtime**. The compiler version (`LangVersion`) and the
**target framework** (`net8.0`) are related but not identical: some features
are syntax-only; some need new BCL types.

| C# | Year | Typical runtime | Theme |
| --- | --- | --- | --- |
| 1.0 / 1.2 | 2002–2003 | .NET Framework 1.x | Component-oriented OOP on the CLR |
| 2.0 | 2005 | .NET Framework 2.0 | Generics and safer data |
| 3.0 | 2007 | .NET Framework 3.5 | LINQ / declarative data |
| 4.0 | 2010 | .NET Framework 4.0 | Dynamic and COM interop |
| 5.0 | 2012 | .NET Framework 4.5 | Async I/O |
| 6.0 | 2015 | .NET 4.6 / Roslyn | Ceremony reduction |
| 7.0–7.3 | 2017–2018 | .NET Core 2 / Fx 4.7 | Patterns + performance |
| 8.0 | 2019 | .NET Core 3 | Null safety + API evolution |
| 9.0 | 2020 | .NET 5 | Immutable data, one .NET |
| 10 | 2021 | .NET 6 LTS | File boilerplate gone |
| 11 | 2022 | .NET 7 | Literals, required, generic math |
| 12 | 2023 | .NET 8 LTS | Primary constructors, collections |
| 13 | 2024 | .NET 9 | params collections, Lock |
| 14 | 2025+ | .NET 10 | Extension members, `field` |

## C# 1.0 / 1.2 — a language for the CLR

**Problem:** Windows development was split across C++ (power, unsafety),
VB6 (approachable, no modern runtime), and Java (safe, but not designed for
COM, properties, or WinForms designers).

**Response:** A Java-like syntax on a new GC virtual machine, plus
**properties**, **events**, **delegates**, **structs**, **enums**,
**attributes**, and **using/IDisposable**. Those choices still dictate how
WPF, WinForms, and later ASP.NET bind data.

**Practice that started here:** Framework Design Guidelines — PascalCase,
`I`-prefix interfaces, exceptions instead of HRESULT in managed code.

## C# 2.0 — stop boxing and casting everything

**Problem:** `ArrayList` stored `object`. `int` was boxed; reads needed casts;
bugs appeared at runtime. Hand-written `IEnumerator` classes were miserable.
SQL / WinForms needed "int or NULL".

**Response:** **Generics** (reified on the CLR, unlike Java erasure),
**`T?` nullable value types**, **`yield return` iterators**, **anonymous
methods**, **partial types** (designer codegen).

**Interview line:** "CLR generics keep `List<int>` unboxed; Java erases to object."

## C# 3.0 — one query language for objects, XML, and SQL

**Problem:** Filtering in C# was nested `foreach`. Filtering in SQL was a
string. XML had yet another API. Teams could not transfer skill.

**Response:** **LINQ**. To make query syntax work the team added **lambdas**,
**extension methods**, **anonymous types**, **`var`**, **auto-properties**,
**object/collection initializers**, and **expression trees** (so LINQ to SQL
can become SQL instead of a delegate).

**Practice:** Prefer declarative queries; remember **deferred execution**.

## C# 4.0 — talk to the dynamic world

**Problem:** Office COM methods had 30 optional parameters (`Type.Missing`).
IronPython/JSON-like objects had no static schema. `IEnumerable<string>` was
not an `IEnumerable<object>` even when that was safe.

**Response:** **named/optional arguments**, **`dynamic` (DLR)**,
**generic covariance/contravariance** (`out T` / `in T`).

**Practice:** Keep `dynamic` at the system boundary; map to real types ASAP.

## C# 5.0 — I/O without callback soup

**Problem:** One thread per blocked request does not scale. UI threads that
call `.Wait()` freeze. Begin/End and Event-based async were unreadable.

**Response:** **`async`/`await`** (compiler state machine) and **caller-info
attributes** (`CallerMemberName`) for logging and `INotifyPropertyChanged`.

**Practice:** Async all the way. Never `.Result` on a UI/ASP.NET sync context.

## C# 6.0 — Roslyn makes small features cheap

**Problem:** The old native compiler made language changes expensive. Everyday
code was noisy: `string.Format`, null-check pyramids, magic strings.

**Response:** Open-source **Roslyn**. Then **interpolation**, **`nameof`**,
**`?.`**, **expression-bodied members**, **auto-property initializers**,
**`using static`**, **exception filters**, await in catch/finally.

**Practice:** `nameof` over magic strings. Do not interpolate SQL.

## C# 7.x — multiple returns and zero-copy

**Problem:** `out` parameters and throwaway DTO classes. Nested `if (x is T)`.
Cloud servers allocated too much while parsing bytes (Kestrel, UTF-8).

**Response:** **tuples**, **deconstruction**, **pattern matching**, **local
functions**, **`out var`**, **discards**, **throw expressions**, then
**`ref`/`in`/`readonly struct`/`Span<T>`** (7.2), better generic constraints (7.3),
**async Main** (7.1).

**Practice:** Tuples internally; named types on public APIs. Learn `Span<T>`.

## C# 8.0 — null is in the type system

**Problem:** `NullReferenceException` in production. Publishing a new method
on `IList<T>` would break the world. Buffering entire async result sets.

**Response:** **nullable reference types**, **default interface methods**,
**`IAsyncEnumerable` + `await foreach`**, **indexes/ranges**, **switch
expressions**, **using declarations**, **`??=`**.

**Practice:** `<Nullable>enable</Nullable>` and treat warnings as errors.

## C# 9.0 — one .NET, immutable data, tiny programs

**Problem:** .NET 5 unified Framework + Core. DTOs needed `Equals`/`with` by
hand. `Program.Main` + usings + class was too much for a 10-line tool.

**Response:** **`record`**, **`init`**, **`with`**, **top-level statements**,
**target-typed `new`**, relational/logical patterns, covariant returns.

**Practice:** Records for data; classes for identity and behavior.

## C# 10 — the file header was wasting attention

**Problem:** Fifteen `using` lines and a namespace brace indent on every file
in a .NET 6 minimal API world.

**Response:** **global usings**, **file-scoped namespaces**, **record structs**,
interpolated string handlers (cheap logging), const interpolation, lambda
improvements.

**Practice:** One `GlobalUsings.cs` or ImplicitUsings; file-scoped `namespace X;`.

## C# 11 — strings, initialization, and math

**Problem:** JSON/SQL in C# was escape-hell. Object initializers forgot required
fields. Every numeric type had a copy-pasted `Math` helper.

**Response:** **raw string literals** `""" ... """`, **`required` members**,
**list patterns**, **UTF-8 literals** `"ab"u8`, **generic math**
(`INumber<T>`, static abstract interface members), file-local types.

**Practice:** `required` or a constructor — never "remember to set X".

## C# 12 — DI and collections were still noisy

**Problem:** ASP.NET constructors were 20 assignment lines. Arrays, lists, and
spans each had a different literal syntax.

**Response:** **primary constructors** on classes/structs, **collection
expressions** `[1, 2, ..rest]`, **inline arrays**, default lambda parameters,
`using` alias for any type.

**Practice:** Primary constructors capture dependencies; validate invariants
explicitly when needed.

## C# 13 (.NET 9) — finish the collection/perf story

**Problem:** `params` always meant "allocate an array". `lock` only understood
monitors. Source generators wanted **partial properties**.

**Response:** **params collections** (`params ReadOnlySpan<T>`, `params IEnumerable<T>`),
**`System.Threading.Lock`**, `\e` escape, implicit `^` in initializers, ref in
async/iterators, partial properties, overload-resolution priority.

This repo compiles as C# 12; the `csharp13` lesson documents these and shows
the C# 12 equivalent.

## C# 14 (.NET 10) — extensions grow up

**Direction:** **extension members** (not only methods), the **`field`**
keyword in properties, null-conditional assignment, `nameof(List<>)`.

Same motive as C# 6–13: less boilerplate, more safety, better library APIs.

## How to reason about "should we upgrade?"

1. **Runtime need** (new BCL / ASP.NET / GC) → new `TargetFramework`.
2. **Syntax only** (collection expressions) → `LangVersion` on the current TFM.
3. **Libraries** target the oldest TFM customers have; multi-target if required.
4. Prefer **LTS** (.NET 8, then .NET 10) for production unless you need STS APIs.
