# Reference

## Statements

### Variable Declaration
#### Local
```
var identifier := expression

e.g. var foo := 1
```
#### Global
```
global identifier := expression

e.g. global foo := "lorem"
```
### Variable Assignment
```
identifier := expression

e.g. foo := 1 + 2 * 3
```
### Array Declaration
Note: All indexer expressions need to evaluate to a number
#### Local
```
var identifier[expression]

e.g. var foo[10]
```
#### Global
```
global identifier[expression]

e.g. var foo[2 * 10]
```
### Array Assignment
```
identifier[expression] := expression

e.g. foo[0] := 1
```
### Block
```
begin
statement1
...
statementN
end

e.g.
begin
    var name := Readln()
    Writeln("Hello " + name)
end
```
### Function Declaration
```
func identifier(param1, ..., paramN)
begin
    statement1
    ...
    statementN
end

e.g.
func Square(n)
begin
    return n * n
end
```
### Function Call
```
identifier(argument1, ..., argumentN)

e.g. Writeln("Hello World!")
```
### If, else
Note: Conditional expressions are considered false when they evaluate to 0. For non zero values they are considered true
#### Without blocks
```
if (expression1)
    statement1
else if (expression2)
    statement2
else
    statement3

e.g.
if (n < 10 or n < 0)
    Writeln("n is less than 10 or is negative")
else if (n >= 10 and n < 20)
    Writeln("n is greater than or equal 10 and less than 20")
else
    Writeln("n is greater than or equal to 20)
```
#### With blocks
```
if (expression1)
begin
    statement1
    ...
    statementN
end
else if (expression2)
begin
    statement1
    ...
    statementN
end
else
begin
    statement1
    ...
    statementN
end
```
### Return
```
return expression

e.g. return 1 + n * Random(1,2)
```
### While
#### Without block
```
while (expression)
    statement1

e.g.
var number := 0
while (number < 100)
    number:= ToNumber(Readln())
```

#### With block
```
while (expression)
begin
    statement1
    ...
    statementN
end

e.g.
var i := 0
while (i < 100)
begin
    i := i + 1
    Writeln(ToString(i))
end
```

## Operators
### Precedence
* or
* and
* <,>,<=, >=, ==, !=
* +,-
* *,/
* Unary: -,!

## Expressions
### Value
#### Number
```
e.g. 42
```
#### String
```
e.g. "lorem ipsum"
```
### Unary
#### Minus
```
e.g. -100
```
#### Negation
```
e.g. !(n < 10)
```
### Binary
```
expression operator expression

e.g.
2 + 3
2 * 3
```
### Array
```
identifier[expression]

e.g.
foo[2 * n]
```
### Function
```
identifier(argument1, ..., argumentN)

e.g. ToNumber("10")
```
### Parantheses
```
e.g.
10 * (4/4 + (2 - 1))
```
### String
Strings support addition
```
e.g. "Hello " + "World"
```

### Boolean
Conditional expressions evaluate to 0 or 1
```
e.g. 
(1 >= 2) == 0
(1 < 2) == 1
!(1 < 2) == 0
1 < 2 and 2 < 3 == 1
2 < 1 or 2 < 3 == 1
```

## Keywords
* var
* global
* begin
* end
* if
* else
* while
* func
* return

## Built-in functions
* ToNumber(str)
* ToString(num)
* Delay(ms)
* Random(min, max)

### Text IO
* Writeln(str)
* Readln()

### Graphics IO
Color and font properties support HTML5 canvas values
* ClearScreen()
* DrawRectangle(x, y, width, height,color)
* DrawText(text, x, y, color, font)
* Events (Built in)
* OnKeyDown(key)
* OnKeyUp(key)