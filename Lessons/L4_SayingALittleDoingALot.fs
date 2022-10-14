module sayingALittleDoingALot

open System

//In F#, the overall emphasis is to enable you to solve complex problems with simple code. You want to focus on solving the problem at hand without necessarily having to first think about design patterns within which you can put your code, or complex syntax. All the features you're going to see now are geared toward helping to achieve that. F# does have some common 'design-patternish' features, but in my experience, they're few and far between, with less emphasis on them.

//The `let` keyword is the single most important keyword in the F# language. You use it to bind values to symbols. In the context of F#, a value can range from a simple value type such as an integer or a Plan Old C# Object (POCO), to a complex value such as an object with fields and methods or even a function. In C#, we're not generally used to treating functions as value, but in F#, they're the same - so any value can be bound to a symbol with 'let'.

//Sample let bindings:
let age = 35 //binding 25 to the symbol age
let website = System.Uri "http://fsharp.org" //binding a URI to the symbol website
let add (first, second) = first + second //binding a function that adds two numbers together to the symbol add

//Takeaways from this sample:
//No types - you'll notice that we haven't bothered with specifying any types. The F# compiler will figure these out for you, so if you mouse over age or website, you'll see int and System.Uri (although you can - and occasionally must - specify them). This type inference is scattered throughout the language, and is so fundamental to how we work in F# that lesson 5 is entirely dedicated to it (and will explain how the compiler understands that the add function takes in two numbers - it’s not magic!)
//No 'new' keyword - in F#, the new keyword is optional, and generally not used except when constructing objects that implement IDisposable. Instead, F# views a constructor as a function, like any other 'normal' function that you might define
//No semicolons - in F#, they're optional; the newline is enough for the compiler to figure out you've finished an expression. You can use semicolons, but they're completely unnecessary (unless you want to include multiple expressions on a single line). Generally, you can forget they ever existed
//No brackets for function arguments - you might have already seen this and asked why this is. F# has two ways to define function arguments, known as tupled form and curried form. We'll deal with this distinction in a later lesson, but for now it's fine to say that both when calling and defining them, functions that take a single argument don't need round brackets, although you can put them in if you like; functions that take in zero or multiple arguments (as per the add function) need them, as well as commas to separate the arguments, just like C#

//Let isn't var!
//Don’t confuse let with var. Unlike var, which declares variables that can be modified later, let binds an immutable value to a symbol. The closest thing in C# would be to declare every variable with the readonly keyword (although this isn’t entirely equivalent). It’s better to think of let bindings as copy-and-paste directives; wherever you see the symbol, replace it with the value that was originally assigned during the declaration. You may have noticed that you can execute the same let binding multiple times in FSI. This is because F# allows you to repurpose a symbol multiple times within the same scope. This is known as shadowing, and is shown in the following listing:
let foo() =
    let x = 10 //binds 10 to the symbol x
    printfn "%d" (x + 20) //prints 30 to the console
    let x = "test" //binds 'test' to the symbol x. The original is now out of scope
    let x = 50.0 //binds 50 to the symbol x. The previous x is now out of scope
    x + 200.0 //returns 250.0

//Shadowing is a more advanced (and somewhat controversial) feature, so don't worry too much abnout it. But this is why you can declare the same symbol multiple times wthin FSI.

//Scoping values
//I’m sure you’ve heard that global variables are a bad thing! Scoping of values is important in any language; scoping allows us not only to show intent by explaining where and when a value is of use within a program, but also to protect us from bugs by reducing the possibilities for a value to be used within an application. In C#, we use { } to explicitly mark scope, as shown in the next listing:
//using System
//public static int DoStuffWithTwoNumbers(int first, int second)
//{
// var added = first + second;
// Console.WriteLine(“{0} + {1} = {2}”, first, second, added);
// var doubled = added * 2;
// return doubled;
//}

//In this context, the variable added is only in scope within the context of the curly braces. Outside of that, it's out of scope and not accessible by the rest of the program. On the other hand, F# is a whitespace-significant language: rather than using curly braces, you have to indent code to tell the compiler that you’re in a nested scope.

//Scoping in F#
let doStuffWithTwoNumbers(first, second) =
    let added = first + second //creation of scope for the doStuffWithTwoNumbers function
    Console.WriteLine("{0} + {1} = {2}", first, second, added)
    let doubled = added * 2
    doubled //return value of the function

//There's no specific restriction on the number of spaces to indent. You can indent 1 space or 10 spaces - as long as you're consistent within the scope! Most people use four spaces. It's not worth wasting time on picking the indent size, so I advise you to go with that to start with.

//You'll notice a few more things from this multiline function:
//No return keyword - the return keyword is unnecessary and not valid F# syntax (except in one case, which you’ll see that in the second half of this book). Instead, F# assumes that the final expression of a scope is the result of that scope. In this case it's the value of doubled
//No accessibility modifier - in F#, public is the default for top-level values. There are several reasons for this, but it makes perfect sense in F#, because with nested scopes (described in detail in the following section), you can hide values effectively without resorting to accessibility modifiers
//No static modifier - again, static is the default way of working in F#. This is different from what you’re used to, but it fits with how you’ll design most solutions in F#

//BOOKMARK page 53 Accessibility modifiers in F#