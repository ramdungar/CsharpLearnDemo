# C# interview preparation

Pair this file with:

```bash
dotnet run --project src/CsharpLearnDemo -- interview
dotnet run --project src/CsharpLearnDemo -- problems
dotnet test
```

Speak in this order for any question: **definition → why it exists → pitfall →
when you used it**.

## Language and CLR

**Value type vs reference type?**
Value: data in the variable (structs, enums, primitives). Copy copies bytes.
Reference: variable holds a pointer to a heap object. Copy copies the pointer.
A struct field on a class still lives on the heap with that object.

**Stack vs heap?**
Stack: frames for locals/references, cheap, gone when the method returns.
Heap: objects, GC generations 0/1/2, LOH for ~85KB+ arrays.

**Boxing?**
Wrapping a value type as `object` / an interface. Allocates. `ArrayList` boxed;
`List<int>` does not. Two boxed ints compared as `object` use references.

**string?**
Immutable reference type. `==` is overloaded for contents. Interning applies to
literals. Use `StringComparison` explicitly. `StringBuilder` for many appends.

**const vs readonly vs static readonly?**
`const` inlined at compile time (breaking if public and changed). `readonly`
instance set in ctor. `static readonly` set in static ctor / initializer.

**Equals / GetHashCode?**
Equal objects must share a hash. Override together. Records do it. Do not use
mutable objects as dictionary keys.

**IDisposable?**
Deterministic cleanup of non-memory resources. `using` / `await using`.
Do not rely on the finalizer for timely close of a SQL connection.

**delegate vs event vs Action/Func?**
Delegate = method type. Event = encapsulated multicast list. Action/Func =
generic delegates. Interface = multi-method role with possible state.

**abstract vs virtual vs override vs new vs sealed?**
`abstract` must be overridden; `virtual` may; `override` replaces a virtual
slot; `new` hides (does not participate in virtual dispatch); `sealed` stops
further override/inherit.

**interface vs abstract class?**
Interface: contract, multiple. Abstract class: shared state/code, single base.
C# 8 default interface methods exist so published interfaces can grow.

**IEnumerable vs IQueryable?**
In-memory delegates vs expression trees translated by EF. Do not return
`IQueryable` from a public application API. Watch accidental client evaluation.

**Covariance / contravariance?**
`IEnumerable<out T>` — produce only. `IComparer<in T>` — consume only.
`IEnumerable<string>` is `IEnumerable<object>`. `List<string>` is not
`List<object>` because you could `Add(new object())`.

**async/await?**
Compiler state machine. Await yields the thread during I/O. `async void` is
for event handlers only. Deadlock: sync-context + `.Result`.
`ConfigureAwait(false)` in libraries. `Task.WhenAll` for independent I/O.

**Task vs Thread vs ValueTask?**
Thread = OS thread. Task = promise (I/O often uses no dedicated thread).
ValueTask = avoid allocation when already complete; await once.

**lock?**
Mutual exclusion on a dedicated private object. Not `this`. Not async-compatible
(`lock` cannot await). Use `SemaphoreSlim` for async gates.

**GC?**
Reachability, generations, workstation vs server GC, LOH, `IDisposable` ≠ GC.
`GC.Collect` in production is almost always the wrong fix (fix allocations).

## Modern C# (they will ask "what's new?")

Have **two sentences per version** from [WHY_VERSIONS_CHANGED.md](WHY_VERSIONS_CHANGED.md).
Minimum to sound current:

- **8:** nullable references, ranges, async streams, DIM
- **9:** records, `init`, top-level programs
- **10:** global usings, file-scoped namespaces
- **11:** raw strings, `required`, generic math
- **12:** primary constructors, collection expressions
- **13:** params collections, `Lock` (needs .NET 9)

**record vs class?**
Record: value equality, `with`, `ToString`. Class: identity, behavior.
Entity with an `Id` is usually a class.

## OOP and design

Be ready to draw **SOLID** with a C# example (run `solid`). Name
**composition over inheritance**, **DI**, **repository** as an abstraction
over persistence (not "a class named Repository that is a second DbContext").

Patterns they expect by name: Strategy, Observer (events), Decorator
(`Stream`), Factory, Singleton (and why you prefer DI + "one registration"
instead of `public static readonly Instance`).

## Coding screen

Problems implemented in `InterviewAlgorithms` (write them from memory):

| Problem | Approach to say first |
| --- | --- |
| Reverse string | `ToCharArray` + `Array.Reverse` (immutable string) |
| Valid palindrome | two pointers, skip non-alphanumeric |
| FizzBuzz | `%` and a clear order (15 before 3 and 5) |
| TwoSum | dictionary complement, O(n) |
| Fibonacci | iterative O(n), mention naive recursion is exponential |
| Group anagrams | sort letters as key, or 26-count key |
| Top per group | `GroupBy` + `OrderByDescending` + `First` |

Always: restated requirement, example, complexity, then code. Ask about
null, empty, unicode, overflow.

## Behavioral / "senior" extras

- How do you review a PR? (tests, nullable, async, allocations, secrets)
- How do you choose .NET LTS vs STS?
- How do you find a memory leak? (dotMemory / `dotnet-trace`, event handles, static events)
- What happens if you `await` inside `lock`? (it does not compile — good)

## A 30-minute drill

1. Explain boxing without drawing.  
2. Explain why `List<string>` is not `List<object>`.  
3. Write TwoSum on paper.  
4. Explain an async deadlock.  
5. Recite why C# 3, 5, and 8 existed.  
6. `dotnet test` — if you changed `InterviewAlgorithms`, you must stay green.
