# Russian Transliterator

[![NuGet](https://img.shields.io/nuget/v/RussianTransliterator?logo=nuget&label=NuGet)](https://www.nuget.org/packages/RussianTransliterator/)
[![Downloads](https://img.shields.io/nuget/dt/RussianTransliterator?label=Downloads)](https://www.nuget.org/packages/RussianTransliterator/)
[![Build and tests](https://img.shields.io/github/actions/workflow/status/Jagailo/RussianTransliterator/ci.yml?branch=master&label=Build%20%26%20tests)](https://github.com/Jagailo/RussianTransliterator/actions/workflows/ci.yml)
![.NET Standard 2.0](https://img.shields.io/badge/.NET%20Standard-2.0-512BD4)

A small .NET library (NuGet package) that converts Russian Cyrillic text to Latin letters.

```text
Привет, мир!  ->  Privet, mir!
```

Use it for URLs, file names, logins, email addresses, search and any place where only Latin letters are allowed.

## Why this library

- **Correct letter case.** One Russian letter can become several Latin letters (`Щ` -> `Shch`).
  The library looks at the word and chooses the right case, so you get `SHCHUKA`, not `ShchUKA`.
- **Fast and saves memory.** It reads the text only once. A text without Russian letters is returned as is, without new memory.
- **Very small.** One class, about 9 KB, no dependencies.
- **Works everywhere.** It targets .NET Standard 2.0: .NET 5 and newer, .NET Core 2.0+, .NET Framework 4.6.1+, Mono, Unity and more.
- **Same result on every computer.** The result does not depend on the current culture of the system.
- **Safe for other text.** Numbers, Latin letters, punctuation, emoji and letters of other alphabets stay unchanged.

How the letter case works:

| Text             | Russian Transliterator | Simple letter replacement |
|------------------|------------------------|---------------------------|
| `Щука`           | `Shchuka`              | `Shchuka`                 |
| `ЩУКА`           | `SHCHUKA`              | `ShchUKA`                 |
| `Журнал «ЖИЗНЬ»` | `Zhurnal «ZHIZN»`      | `Zhurnal «ZhIZN»`         |
| `Я ЛЮБЛЮ ЧАЙ`    | `YA LYUBLYU CHAY`      | `Ya LYuBLYu ChAY`         |

## Installation

```shell
dotnet add package RussianTransliterator
```

Or in the Package Manager Console in Visual Studio:

```powershell
Install-Package RussianTransliterator
```

## Usage

```csharp
using RussianTransliteration;

var latin = RussianTransliterator.ToLatin("Какой-нибудь русский текст");

// or as an extension method
var latin = "Какой-нибудь русский текст".ToLatin();
```

## Examples

| Russian                     | Latin                        |
|-----------------------------|------------------------------|
| `Яга`                       | `Yaga`                       |
| `ЖКХ`                       | `ZHKH`                       |
| `Белый`                     | `Belyy`                      |
| `Василий`                   | `Vasily`                     |
| `Бийск`                     | `Biysk`                      |
| `Труха`                     | `Truha`                      |
| `Вверх`                     | `Vverkh`                     |
| `деревья`                   | `derevya`                    |
| `сверхъестественное`        | `sverkhestestvennoe`         |
| `iPhone в Минске`           | `iPhone v Minske`            |
| `Отчёт_за_Январь_2026.docx` | `Otchet_za_Yanvar_2026.docx` |

## Rules

| Russian | Latin | Russian | Latin | Russian | Latin |
|---------|-------|---------|-------|---------|-------|
| а       | a     | к       | k     | х       | kh    |
| б       | b     | л       | l     | ц       | ts    |
| в       | v     | м       | m     | ч       | ch    |
| г       | g     | н       | n     | ш       | sh    |
| д       | d     | о       | o     | щ       | shch  |
| е       | e     | п       | p     | ъ       |       |
| ё       | e     | р       | r     | ы       | y     |
| ж       | zh    | с       | s     | ь       |       |
| з       | z     | т       | t     | э       | e     |
| и       | i     | у       | u     | ю       | yu    |
| й       | y     | ф       | f     | я       | ya    |

Special rules:

- `ъ` and `ь` are removed: `Объявление` -> `Obyavlenie`.
- `х` after a vowel or after `к` becomes `h`: `Труха` -> `Truha`, `ЖКХ` -> `ZHKH`.
- `ий` at the end of a word becomes `y`: `Василий` -> `Vasily`, but `Бийск` -> `Biysk`.

## Benchmarks

Time and memory for one `ToLatin` call, measured with [BenchmarkDotNet](https://benchmarkdotnet.org/) on AMD Ryzen 7 7800X3D:

| Text                                                                                                                 | Characters |   .NET&nbsp;10 | .NET&nbsp;Framework&nbsp;4.8 |       Memory |
|----------------------------------------------------------------------------------------------------------------------|-----------:|---------------:|-----------------------------:|-------------:|
| `Как выбрать тариф ЖКХ в 2026 году: советы и примеры`                                                                |         51 | 104.62&nbsp;ns |               218.02&nbsp;ns |   320&nbsp;B |
| `КАК ВЫБРАТЬ ТАРИФ ЖКХ В 2026 ГОДУ: СОВЕТЫ И ПРИМЕРЫ`                                                                |         51 | 123.62&nbsp;ns |               327.82&nbsp;ns |   320&nbsp;B |
| `Архив ЮРИДИЧЕСКИЙ отдел ЖКХ Щёлково ОХРАНА ТРУДА Счета и ПЛАТЁЖ за Январь–Июнь 2026 (Шаблоны ЦЕНЫ v2) ИТОГОВЫЙ.zip` |        114 | 304.44&nbsp;ns |               717.22&nbsp;ns |   600&nbsp;B |
| `How to choose a utility tariff in 2026: tips and examples`                                                          |         57 |  18.78&nbsp;ns |                24.53&nbsp;ns |     0&nbsp;B |
| The full text of the novel "War and Peace"                                                                           |  3,606,380 |  15.07&nbsp;ms |                20.32&nbsp;ms | 14.9&nbsp;MB |

To run the benchmarks yourself:

```shell
# short texts
dotnet run -c Release -f net10.0 --project benchmarks/RussianTransliterator.Benchmarks -- --filter "*ToLatinBenchmarks*"

# "War and Peace"
dotnet run -c Release -f net10.0 --project benchmarks/RussianTransliterator.Benchmarks -- --filter "*BookBenchmarks*"
```

## License

[MIT-0](https://github.com/Jagailo/RussianTransliterator/blob/master/LICENSE): you can use this library for free in any project, including commercial ones, and you don't need to mention the author.
