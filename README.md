# Russian Transliterator
Library for transliterating the cyrillic to latin.

Only Russian language is supported.

# Usage
```CSharp
var latin = RussianTransliterator.GetTransliteration("Какой-нибудь русский текст");
```

# Examples
| №  | Cyrillic                 | Latin                       |
|----|--------------------------|-----------------------------|
| 1  | Щука                     | Shchuka                     |
| 2  | ЩУКИН ЕВГЕНИЙ НИКОЛАЕВИЧ | SHCHUKIN EVGENY NIKOLAEVICH |
| 3  | Белый                    | Belyu                       |
| 4  | БоСоЙ                    | BoSoY                       |
| 5  | КАЛИЙ                    | KALY                        |
| 6  | музей                    | muzey                       |
| 7  | Негодяй                  | Negodyay                    |
| 8  | Труха                    | Truha                       |
| 9  | Вверх                    | Vverkh                      |
| 10 | ЯгЕлО аЛеКсЕй СеРгЕеВиЧ  | YagElO aLeKsEy SeRgEeViCH   |