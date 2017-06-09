// TheLang is a type inferred language. What this means is, that you write code in a 
// statically type language, but you never have to write a type.
// How does this work? Can it even be useful?
// Well, let's take a look

// First we define 'main'
main :: () => {
    // We will look further into function declarations later. For now, we will focus on statements

    // First, let's go over variable declaration.
    i: 1 // Declare a variable 'i' with the value '1'

    // Note here, that we don't define the type. 'i' is inferred to be an integer by the compiler.
    // We can now also assign to this variable now that it is declared.
    i = 2

    // We should here note, that "i = 2.0" will give a compiler error, as 'i' has already been declared as an integer.

    // Now, let's go over all literals in the language.
    i: 100      // Integer literal (Type: i64)
    f: 100.0    // Float literal   (Type: f64)
    b: true     // Boolean literal (Type: bool)
    s: "String" // String literal  (Type: string)

    // We don't have different sizes, such as i32 or f32 (yet, if ever)

    // We can't do much with these types, however. We need collections of values as well.
    // Let's start with arrays
    a: [4]{ 0 } // Declaring a variable 'a' which is an integer array

    // The syntax for array literals goes as followed: '[' <Expr> ']' '{' <Expr> (, <Expr>)* '}'
    // Inside "[]" we define the size of the array (in this case 4).
    // Inside "{}" we define it's values.
    // If there is only 1 value specified, this value will fill the whole array (in this case, the array will contain 4 0's).

    // We can't declare "a: [4]{ 1, 1.0, true, "String" }", because these are not the same types.

    // Arrays in this language are defined as a struct with a 'length' and a 'data' pointer. The 'length' of the array, can be accessed
    // through the array, but the pointer cannot (yet)
    length: a.length
    // data: a.data // Not yet supported

    // We can also have composit types.
    v: struct(x: 0, y: 0.0, z: true) // Declaring a variable 'v' which is a struct with three fields: x (int), y (float) and z (boolean)

    // Behind the sceen, the compiler will keep track of your structs and ensure that all structs with the same fields are assignable.
    // Let's go over, how assignability for structs are defined:
    q: struct(a: 0, b: 0)
    q = struct(a: 2, b: 2)      // All fields are the same, so we are ok
    // q = struct(a: 2.0, b: 4) // Error, q has defined 'a' to be an int, but the new struct has 'a' as a float
    // q = struct(b: 8, a: 4)   // Error, names and order matters in structs "struct(a, b)" cannot be assigned to "struct(b, a)"

    // For structs, we can access all its fields through the '.' operator
    qa: q.a
    qb: q.b
    
    // These are all our literals. Let's now look at all operators in the language:
    // Addition
    t_int  : 1 + 1     // Works on integers and floats by default
    t_float: 1.0 + 1.0

    // Subtraction
    t_int   = 1 - 1     // Works on integers and floats by default
    t_float = 1.0 - 1.0

    // Multiplication
    t_int   = 1 * 1     // Works on integers and floats by default
    t_float = 1.0 * 1.0

    // Division
    t_int   = 1 / 1     // Works on integers and floats by default
    t_float = 1.0 / 1.0

    // Modulo
    t_int   = 1 % 1     // Works on integers and floats by default
    t_float = 1.0 % 1.0

    // Less than
    t_bool:  1 < 1      // Works on integers and floats by default
    t_bool = 1.0 < 1.0

    // Less than and equals
    t_bool = 1 <= 1      // Works on integers and floats by default
    t_bool = 1.0 <= 1.0

    // Greater than
    t_bool = 1 > 1      // Works on integers and floats by default
    t_bool = 1.0 > 1.0

    // Greater than and equals
    t_bool = 1 >= 1      // Works on integers and floats by default
    t_bool = 1.0 >= 1.0

    // Boolean equals
    t_bool = 1 == 1      // Works on integers, floats and strings by default
    t_bool = 1.0 == 1.0
    t_bool = "Hello" == "Hello"

    // Boolean and
    t_bool = true && true // Works on booleans

    // Boolean or
    t_bool = true || true // Works on booleans

    // Dot 
    st: struct(a: 0, b: 1, c: 2.0)
    arr: [2]{ 0 }

    st_a: st.a             // Works on structs, and arrays, if the right side is 'length'
    arr_length: arr.length

    // TODO: Do we want pointers or maybe a higher level construct?
    // Reference
    value: 100
    pointer: @value // Works on all types

    // Dereference
    value = *pointer // Works on pointers

    // Indexing
    value = arr[0] // Works on arrays


    // Now, let's look into control flow.
    // We will start with the 'if' statement
    if true {
        // Do something
    }

    // The basic if statement if fairly simple, but it has a few extra specialisations that make it
    // more useful in certain scenarios.
    // Here, the if statement is used like a switch.
    some_int: 0
    
    if some_int {
        case 0 {
            // Do one thing
        }
        case 1 {
            // Do something else
        }
        case {
            // Default case
        }
    } else {
        // If no case matched
        // Can be used as a default case
    }

    // The reason this works is, that for booleans this:
    if true {
        // Do stuff
    }

    // is syntatic sugar for
    if true {
        case true {
            // Do stuff
        }
    }


    // Now, we haven't talked about procedures yet, but procedures can return multiple values if they want.
    // This can be used for error handling, but when done like this, if statements cannot be used to
    // directly check the bool:
    success, result: parse("100b")
    
    if success {
        // Use result
    }

    // In TheLang, however, we have a feature to aid with this
    if parse("100b") ; result {
        // use result
    }

    // Since we don't use ';' for statements, we decided to use it for this instead. 
    // How this works is, that the 'if' statement will use the first returned value in the 'if',
    // and the rest are stored in the variables after ';'.
    // Note, that in this example, 'result' only exists inside the scope of the 'if' block.
    if parse("100b") ; result {
        i: result + 2
    } else {
        i: result + 1 // Error, result is not defined in this scope
    }

    // This feature also works when working with the 'switch' version of if
    // TODO: We don't really have any kind of enum. Since an enum is kind of a type, it goes against
    //       the goal of this language, to be able to specify them

    if do_some_stuff() ; result {
        case "Error: Program died" {
            // Save program
        }
        case "Error: Pointer was null" {
            // Initialize pointer
        }
        case "Success" {
            // Work with result
        }
        case {
            // Handle unknown error
        }
    } else {
        // Note here, that result does not exist in the else but does
        // exist in the default case above.
        // This is the reason there are two ways of having a default case.
    }


    // Next, we have our 'loop'
    loop {
        // Endless loop
    }

    // Loop with condition
    val1: 0
    val2: 6
    loop val1 != val2 {
        val1 += 1
    }

    // Just like 'if', we can use the ';' to extract results from multi return functions
    val3: 0
    loop try_something(val3) ; result {
        val3 += result
    }

    // We can also loop over a range
    sum: 0
    arr: [5]{ 2 }
    loop 2 .. arr.length { // This is an exclusive range. From 2 to and with arr.length - 1 (2, 3, 4)
        // 'item' is the default name for the value at the current iteration
        // 'item_index' is the default name for the index of the item (in this loop, item_index is (0, 1, 2))
        sum += arr[item] + item_index 
    }

    loop 2 .. arr.length ; i { // We can give 'item' and explicit name
        sum += arr[i] + item_index 
    }

    loop 2 .. arr.length ; i, i_index { // We can also give 'item_index' an explicit name
        sum += arr[i] + i_index 
    }

    // loop 2 .. arr.length ; i_index { // We can't only give 'item_index' an explicit name
    //     sum += arr[item] + i_index 
    // }

    // We should also here note, that 'loop val1 .. val2' always increments val1 by 1, as long as va1 < val2.
    // This means, that 'loop 5 .. 4' will iterate until overflowing and then becomes 4

    // loops can also iterate over collections that have a few procedures defined on them
    loop arr {
        sum += item + item_index
    }

    // This is syntatic sugar for
    iterator: get_iterator(arr)
    loop get_next(iterator) ; item, item_index {
        sum += item + item_index
    }

    // As long as 'get_iterator' and 'get_next' are defined and work on our type, we can iterate over that collection

    // Finally, let's have a look at procedures, the one thing that could complicate our type system quite a bit.
    // First, let's define a simple procedure, and see how it works.

    doing_stuff :: (a, b, c) => {
        d: a + b

        if d < c {
            return d, a
        }
        
        return a, b
    }

    // This is a small procedure that will give a lot of info into how procedures work. 
    // First, we can see that 'doing_stuff' takes 3 arguments (a, b and c).
    // We do not know, at definition site, if this procedure is valid (we can check for missing declarations). 
    // The compiler does however set up some requirements that the arguments must forfill for this procedure
    // to be called with them.
    // 'doing_stuff' would define requirements like:
    //    * require '+'(a, b) -> ?
    //    * require type of 'd'
    //    * require '<'(d, c) -> bool

    // With requirements set up, the compiler only have to go through the body of the procedure once. After this,
    // the compiler will just ensure that the requirements are met for call
    
    // So if we call 'doing_stuff':
    doing_stuff(1, 2, 3)

    // The compiler now knows all types:
    // a: int, b: int, c: int
    // It can therefore check if the procedures and operators exists
    //    * +(int, int) -> int exists
    //    * 'd' is int
    //    * <(int, int) -> bool exists

    // The compiler will cache these results, so the next 'doing_stuff(int, int, int)' call is checked instantly.

    // This language also have procedure overloading, but it only works in cases where only one procedure success in 
    // forfilling its requirements for a set of arguments.
    // If we define a new 'doing_stuff':
    doing_stuff :: (x, y, z) => { // We can overload with different names, no problem
        if x < y && y < z {
            return x, y
        }

        return y, x
    }
    
    // These two procedures will collide when called with integer arguments. You will therefore get a compiler error, when
    // it is ambiguous what procedure to call.

    // There are, however, some procedures that you might thing are ambiguous, but that the compiler can handle, following a
    // certain rule set.
    add :: (a, b) => { }
    add :: (a, b) => a + b
    add :: (a, b) => a + b, b + a

    a, b: add(1, 1) // third add
    c   : add(1, 1) // second add
          add(1, 1) // first add

    // Here, the compiler will pick the correct procedure based on the return count, as it is the only way it was able
    // to tell the procedures apart.
    // One should be careful of this though, as if the first add is deleted, the second add will be called in both 
    // cases, since return values can be ignored.

    // We can also overload operators
    + :: (v1, v2) => {
        v1.x += v2.x
        v1.y += v2.y

        return v1
    }

    res: struct(x: 1, y: 1) + struct(x: 2, y: 2) // It just works, as long as 'v1' and 'v2' both specify an 'x' and 'y' field
}