# WhyNotLang [![Build Status](https://travis-ci.org/pkedziora/WhyNotLang.svg?branch=master)](https://travis-ci.org/pkedziora/WhyNotLang)

Programming language and interpreter created for fun

## Online Playground
[https://whynotlang.kedziora.dev/](https://whynotlang.kedziora.dev/)

## Syntax Reference
[Reference](src/WhyNotLang.Samples/REFERENCE.md)

## Implementation
* Core (C#, .NET Standard 2.0)
    * Tokenizer
    * Parser
    * Interpreter
* Web test area (JavaScript, C#, [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor), [.NET Core 3.0 preview8](https://github.com/dotnet/core/blob/master/release-notes/3.0/preview/3.0.0-preview8.md))
* Command line executable (C#, .NET Core 2.2)

## Example
```
func QuickSort(array, first, last)
begin
    if (last <= first)
        return 0

    var pivotIndex := Partition(array, first, last)
    QuickSort(array, first, pivotIndex - 1)
    QuickSort(array, pivotIndex + 1, last)
end

func Partition(array, first, last)
begin
    var pivotIndex := first
    var pivot := array[first]
    var i:= first
    while (i <= last)
    begin
        if (array[i] < pivot)
        begin
            pivotIndex := pivotIndex + 1
            Swap(array, pivotIndex, i)
        end

        i := i + 1
    end

    Swap(array, pivotIndex, first)

    return pivotIndex
end
```

## Interactive Samples
* [Pong game](https://whynotlang.kedziora.dev/sample/pong)
* [QuickSort](https://whynotlang.kedziora.dev/sample/quicksort)
* [Fibonacci](https://whynotlang.kedziora.dev/sample/fibonacci)

## Features
* Dynamic typing
* Block scope support (variables, arrays)
* Global scope support (functions, variables, arrays)
* Boolean expressions (evaluating to 0 or 1)
* Integer arithmetic expressions: +,-,*,/, ( )
* While loop
* If statement
* Functions (User defined)
    * Array parameters passed by reference
* Functions (Built in)
    * ToNumber(str)
    * ToString(num)
    * Delay(ms)
    * Random(min, max)
* Text IO
    * Writeln(str)
    * Readln()
* Graphics IO
    * ClearScreen()
    * DrawRectangle(x, y, width, height,color)
    * DrawText(text, x, y, color, font)
    * Events (Built in)
        * OnKeyDown(key)
        * OnKeyUp(key)


