# WhyNotLang [![Build Status](https://travis-ci.org/pkedziora/WhyNotLang.svg?branch=master)](https://travis-ci.org/pkedziora/WhyNotLang)

Programming language and interpreter created for fun

## Online Playground
[https://whynotlang.kedziora.dev/](https://whynotlang.kedziora.dev/)

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

## Features
* Dynamic typing
* Block scope support (variables, arrays)
* Global scope support (functions, variables, arrays)
* Boolean expressions (evaluating to 0 or 1)
* Arithmetic expressions: +,-,*,/, ( )
* While loop
* If statement
* Functions (User defined)
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

## Implementation
* Core (C#, .NET Standard 2.0)
    * Tokenizer
    * Parser
    * Interpreter
* Command line executable (C#, .NET Standard 2.0)
* Web test area (JavaScript, C# (Blazor.NET))

## Syntax


