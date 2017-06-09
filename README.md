# TheLang2

TheLang is a type inferred language. What this means is, that you write code in a statically type language, but you never have to write a type.

## Language Spec
### Declarations
Just like most staticly typed languages, you have to declare variables before you can use them:

```C
// Declare a variable 'i' of infered type 'int'
i: 1
```

We don't have to define the type of `i`, as the type is infered by the compiler. `i` can now be used like a variable in other languages.

```C
// Assigning 'i' to 2.
i = 2

// We can't assign other types to 'i'
i = 2.0 // Error, trying to assign a float to an int

// We also can't re declare 'i'
i: 2.0 // Error, 'i' has already been declared
```

### Types
Even though types are never specified in the program, types still exists, and are used for compile time error checking.

#### Basic Types
| Literal  | Type   |
|----------|--------|
| 100      | int    |
| 100.0    | float  |
| true     | bool   |
| "String" | string |

#### Struct
Not only do we have basic types, but we also have composit types.
```C
// Declare a variable 'vec' of type 'struct(x: int, y: int)' 
// with the value of 'x' and 'y' being 0
vec: struct(x: 0, y: 0)
```

For structs, we have to be clear, which structs can be assigned to eachother.
```C
vec1: struct(x: 0, y: 0)
vec2: struct(x: 1, y: 1)
vec3: struct(x: 0.0, y: 0.0)
vec4: struct(y: 0, x: 0)

vec1 = vec2 // Ok, struct(x: int, y: int) can be assigned to itself
vec1 = vec3 // Error: struct(x: int, y: int) cannot be assigned to struct(x: float, y: float)
vec1 = vec4 // Error: struct(x: int, y: int) cannot be assigned to struct(y: int, x: int)
```

#### Array
We also have arrays.
```C
// Declare a variable 'arr1' of type '[]int', where all elements are 0
arr1: [4]{ 0 }

// Declare a variable 'arr2' of type '[]int', where elements are assigned to 0, 1, 2, 3
arr2: [4]{ 0, 1, 2, 3 }

// Declare a variable 'arr3' of type '[]int', which is empty (We need { 0 } to infer the type)
arr3: [0]{ 0 }
```

### Control Flow
Without control flow, programs can't do much.

#### If
TheLang have `if` statements, like most languages
```C
a: 1
b: 2

if a == b {
    // Do stuff
} else if a < b {
    // Do other stuff
} else {
    // Do third stuff
}
```

`if` statements are however more poverfull in this language, as certain features have all been bundled into the 'if' statement.
```C
i: 5

// An 'if' statement with 'switch' like semantics
if i {
    case 0 {
    }
    case 1 {
    }
    case {
        // Default case
    }
} else {
    // If no cases match, the else block is executed
}
```

`if` satements also have a feature that allows them to handle multible return types from functions.
```C
// In languages with multiple return values, code would need to be written like this:
success, result: parse("100")

if success {
    // Do something with result
} else {
    // Result can still be accessed, but we most likely don't want this
}

// In TheLang, 'if' statements can specify a 'payload', which are declarations to 
// store the rest of the return values.
if parse("100") ; result {
    // Do something with result
} else {
    // We can't access result here
}
```

The `switch` like `if` statement, and the payload feature also work together.
```C
if parse_program() ; tree {
    // TODO: Enums have not been thought of yet
    case "Error: Syntax" { }
    case "Error: EOF" { }
    case "Success" { }
    case {
        // Handle unknown error
    }
} else {
    // Note here, that result does not exist in the else but does
    // exist in the default case above.
    // This is the reason there are two ways of having a default case.
}
```

#### Loop
Next, we have loops.
```

```

#### Procedure