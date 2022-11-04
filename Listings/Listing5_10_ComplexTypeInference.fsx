//Copy the code from listing 5.10 into an F# script in VS; everything will compile by default. Now let's see how to break this code! Start by first changing the first if/then case in innerFunction to compare against a string ("hello") rather than an integer (10)

let sayHello(someValue) =
    let innerFunction(number) =
        if number > "hello" then "Isaac"
        elif number > 20 then "Fred"
        else "Sara"
    let resultOfInner =
        if someValue < 10.0 then innerFunction(5)
        else innerFunction(15)
    "Hello " + resultOfInner
let result = sayHello(10.5)

//Rather than this line showing an error, you'll see errors in three other places! Why? This is what I refer to as following the breadcrumbs: you need to track through at least one of the errors and see the inferred types to understand why this has happened. Let's look into the first error and try to work out why it's occurring: this expression was expected to have type string , but here it has type int. Remember to mouse over the values and functions to get IntelliSense of the type signatures being inferred!

//1 Looking at the compiler error message, you can see that the call site to innerFunction now expects a string, although you know that it should be an int.
//2 Now look at the function signature of innerFunction. It used to be int -> string, but now is string -> string (given a string, it returns a string).
//3 Look at the function body. You can see that the first branch of the if/then code compares number against a string rather than an int. The compiler has used this to infer that the function should take in a string.
//4 You can prove this by hovering over the number value, which sure enough is now inferred to be a string.
//5 To help you, and to guide the compiler, let's temporarily explicitly type-annotate the function:

let sayHello2(someValue) =
    let innerFunction(number:int) =
        if number > "hello" then "Isaac"
        elif number > 20 then "Fred"
        else "Sara"
    let resultOfInner =
        if someValue < 10.0 then innerFunction(5)
        else innerFunction(15)
    "Hello " + resultOfInner
let result = sayHello(10.5)

//6 You'll see that the 'false' compiler errors disappear, and the compiler now correctly identifies the error as being "hello", which should be an int.
//7 After you correct the error, you can remove the type annotation again.

//From this example, you can see that adding in type annotations can sometimes be useful, particularly when trying to narrow down an error caused by clashing types. I recommend, however, that you in general try to avoid working with explicit types everywhere. Your code will look much cleaner as a result.