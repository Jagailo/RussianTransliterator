# Russian Transliterator

![GitHub License](https://img.shields.io/github/license/Jagailo/RussianTransliterator)
![NuGet Downloads](https://img.shields.io/nuget/dt/RussianTransliterator?label=NuGet%20downloads)

.NET library for transliterating the cyrillic to latin. Only Russian language is supported.

# Installation
[NuGet](https://www.nuget.org/packages/RussianTransliterator/):
```
Install-Package RussianTransliterator
```

# Usage
```CSharp
var latin = RussianTransliterator.ToLatin("Какой-нибудь русский текст");
// or
var latin = "Какой-нибудь русский текст".ToLatin();
```

# Examples
| №  | Cyrillic                 | Latin                       |
|----|--------------------------|-----------------------------|
| 1  | Щука                     | Shchuka                     |
| 2  | Белый                    | Belyy                       |
| 3  | БоСоЙ                    | BoSoY                       |
| 4  | КАЛИЙ                    | KALY                        |
| 5  | музей                    | muzey                       |
| 6  | Негодяй                  | Negodyay                    |
| 7  | Труха                    | Truha                       |
| 8  | Вверх                    | Vverkh                      |
| 9  | деревья                  | derevya                     |
| 10 | сверхъестественное       | sverkhestestvennoe          |
