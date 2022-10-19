module trustTheCompiler

//Type inference as we know it
//Let's refamiliarize ourselves with the C# var keyword based on the official MSDN documentation:
//Variables that are declared at method scope can have an implicit type var. An implicitly typed local variable is strongly typed just as if you had declared the type yourself, but the compiler determines the type.

//Type inference in C# in detail
//Here's a simple example of that description in action.
var i = 10; //implicitly typed
int i = 10; //explicitly typed

//The right-hand side of the = can be just about any expression, so you can write more complicated code that may defer to another method, perhaps in another class. Naturally, you need to give a little bit of thought regarding the naming of variables when using var!

//Variable naming with type inference
var i = customer.Withdraw(50); //Implicitly typed. Withdraw() returns an int, so i is inferred to be an int.
var newBalance = customer.Withdraw(50); //Use of intelligent naming to explain intent to the reader

//The multipurpose var
//There's another reason for the use of var in C#: to store references to types that have no formal declaration, a.k.a. anonymous types. Anonymous types don’t exist in F#, although as you’ll see later, you rarely miss them, as good alternatives exist that are in many ways more powerful.

//It's important to stress that var mustn't be confused with the dynamic keyword in C#, which is (as the name suggests) all about dynamic typing. var allows you to use static typing without the need to explicitly specify the type by allowing the compiler to determine the type for you at compile time:
var name = "Isaac Abraham";
name.ToLower();
var number = 123;
number.ToLower(); // 'int' does not contain a definition for 'ToLower'

//Practical benefits of type inference
//Even in its restricted form in C#, type inference can be a nice feature. The most obvious benefit is that of readability: you can focus on getting results from method calls and use the human-readable name of the value to gain its meaning, rather than the type. This is especially useful with generics, and F# uses generics a lot. But you gain another subtler benefit: the ease of refactoring. Imagine you have a method Save() that stores data in the database and returns an integer value. Let’s assume that this is the number of rows saved. You then call it in your main code:
int result = Save(); //explicit binding to int
if (result == 0) //where the value is explicitly used as an int
 Console.WriteLine("Failure!");
else
 Console.WriteLine(“Worked!”);

 //Note that you're explicitly marking result as an integer, although the declaration of the variable could just as well have been var. Then at some point in the future, you decide that you want to return a Boolean that represents success/failure instead. You have to change two things:
 //1: You need to manually change the method signature to specify that the method returns bool rather than int. There’s no way around this in C#.
 //2 You need to go through every call site to Save() and manually fix it to bind the result to bool rather than int. If you had used var, this wouldn't have been a problem at all, because you'd have left the compiler to figure out the type of result.

 //Even when using var, at some point you'd normally have to make some kind of change to your code to handle a bool instead of an int - in this case, it's the conditional expression for the if statement. Although var won't fix everything for you - it's not magic - the difference is that the compiler would have taken care of fixing the 'boilerplate' error for you automatically, leaving you to change the 'real' logic (changing the expression from comparing with 0 to comparing with true).

 //Critics of type inference
 //Some developers shy away from type inference. A common complaint I hear is that it's 'magic', or alternatively that one can't determine the type of a variable at a glance. The first point can be easily dispelled by reading the rules for type inference: the compiler doesn't guess the types; a set of precedence rules guides the compiler. The second point can also be dispelled by the number of excellent IDEs (including VS2015) that give you mouse-over guidance for types, as well as following good practices such as sane variable naming (which is generally a good thing to do). Overall, particularly in F#, the benefits massively outweigh any costs.

 //Imagining a more powerful type-inference system
 //Unfortunately, type inference in C# and VB .NET is restricted to the single use case I've illustrated. Let’s look at a slightly larger code snippet:
 public static var CloseAccount(var balance, var customer)
{
    if (balance < 0) //balance compared with 0
    return false; //returning a boolean
    else
    {
        customer.Withdraw(customer.AccountBalance); //calling methods and accessing properties on a type
        customer.Close();
        return true; //returning a boolean
    }
}

//This is invalid C#, because I've omitted all types. But couldn't the compiler possibly work out the return type or input arguments based the following?
//- We're comparing balance with 0. Perhaps this is a good indicator that balance is also an integer (although it could also be a float or other numeric type)
//- We're returning Boolean values from all possible branches of the method. Perhaps we want the method to return Boolean?
//- We're accessing methods and properties on the customer object. How many types in the application have Withdraw and Close methods and an AccountBalance property (which is also compatible with the input argument of Withdraw)? 

//F# type-inference basics
//We've discussed some of the benefits of type inference in C#, as well as some of the issues and concerns about it. All of these are magnified with F#, because type inference in F# is pervasive. You can write entire applications without making a single type annotation (although this isn't always possible, nor always desirable). In F#, the compiler can infer the following:
//- Local bindings (as per C#)
//- Input arguments for both built-in and custom F# types
//- Return types

//F# uses a sophisticated algorithm that relies on the Hindley–Milner (HM) type system. It's not especially important to know what that is, although feel free to read up on it in your own time! What is important to know is that HM type systems do impose some restrictions in order to operate that might surprise you, as we'll see shortly. Without further ado, let's finally get onto some F#! Thus far, all the examples you've seen in F# haven't used type annotations, but now I'll show you a simple example that we can break down piece by piece so you can understand how it works.

//Explicit type annotations in F#
let add (a:int, b:int) : int =
    let answer:int = a + b
    answer

//We'll cover functions in more depth later, but to get us going here, a type signature in F# has three main parts:
//- The function name
//- All input argument type(s)
//- The return type

//You can see from the code sample that both input arguments a and b are of type int, and the function also returns an int.

//Type annotations in C# and F#
//Many C# developers recoil when they see types declared after the name of the value. In fact, most languages outside of C, C++, Java, and C# use this pattern. It’s particularly common in languages such as F#, where type inference is especially powerful, or optional type systems such as TypeScript.

//Start by removing just the return type from the type signature of the function; when you compile this function in FSI, you’ll see that the type signature is exactly the same as before.

//Omitting the return type from a function in F#
let add (a:int, b:int) =
    let answer:int = a + b
    answer

// val add : a:int * b:int -> int

//F# infers the return type of the function, based on the result of the final expression in the function. In this case, that’s answer. Now go one step further and remove the type annotation from b. Again, when you compile, the type signature will be the same. In this case, it raises an interesting question: how does the compiler know that b isn't a float or decimal? The answer is that in F#, implicit conversions aren’t allowed. This is another feature of the type system that helps enforce safety and correctness, although it's not something that you'll necessarily be used to. In my experience, it's not a problem at all. And given this restriction, the compiler can safely make the assumption that b is an int. Finally, remove the remaining two type annotations:
let add (a, b) =
    let answer:int = a + b
    answer

//Amazingly, the compiler still says that the types are all integers! How has it figured this out? In this case, it's because the + operator binds by default to integers, so all the values are inferred to be ints.

//Earlier, we demonstrated that type inference can not only improve readability by removing unnecessary keywords that can obscure the meaning of your code, but also speed up refactoring - for example, by allowing you to change the return type of a function without necessarily breaking the caller. This benefit is increased by a significant factor when working with F#, because you can automatically change the return type by simply changing the implementation of a function, without needing to manually update the function signature. Indeed, when coupled with its lightweight syntax and ability to create scopes by indenting code, F# enables you to create new functions and change type signatures of existing code incredibly easily - particularly because type inference in F# can escape local scope, unlike in C#.

//Limitations of type inference
//F# has a few more restrictions and limitations related to type inference. Let’s go through them one by one here.

//Working with the BCL
//First, type inference works best with types native to F#. By this, I mean basic types such as ints, or F# types that you define yourself. We haven't looked at F# types yet, so this won't mean much to you, but if you try to work with a type from a C# library (and this includes the .NET BCL), type inference won't work quite as well - although often a single annotation will suffice within a code base.

//Type inference when working with BCL types in F#
let getLength name = sprintf "Name is %d letters." name.Length //doesn't compile - type annotation is required
let getLength (name:string) = sprintf "Name is %d letters." name.Length //compiles
let foo(name) = "Hello! " + getLength(name) //compiles - 'name' argument is inferred to be string, based on the call to getLength()

//BOOKMARK p65 Listing 5.7

