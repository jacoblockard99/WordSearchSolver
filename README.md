# WordSearchSolver

[![Maintainability](https://api.codeclimate.com/v1/badges/eff8c5c768f4d7738011/maintainability)](https://codeclimate.com/github/jacoblockard99/WordSearchSolver/maintainability)

## Introduction

WordSearchSolver is a simple little console application written in C# that solves word search puzzles, given a list of search words.

## Installation (or lack thereof)

No official installer curently exists for WordSearchSolver. Standalone compiled executables are available on the GitHub releases page. Additionally, in this document, a bash alias has been set like so:

```bash
# ~/.bashrc
alias wordsearchsolver="/home/{user}/RiderProjects/WordSearchSolver/WordSearchSolverConsole/bin/Debug/net5.0/WordSearchSolverConsole"
```

You will thus see commands like `wordsearchsolver -f wordsearch.txt -w "Word"`. Depending on your setup, you may have to run something more akin to `./WordSearchSolverConsole ...`.

## Background

A word search puzzle is composed of a uniform grid of letters of arbitrary dimensions. Most of the letters are random, but within the grid a number of secret words are hidden. These words can start from any location, be any length, and go in any horizontal, vertical, or diagonal direction. The goal of the WordSeachSolver application is to find the locations of these hidden words. It does so by iterating over all the characters in the grid, trying words of all lengths and directions until matches are found.

## Usage

### Basic Usage

At minimum, two pieces of information are required to invoke the WordSearchSolver: a word search puzzle and a list of search words. These can be specified with the following options:

- **--word-search** — A raw string of characters representing a word search puzzle. Rows are seperated with semicolons (`;`). All whitespace is ignored.
- **--words, -w** — A string containing the list of search words, seperated with semicolons (`;`). Empty words are not allowed.
- **--word-search-file, -f** — A path to a file containing a word search puzzle. Within the file, rows are seperated with newlines. Whitespace and empty lines are ignored. This option takes precedence over **--word-search**.
- **--words-file** — A path to a file containing the list of search words, seperated by newlines. Empty lines are not allowed. This option takes precedence over **--words, -w**.

For example, consider this word search:

```
A C C
D A F
G T I
```

To solve this word search, this command can be run:

```
wordsearchsolver --word-search "ACC;DAF;GTI" --words "CAT:
```

This will output:

<pre>
Solved:

A <b>C</b> C
D <b>A</b> F
G <b>T</b> I

Coordinates to Solutions:

'CAT': (1, 0) through (1, 2)
</pre>

Or, take this word search file ("example.txt"):

```
B T C
D A F
G H T
```

And this search word file ("example-words.txt"):

```
BAT
HAT
```

The solution(s) can be found with this command:

```
wordsearchsolver --word-search-file "example.txt" --words-file "example-words.txt"
```

The result:

<pre>
Solved:

<b>B</b> <b>T</b> C
D <b>A</b> F
G <b>H</b> <b>T</b>

Coordinates to Solutions:

'BAT': (0, 0) through (2, 2)
'HAT': (1, 2) through (1, 0)
</pre>

### Overlapping Words

Typically, multiple words starting from the same location in the same direction are not allowed. For example, take this word search:

```
A B C K E F G
H I J E L M N
O P Q Y S T U
V W X W Z A B
C D E O G H I
J K L R N O P
Q R S D U V W
X Y Z S B C D
```

There are three words in this puzzle: "KEY", "KEYWORD", and "KEYWORDS". But they all start from location (3, 0) and point directly downward. Because of the way the solving algorithm works, it is less efficient to check for such words. Thus, by default, only the first match ("KEY") is caught. This behavior can be overriden with the **--allow-overlapping, -o** flag.

### Formatting

The application accepts several options to control formatting of word searches.

#### Spacing

To control the spacing of formatted word searches, two options are used:

- **--hspacing, -h** — Controls the number of spaces placed between each column of a word search. The default is `1`.
- **--vspacing, -v** — Controls the number of lines placed between each row of a word search. The default is `0`.

#### Solutions Format

While the WordSearchSolver displays the coordinates of the solutions after solving the word search, it also has the ability to format the word search grid itself in such a way that the solutions are obvious. This can be done in several ways, determined by  the **--solutions-format** option. The options are:

- **asterisk** — Replaces any character that is part of a solution with an asterisk (`*`). This method is only reccomended if there are very few words in the word search. With too many words, the word search can become illegible.
- **parentheses** — Wraps solution characters in parentheses. `A`, for example, becomes `(A)`. Note that, because of the parentheses, an `hspacing` value of at least 2 is required when using this method.
- **color** — Highlights each solution in a different color. This is the default method and the recomended one for terminals that support ANSI 256-color codes. Note that this method has only been confirmed to work on Ubuntu.
- **single-color** — Highlights each solution in red. This is the recomended method for terminals that support ANSI 8-color codes. Note that this method has only been confirmed to work on Ubuntu.
- **none** — Adds no special formatting to the solutions.

## How it Works

### Structure

WordSearchSolver is divided into two primary parts: the WordSearchSolver class library and the WordSearchSolverConsole console application. The class library contains the representative classes and solving logic. The console application is the actual front-end. In the future, other interfaces (desktop application, phone application, etc.) may be added.

### Word Searches

A word search is defined as a uniform grid of letters within which are hidden secret words. This is represented in the WordSearchSolver by the `WordSearch` class. The `WordSearch` class internally uses a `char[,]` array to store this grid. `WordSearch` has several useful functions, including: the ability to parse word search grids given a list of rows/lines (`.Parse()`), the ability to retrieve a word at the specified location (`.GetWord()`), and the ability to validate grid coordinates (`.InBounds()`).

There are two important points to understand about `WordSearch`:

1. It uses a `char[,]` (two-dimensional) array rather than a jagged array. Thus, a jagged word search *cannot* exist. Of course, when parsing word searches, extra effort must be expended in order to convert the standard "array of arrays" to a two-dimensional array. There are advantages, however: 1) it better represents the data, and 2) it is self-validating.
2. `WordSearch` represents only the word search itself and not any of its solutions. These are represented seperately by immutable `WordLocation` instances (see below).

### Word Locations

Throughout the application, the location of a "word" in a word search must be referred to. This is represented with the immutable `WordLocation` class. Note that a `WordLocation` is entirely seperate from a `WordSearch`. Thus, a `WordLocation` may refer to a completely different word in one `WordSearch` than in another! `WordLocation` merely represents the **location** of a word, not the word itself.

A word in a word search is essentially a series of adjacent characters that lie along the same line. A character is adjacent to another if it is one "step" away, i.e. immediately to the  north, northeast, east, southeast, south, southwest, west, or northwest. This is represented in the `WordLocation` class in three parts:

1. **A Starting Location** — The row and column in which the word begins.
2. **A Direction** — The direction in which the word points. Represented with two integers, `DirectionX` and `DirectionY`, each of which may be `-1`, `0`, or `1`. An `X` value of `-1` indicates that the word points left, `1` indicates right, and `0` indicates that the word has no `X` direction at all, but rather points straight up or straight down. Similarly, a `Y` value of `-1` indicates up, `1` indicates down, and `0` indicates that the word is either straight left or straight right. Naturally, both directions cannot be zero at the same time.
3. **A Length** — How many characters the word extends. Cannot be negative—for backwards words, an opposite direction should be used.

The reasons why this method was preferred over using a starting location and an ending location are threefold:

1. **It better represents the data.** It is not possible in a word search to have a diagonal line with a slope other than 1 or -1. Using the direction method described above, only valid lines are even possible. With the start/end coordinate method, validations would be needed to ensure that, for example, a line from (0, 0) to (1, 2) is not possible.
2. **The direction method fits the solving algorithm.** Because the solving algorithm (see below) uses a very similar process—starting from one location and moving in a certain direction so many times—no conversion is necessary with the direction method to get a `WordLocation` when a match is found.
3. **The direction method makes it easier to check for constituent characters.** In order to properly format solutions on a word search grid, the application must be able to check whether a certain coordinate is contained in a `WordLocation`. This is far easier with the direction method than otherwise.

### The Solving Algorithm

The final major piece in the WordSearchSolver class library is the solving algorithm, contained in the `IterativeSolver` class. The goal of this algorithm is to discover the locations of all the instances of every word in the search word list. The steps that `IterativeSolver` uses to accomplish this are:

1. Iterates over every row and column (and thus, character) in the word search.
2. For each one, iterates over all eight valid combinations of direction integers.
3. For each combination, recursively searches for a match in the given direction until the word search bounds are reached, no match is possible, or (if overlapping words are disallowed) until a match is found.

Step 3 is executed by the `IterativeSolver.TryCharacterInDirection()` method, which accepts as parameters the coordinates to check, the direction in which to check, and two parameters used by the recursive process: the current word and the current number of characters the search is extended. It does the following:

1. Calculates the current row/column given the starting one and the number of characters the search is extended.
2. Quits if the search has extended beyond the bounds of the word search.
3. Adds the new character to the current word.
4. Determines 1) if the current word is a match, and 2) if a match is even possible (i.e. if any search word begins with the current word).
5. Quits if a match is impossible.
6. Adds a match if one was found, quitting afterwards unless overlapping words are allowed.
7. Extends the search one character if no match was found.
