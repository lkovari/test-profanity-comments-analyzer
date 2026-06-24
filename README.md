# TestProfanity

Sample .NET solution that demonstrates [ProfanityCommentsAnalyzer 2.0.3](https://www.nuget.org/packages/ProfanityCommentsAnalyzer) — a Roslyn analyzer that detects profanity, threats, slurs, insults, and harsh code-critique language in **C# comments only**.

Word lists are stored in JSON. Built-in lists for **en**, **hu**, **de**, **ro**, and **it** ship inside the NuGet package. You can override or extend them with JSON files in your project.

---

## Prerequisites

- [.NET SDK 9.0](https://dotnet.microsoft.com/download) or later

```bash
dotnet --version
```

---

## What you need to set (and where)

There are three configuration layers. Only **step 1 is required** to get the analyzer running.

| Step | Required? | Where | Purpose |
|------|-----------|-------|---------|
| 1. NuGet package | **Yes** | `.csproj` | Installs the Roslyn analyzer |
| 2. Analyzer options | Recommended | `.editorconfig` or `.globalconfig` | Severity, languages, allow-list |
| 3. Custom word lists | Optional | `profanity/*.json` + `.csproj` | Add, remove, or change words |

### Step 1 — Install the NuGet package (required)

**Where:** your project file, e.g. [TestProfanityCommentsAnalyzer/TestProfanityCommentsAnalyzer.csproj](TestProfanityCommentsAnalyzer/TestProfanityCommentsAnalyzer.csproj)

**CLI:**

```bash
dotnet add TestProfanityCommentsAnalyzer/TestProfanityCommentsAnalyzer.csproj package ProfanityCommentsAnalyzer --version 2.0.3
```

**Or add manually to the `.csproj`:**

```xml
<ItemGroup>
  <PackageReference Include="ProfanityCommentsAnalyzer" Version="2.0.3">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
</ItemGroup>
```

Both `PrivateAssets` and `IncludeAssets` (including `analyzers` and `buildtransitive`) are required so MSBuild loads the analyzer at compile time.

No application code is needed. After install, run `dotnet build` and the analyzer runs automatically.

---

### Step 2 — Configure analyzer behavior (recommended)

**Where:** [.editorconfig](.editorconfig) at the solution/repo root, or `.globalconfig` with `is_global = true`

**What to set:**

```ini
root = true

[*.cs]
dotnet_diagnostic.PCA001.severity = warning

profanity_comments_analyzer.min_severity = mild
profanity_comments_analyzer.languages = en,hu,de,ro,it
profanity_comments_analyzer.allow_list =
```

| Setting | Default | Values | Effect |
|---------|---------|--------|--------|
| `dotnet_diagnostic.PCA001.severity` | `warning` | `warning`, `error`, `none` | How matches are reported |
| `profanity_comments_analyzer.min_severity` | `mild` | `mild`, `moderate`, `severe` | Only report entries at or above this level |
| `profanity_comments_analyzer.languages` | `en,hu,de,ro,it` | Comma-separated codes | Which language lists to check |
| `profanity_comments_analyzer.allow_list` | (empty) | Comma-separated words | Suppress exact matches (case-insensitive) |

**Examples:**

Only report moderate and severe profanity:

```ini
profanity_comments_analyzer.min_severity = moderate
```

Check English and Hungarian only:

```ini
profanity_comments_analyzer.languages = en,hu
```

Allow the word `hack` in comments:

```ini
profanity_comments_analyzer.allow_list = hack
```

Fail the build on every match:

```ini
dotnet_diagnostic.PCA001.severity = error
```

Or use MSBuild: `/p:TreatWarningsAsErrors=true` (without excluding PCA001).

---

### Step 3 — Custom word lists (optional)

**When you need this:**

| Scenario | AdditionalFiles required? |
|----------|---------------------------|
| Use built-in word lists only | **No** — skip this step entirely |
| Override or trim a language list | **Yes** |
| Add a new language (e.g. French) | **Yes** |
| Add team-specific banned phrases | **Yes** (`extra-patterns.json`) |

**Where:**

1. Create a `profanity/` folder inside your project (this repo: [TestProfanityCommentsAnalyzer/profanity/](TestProfanityCommentsAnalyzer/profanity/))
2. Register it in the `.csproj`:

```xml
<ItemGroup>
  <AdditionalFiles Include="profanity/**/*.json" />
</ItemGroup>
```

**Where to get the initial JSON files:**

Copy templates from the NuGet package after install:

```
~/.nuget/packages/profanitycommentsanalyzer/2.0.3/content/templates/profanity/
```

Or from this repo's [TestProfanityCommentsAnalyzer/profanity/](TestProfanityCommentsAnalyzer/profanity/) folder.

**Folder layout:**

```
TestProfanityCommentsAnalyzer/
  profanity/
    languages.json          ← which languages are active
    en.json                 ← all English words/patterns
    hu.json
    de.json
    ro.json
    it.json
    extra-patterns.json     ← optional cross-language phrases
```

---

## How to add, delete, or modify words

All language-specific words live in JSON files under [TestProfanityCommentsAnalyzer/profanity/](TestProfanityCommentsAnalyzer/profanity/). Edit the file for the language you want to change, then rebuild to verify.

### Language files

| Language | File | `"code"` value |
|----------|------|----------------|
| English | `profanity/en.json` | `"en"` |
| Hungarian | `profanity/hu.json` | `"hu"` |
| German | `profanity/de.json` | `"de"` |
| Italian | `profanity/it.json` | `"it"` |
| Romanian | `profanity/ro.json` | `"ro"` |

Each file has the same structure:

```json
{
  "code": "en",
  "name": "English",
  "entries": [
    {
      "word": "damn",
      "pattern": "\\bd[a@]mn\\b",
      "severity": "mild",
      "category": "profanity"
    }
  ]
}
```

| Field | Required | Description |
|-------|----------|-------------|
| `word` | Yes | Label shown in the PCA001 message |
| `pattern` | Yes | .NET regex matched against comment text (case-insensitive; do not use `(?i)`) |
| `severity` | Yes | `mild`, `moderate`, or `severe` |
| `category` | No | `profanity`, `threat`, `slur`, `poorCode`, `badPractice`, `confusion` |

Do **not** change `"code"` or `"name"` unless you rename the file itself. `"code"` must match the file name (`hu.json` → `"code": "hu"`).

After any edit, verify with:

```bash
dotnet build TestProfanity.sln -t:Rebuild -v minimal
```

---

### Add a new word

Open the language file and append a new object to the `"entries"` array. Add a comma after the previous entry.

**English — add `foobar` to `profanity/en.json`:**

Before (last entries):

```json
    {
      "word": "idiot",
      "pattern": "\\bidiot\\b",
      "severity": "mild",
      "category": "profanity"
    }
  ]
}
```

After:

```json
    {
      "word": "idiot",
      "pattern": "\\bidiot\\b",
      "severity": "mild",
      "category": "profanity"
    },
    {
      "word": "foobar",
      "pattern": "\\bfoobar\\b",
      "severity": "mild",
      "category": "profanity"
    }
  ]
}
```

**Hungarian — add `barom` to `profanity/hu.json`:**

```json
    {
      "word": "barom",
      "pattern": "\\bbarom\\b",
      "severity": "mild",
      "category": "profanity"
    }
```

**German — add `Blödsinn` to `profanity/de.json`:**

```json
    {
      "word": "Blödsinn",
      "pattern": "\\bBl[oö]dsinn\\b",
      "severity": "mild",
      "category": "profanity"
    }
```

**Italian — add `merda` to `profanity/it.json`:**

```json
    {
      "word": "merda",
      "pattern": "\\bmerda\\b",
      "severity": "mild",
      "category": "profanity"
    }
```

**Romanian — add `prost` to `profanity/ro.json`:**

```json
    {
      "word": "prost",
      "pattern": "\\bprost\\b",
      "severity": "mild",
      "category": "profanity"
    }
```

Use `\\b` at the start and end of the pattern to match whole words only. Use character classes for common substitutions (e.g. `@` for `a`, `0` for `o`).

---

### Delete a word

Find the entry in the `"entries"` array and remove the entire `{ ... }` block. Remove the trailing comma from the entry above if it becomes the last one.

**English — remove `damn` from `profanity/en.json`:**

Before:

```json
    {
      "word": "wtf",
      "pattern": "\\bw[t*][f*]\\b",
      "severity": "mild",
      "category": "profanity"
    },
    {
      "word": "damn",
      "pattern": "\\bd[a@]mn\\b",
      "severity": "mild",
      "category": "profanity"
    },
    {
      "word": "crap",
      "pattern": "\\bcr[a@]p\\b",
      "severity": "mild",
      "category": "profanity"
    }
```

After:

```json
    {
      "word": "wtf",
      "pattern": "\\bw[t*][f*]\\b",
      "severity": "mild",
      "category": "profanity"
    },
    {
      "word": "crap",
      "pattern": "\\bcr[a@]p\\b",
      "severity": "mild",
      "category": "profanity"
    }
```

The same steps apply to any language file — e.g. delete `kurva` from `profanity/hu.json`, `Mist` from `profanity/de.json`, `casino` from `profanity/it.json`, or `naiba` from `profanity/ro.json` by removing that entry object.

Comments containing the deleted word will no longer trigger PCA001 after rebuild.

---

### Modify a word

Find the existing entry and change one or more fields. Common changes: severity, regex pattern, or display label.

**English — change `damn` from mild to moderate in `profanity/en.json`:**

Before:

```json
    {
      "word": "damn",
      "pattern": "\\bd[a@]mn\\b",
      "severity": "mild",
      "category": "profanity"
    }
```

After:

```json
    {
      "word": "damn",
      "pattern": "\\bd[a@]mn\\b",
      "severity": "moderate",
      "category": "profanity"
    }
```

With `profanity_comments_analyzer.min_severity = mild` in `.editorconfig`, severity changes have no visible effect. Set `min_severity = moderate` to hide mild matches, or set `min_severity = severe` to hide mild and moderate matches.

**Hungarian — widen the regex for `szar` in `profanity/hu.json` to catch `sz@r`:**

Before:

```json
    {
      "word": "szar",
      "pattern": "\\bsz[a@]r\\b",
      "severity": "mild",
      "category": "profanity"
    }
```

After (same pattern, already covers `@`; change the label instead):

```json
    {
      "word": "szar/sz@r",
      "pattern": "\\bsz[a@]r\\b",
      "severity": "moderate",
      "category": "profanity"
    }
```

**German — rename label and tighten category for `Mist` in `profanity/de.json`:**

```json
    {
      "word": "Mist",
      "pattern": "\\bMist\\b",
      "severity": "mild",
      "category": "badPractice"
    }
```

**Italian — change severity for `casino` in `profanity/it.json`:**

```json
    {
      "word": "casino",
      "pattern": "\\bcasino\\b",
      "severity": "severe",
      "category": "profanity"
    }
```

**Romanian — update pattern for `dracu` in `profanity/ro.json`:**

```json
    {
      "word": "dracu",
      "pattern": "\\bdr[a@]cu\\b",
      "severity": "mild",
      "category": "profanity"
    }
```

---

### Quick reference

| Action | Steps |
|--------|-------|
| **Add** | Open `profanity/{code}.json` → add `{ ... }` to `"entries"` → rebuild |
| **Delete** | Open `profanity/{code}.json` → remove the `{ ... }` block → fix commas → rebuild |
| **Modify** | Open `profanity/{code}.json` → edit fields in the existing `{ ... }` block → rebuild |

| Language code | File to open |
|---------------|--------------|
| `en` | `profanity/en.json` |
| `hu` | `profanity/hu.json` |
| `de` | `profanity/de.json` |
| `it` | `profanity/it.json` |
| `ro` | `profanity/ro.json` |

---

### Which file to edit (other tasks)

| Task | File to edit |
|------|--------------|
| Turn a language on or off | `profanity/languages.json` and/or `.editorconfig` `profanity_comments_analyzer.languages` |
| Add a new language (e.g. French) | Create `profanity/fr.json` + add `"fr"` to `languages.json` |
| Add a phrase for all languages | `profanity/extra-patterns.json` |

### `languages.json` — language manifest

Declares which languages the analyzer loads. Every code listed here **must** have a matching `{code}.json` file.

```json
{
  "languages": ["en", "hu", "de", "ro", "it"]
}
```

- **Remove a language:** delete its code from the `"languages"` array (and optionally delete `{code}.json`)
- **Add a language:** add the code here and create `{code}.json` with at least one entry

When you provide `languages.json` via `AdditionalFiles`, it **replaces** the built-in manifest.

When you provide a `{code}.json` via `AdditionalFiles`, it **fully replaces** the built-in embedded list for that language — it does **not** merge. Languages you do not override keep their built-in lists.

Example: you provide only `profanity/hu.json`. English, German, Italian, and Romanian still use embedded defaults; Hungarian uses your file.

---

### `extra-patterns.json` — cross-language phrases

Use for company-specific terms, internal codenames, or phrases that should match **regardless of language settings**.

Unlike `{code}.json` files, entries here are **appended** on top of language lists (not a replacement).

```json
{
  "entries": [
    {
      "word": "company banned phrase",
      "pattern": "company\\s+banned\\s+phrase",
      "severity": "moderate"
    }
  ]
}
```

Diagnostics show `custom` as the language label. This file is always checked, even if you limit `profanity_comments_analyzer.languages` in `.editorconfig`.

| Use `{code}.json` | Use `extra-patterns.json` |
|-------------------|---------------------------|
| Standard profanity in a given language | Phrases that apply to all languages |
| Replacing or trimming a built-in language list | A few team-specific terms without editing five language files |
| Adding a new language (e.g. `fr.json`) | Regex that should match regardless of language filter |

---

### Merge and replace behavior (summary)

| Source | Effect |
|--------|--------|
| Embedded JSON in the NuGet package | Default word lists; always loaded first |
| Your `languages.json` | Replaces the embedded manifest |
| Your `{code}.json` | Replaces embedded list for that code only |
| Your `extra-patterns.json` | Appends extra patterns |
| Languages not overridden | Keep using embedded `{code}.json` |

---

### Validation

Custom JSON is validated at build time. Invalid files fail the build with a descriptive error:

| Problem | Result |
|---------|--------|
| Code in `languages.json` but missing `{code}.json` | Build fails |
| `{code}.json` empty or invalid JSON | Build fails |
| Entry missing `word` or `pattern` | Build fails |

Every code in `languages.json` must have a corresponding `{code}.json` with at least one complete entry.

---

## Solution structure (this repo)

```
.editorconfig                            # Step 2: analyzer options
TestProfanity.sln
TestProfanityCommentsAnalyzer/
  TestProfanityCommentsAnalyzer.csproj   # Step 1: NuGet + Step 3: AdditionalFiles
  profanity/                             # Step 3: custom word lists
    languages.json
    en.json, hu.json, de.json, ro.json, it.json
    extra-patterns.json
  test-en.cs                             # Sample comments (English)
  test-hu.cs                             # Sample comments (Hungarian)
  test-de.cs                             # Sample comments (German)
  test-it.cs                             # Sample comments (Italian)
  test-ro.cs                             # Sample comments (Romanian)
```

---

## How profanity checking works

The analyzer runs at **compile time**. You do not call it from C# code.

| Analyzed | Not analyzed |
|----------|--------------|
| `//` single-line comments | String literals |
| `/* */` multi-line comments | Identifiers |
| `///` and `/** */` doc comments | `.cshtml`, `.razor`, VB files |
| `.cs` files only | |

Build pipeline:

1. Read `.editorconfig` / `.globalconfig` options
2. Load embedded JSON from the NuGet package + optional `AdditionalFiles`
3. Scan comment text in each `.cs` file
4. Report **PCA001** for each match

Example diagnostic:

```
warning PCA001: [profanity-in-comments] "damn" (en, severity: mild) found in comment
```

---

## Commands to run the profanity check

Run from the repository root.

### List all warnings on the console

Incremental builds skip recompilation when nothing changed, so warnings may not appear. Use **Rebuild** to always run the analyzer:

```bash
dotnet build TestProfanity.sln -t:Rebuild -v minimal
```

Alternative:

```bash
dotnet clean TestProfanity.sln
dotnet build TestProfanity.sln --no-incremental -v minimal
```

| Build output | Meaning |
|--------------|---------|
| ~0.3s, no PCA001 lines | Compile skipped — run Rebuild |
| ~1.5s+, lists PCA001 lines | Full scan completed |

### Fail the build when profanity is found

```bash
dotnet build TestProfanity.sln -t:Rebuild /p:TreatWarningsAsErrors=true -v minimal
```

Or set `dotnet_diagnostic.PCA001.severity = error` in `.editorconfig`.

### Other useful commands

```bash
dotnet restore TestProfanity.sln
dotnet build TestProfanityCommentsAnalyzer/TestProfanityCommentsAnalyzer.csproj -t:Rebuild -v minimal
```

---

## Expected build output

After a full rebuild:

```
test-en.cs(5,5): warning PCA001: [profanity-in-comments] "damn" (en, severity: mild) found in comment
test-hu.cs(5,5): warning PCA001: [profanity-in-comments] "szar" (hu, severity: mild) found in comment
test-de.cs(5,5): warning PCA001: [profanity-in-comments] "Mist" (de, severity: mild) found in comment
test-it.cs(5,5): warning PCA001: [profanity-in-comments] "casino" (it, severity: moderate) found in comment
test-ro.cs(5,5): warning PCA001: [profanity-in-comments] "naiba" (ro, severity: mild) found in comment

Build succeeded.
    12 Warning(s)
    0 Error(s)
```

The string literal `"damn"` in `test-en.cs` does **not** produce a warning — only comments are checked.

---

## Minimal setup vs full setup

**Minimal (built-in lists only):**

1. Add the NuGet package to `.csproj` (step 1)
2. Optionally add `.editorconfig` (step 2)
3. Run `dotnet build -t:Rebuild`

**Full (custom word lists):**

1. Add the NuGet package to `.csproj` (step 1)
2. Add `.editorconfig` (step 2)
3. Copy `profanity/` templates, register `AdditionalFiles` in `.csproj` (step 3)
4. Edit JSON files to add/remove/modify words
5. Run `dotnet build -t:Rebuild`

---

## License

See [LICENSE](LICENSE).
