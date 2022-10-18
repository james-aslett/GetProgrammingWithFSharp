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

//Accessibility modifiers in F#
//It's worth pointing out that although F# supports most modifiers, there's no protected access modifier. This isn't usually a problem; I've certainly never needed 'protected' since I started using F#. This is probably because protected is a modifier used when working with object-oriented hierarchies - something you rarely use in F#.

//Nested scopes
//We're used to using classes and methods as a means of scoping and data hiding. You might have a class that contains private fields and methods, as well as one or many public methods. You can also use methods for data hiding - again, the data is visible only within the context of that function call. In F#, you can define arbitrary scopes at any point you want. Let's assume you want to estimate someone's age by using the current year, as shown in the following listing:
let year = DateTime.Now.Year
let theAge = year - 1979
let estimatedAge = sprintf "You are about %d years old!" theAge
// rest of application…

//Looking at this code, the only thing you're interested in is the string value estimatedAge. The other lines are used as part of the calculation of that; they're not used anywhere else in your application. But currently, they're at the top level of the code, so anything afterward that uses estimatedAge can also see those two values. Why is this a problem? First, because it's something more for you as a developer to reason about - where is the year value being used? Is any other code somehow depending on it? Second (and again, this is slightly less of an issue in F#, where values are immutable by default), values that have large scopes tend to negatively impact a code base in terms of bugs and/or code smells. In F#, you can eliminate this by nesting those values inside the scope of estimatedAge as far as possible, as the next listing shows.

let estimatedAgeNested =                       //top-level scope
    let age =                                  //nested scope
        let year = DateTime.Now.Year           //value of year only visible within scope of age value
        year - 1979
    sprintf "You are about %d years old!" age  //can't access year value

//Now it's clear that age is used only by the estimatedAge value. Similarly, DateTime.Now.Year is used only when calculating age. You can't access any value outside the scope that it was defined in, so you can think of each of these nested scopes as being mini classes if you like - scopes for storing data that's used to generate a value.

//Nested functions
//If you've been paying attention, you'll remember that F# treats functions as values. This means that you can also create functions within other functions! Here’s an example of how to do this in F#:

let estimateAges(familyName, year1, year2, year3) = //top-level function
    let calculateAge yearOfBirth =                  //nested function
        let year = System.DateTime.Now.Year
        year - yearOfBirth
    let estimatedAge1 = calculateAge year1          //calling the nested functon
    let estimatedAge2 = calculateAge year2
    let estimatedAge3 = calculateAge year3
    let averageAge = (estimatedAge1 + estimatedAge2 + estimatedAge3) / 3
    sprintf "Average age for family %s is %d" familyName averageAge

//You declare a function called estimateAges, which itself defines a nested helper function called calculateAge inside it. The estimateAges function then calls calculateAge three times in order to generate an average age estimate for the three ages that were supplied. The ability to create nested functions means that you can start to think of functions and classes that have a single public method as interchangeable.

//Capturing values in F#
//Within the body of a nested function (or any nested value), code can access any values defined in its containing (parent) scope without you having to explicitly supply them as arguments to the nested function. You can think of this as similar to a lambda function in C# 'capturing' a value declared in its parents' scope. When you return such a code block, this is known as a closure; it's common to do this in F# - without even realizing it.

//Cyclical dependencies in F#
//This is one of the best 'prescriptive' features of F# that many developers coming from C# and VB are shocked by: F# doesn’t (easily) permit cyclical dependencies. In F#, the order in which types are defined matters. Type A can’t reference Type B if Type A is declared before Type B, and the same applies to values. Even more surprising is that this applies to all the files in a project - so file order in a project matters! Files at the bottom of the project can access types and values defined above them, but not the other way around. You can manually move files up and down in VS by selecting the file and pressing Alt-up arrow or Alt-down arrow (or right-clicking a file and choosing the appropriate option). As it turns out, though, this 'restriction' turns into a feature. By forcing you to avoid cyclic dependencies, the design of your solutions will naturally become easier to reason about, because all dependencies will always face 'upward'.

