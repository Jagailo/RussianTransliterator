# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [1.1.0] - 2026-10-26

### Added
- `ToLatin` method, also available as a string extension method (`"Текст".ToLatin()`).
- Nullable annotations: `null` input returns `null`, and for a non-null argument the compiler knows the result is not `null`.
- XML documentation for IntelliSense, symbol package (`.snupkg`) and Source Link for debugging into the library source.

### Changed
- **Breaking:** `ый` is transliterated as `yy` instead of `yu` (`Белый` -> `Belyy`).
- **Breaking:** `ий` is transliterated as `y` only at the end of a word (`Бийск` -> `Biysk`, `Василий` -> `Vasily`).
- **Breaking:** `х` after `к` is transliterated as `h`, as after vowels (`ЖКХ` -> `ZHKH` instead of `ZHKKH`, `Кхмер` -> `Khmer`).
- Retargeted back to .NET Standard 2.0, so .NET Framework is supported again.
- Transliteration is 40 to 170 times faster and allocates almost nothing beyond the result string
  (a short title: 10.7 µs -> 0.1 µs; the full text of "War and Peace": 618 ms -> 15 ms and 1.3 GB -> 15 MB allocated on .NET 10).
- A string without Russian letters is returned as is, without allocating a new one.
- The assembly is now strong-named.
- License changed from MIT to MIT-0.

### Deprecated
- `GetTransliteration` is obsolete, use `ToLatin` instead.

### Fixed
- Letters that map to several Latin letters are fully uppercased at the end of an uppercase word (`ПЛЯЖ МОРЯ` -> `PLYAZH MORYA`).
- A single-letter uppercase word follows the case of the neighbouring words (`Я ЛЮБЛЮ` -> `YA LYUBLYU`).
- The result no longer depends on the current culture (`И` was transliterated as `İ` under `tr-TR`).

## [1.0.1.1] - 2023-03-05

### Changed
- Retargeted from .NET Standard 2.0 to .NET Standard 2.1. .NET Framework is no longer supported.
- Enabled nullable reference types.

### Added
- README and icon are now included in the package.

## [1.0.1] - 2019-08-11

### Changed
- **Breaking:** renamed the namespace and the assembly from `RussianTransliterator` to `RussianTransliteration`.

### Added
- README.

## [1.0.0] - 2019-08-11

### Added
- Initial release with the `RussianTransliterator.GetTransliteration` method.
