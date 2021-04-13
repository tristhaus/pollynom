# PollyNom - a math teaching tool pretending to be a game 

Copyright tristhaus 2018 and later.

## For Users

PollyNom is a game wherein you attempt to hit as many good dots as possible without hitting any bad dots using as few mathematical functions as possible.

![main](/../screenshot/screenshot.png?raw=true)

Scoring is simple: the more good dots (dark blue) your function hits, the higher the score. If you hit a bad dot (light red), your score becomes negative infinity. Basic support for creation of random games, saving, and loading is provided.

Currently supported functions/operations are:
| Name  | Code  |
|---|---|
| Independent variable | `x` | 
| Basic arithmetic | `+ - * \` |
| Power | `^` |
| Grouping |  `()` |
| Absolute value | `abs` |
| Sine | `sin` |
| Cosine | `cos` |
| Tangent | `tan` |
| Exponential function to base *e* | `exp` |
| Natural logarithm, base *e* | `ln` |

The named functions require that the argument be placed in parenthesis. A valid function is thus:

    abs(x^2.3 + x*(sin(x)))

The complexity of the function is currently limited only by the capabilities of the machine used to run the executable. The project is currently in beta state.

Have fun!

## For Developers

I welcome contributions if they address open issues as listed in the tab.

## License

All source code licensed under GPL v3 (see LICENSE for terms).

## Attributions

Icon (axes and graph) attributed to: Icons made by [Pixel perfect](https://www.flaticon.com/authors/pixel-perfect) from [Flaticon](https://www.flaticon.com/)
