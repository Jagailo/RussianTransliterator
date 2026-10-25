# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [Unreleased]

### Changed
- **Breaking:** `ый` is transliterated as `yy` instead of `yu` (`Белый` -> `Belyy`).
- **Breaking:** `ий` is transliterated as `y` only at the end of a word (`Бийск` -> `Biysk`, `Василий` -> `Vasily`).
- **Breaking:** `х` after `к` is transliterated as `h`, as after vowels (`ЖКХ` -> `ZHKH` instead of `ZHKKH`, `Кхмер` -> `Khmer`).

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
