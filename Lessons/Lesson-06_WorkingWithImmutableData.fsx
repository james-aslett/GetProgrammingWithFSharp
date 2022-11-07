//WORKING WITH IMMUTABLE DATA
//Working with immutable data is one of the more difficult aspects of functional programming to deal with, but as it turns out, after you get over the initial hurdle, you'll be surprised just how easy it is to write entire applications working with purely immutable data structures. It also goes hand in hand with many other F# features you'll see, such as expression-based development. In this lesson, you'll learn:

//- The basic syntax for working with immutable and mutable data in F#
//- Some reasons you should consider immutability by default in software development today
//- Simple examples of working with immutable values to manage changing state

//Working with mutable data - a recap
//Let's start by thinking about some of the issues we come up against but often take for granted as simply 'the way things are.' Here are a few examples that I've either seen firsthand or fallen foul of myself.

//The unrepeatable bug
//Say you're developing an application, and one of the test team members comes up to you with a bug report. You walk over to that person's desk and see the problem happening. Luckily, your tester is running in Visual Studio, so you can see the stack trace and so on. You look through the locals and application state, and figure out why the bug is showing up. Unfortunately, you have no idea how the application got into this state in the first place; it's the result of calling a number of methods repeatedly over time with some shared mutable state stored in the middle.

//You go back to your machine and try to get the same error, but this time you can't reproduce it. You file a bug in your work-item tracking system and wait to see if you can get lucky and figure out how the application got into this state. 

//Multithreading pitfalls
//How about this one? You're developing an application and have decided to use multithreading because it's cool. You recently heard about the Task Parallel Library in .NET, which makes writing multithreaded code a lot easier, and also saw that there's a Parallel.ForEach() method in the BCL. Great! You've also read about locking, so you carefully put locks around the bits of the shared state of your application that are affected by the multithreaded code. You test it locally and even write some unit tests. Everything is green! You release, and two weeks later find a bug that you eventually trace to your multithreaded code. You don't know why it happened, though; it's caused by a race condition that occurs only under a specific load and a certain ordering of messages. Eventually, you revert your code to a single-threaded model.

//Accidentally sharing state
//Here's another one. You're working on a team and have designed a business object class. Your colleague has written code to operate on that object. You call that code, supplying an object, and then carry on. Sometime later, you notice a bug in your application: the state of the business object no longer looks as it did previously! It turns out that the code your colleague wrote modified a property on the object without you realizing it. You made that property public only so that you could change it; you didn't intend or expect other bits of code to change the state of it! You fix the problem by making an interface for the type that exposes the bits that are 'really' public on the type, and give that to consumers instead.

//Testing hidden state
//Or maybe you're writing unit tests. You want to test a specific method on your class, but unfortunately, to run a specific branch of that method, you first need to get the object into a specific state. This involves mocking a bunch of dependencies that are needed to run the other methods; only then can you run your method. Then, you try to assert whether the method worked, but the only way to prove that the method worked properly is to access a shared state that's private to the class. Your deadlines are fast approaching, so you change the accessibility of the private field to be Internal, and make internals visible to your test project.

//Summary of mutability issues
//All of these problems are real issues that occur on a regular basis, and they're nearly always due to mutability. The problem is often that we simply assume that mutability is a way of life, something we can't escape, and so look for other ways around these sorts of issues - things like encapsulation, hacks such as InternalsVisibleTo, or one of the many design patterns out there. It turns out that working with immutable data solves many of these problems in one fell swoop.

//Being explicit about mutation
//So far, you've only looked at simple values in F#, but even these show that by default, values are immutable. As you'll see in later lessons, this also applies to your own custom F# types (for example, Records).

//Mutability basics in F#

//Listing 6.1 Creating immutable values in F#
let name = "isaac" //creating an immutable value
name = "kate" //trying to assign 'kate' to name

//You'll notice when you execute this code, you receive the following output in FSI:
//val name : string = "isaac"
//val it : bool = false

//The false doesn't mean that the assignment has somehow failed. It occurs because in F#, the = operator represents equality, as == does in C#. All you've done is compare 'isaac' with 'kate', which is obviously false. How do you update or mutate a value? You use the assignment operator, <-. Unfortunately, trying to insert that into your code leads to an error, as shown next.

//Listing 6.2 Trying to mutate an immutable value
name <- "kate" //error FS0027: This value is not mutable

//Oops! This still doesn't work. It turns out that you need to take one final step to make a value mutable, which is to use the mutable keyword.

//Listing 6.3 Creating a mutable variable
let mutable name = "isaac" //defining a mutable variable
name <- "kate" //assigning a new value to the variable

//You'll notice that the name value is now automatically highlighted in red as a warning that this is a mutable value. You can think of this as the inverse of C# and VB .NET, whereby you use variables by default, and explicitly mark individual items as immutable values by using the readonly keyword. The reason that F# makes this decision is to help guide you down what I refer to as the pit of success; you can use mutation when needed, but you should be explicit about it and should do so in a carefully controlled manner. By default you should go down the route of adopting immutable values and data structures. As it turns out, you can easily develop entire applications (and I have, with web front ends, SQL databases, and so forth) by using only immutable data structures. You'll be surprised when you realize how little you need mutable data, particularly in request/response-style applications such as web applications, which are inherently stateless.

//Listing 6.4 Working with mutable objects
//Before we move on to working with immutable data, here's a quick primer on the syntax for working with mutable objects. I don't recommend you create your own mutable types, but working with the BCL is a fact of life as a .NET developer, and the BCL is inherently OO-based and filled with mutable structures, so it's good to know how to interact with them.

open System.Windows.Forms
let form = new Form() //creating the form object
form.Show()
form.Width <- 400 //mutating the form by using the <- operator
form.Height <- 400
form.Text <- "Hello from F#!"

//Mutable bindings and objects
//Most objects in the BCL, such as a Form, are inherently mutable. Notice that the form symbol is immutable, so the binding symbol itself can't be changed. But the object it refers to is itself mutable, so properties on that object can be changed!

//Notice that you can see the mutation of the form happen through the REPL. If you execute the first three lines, you start with an empty form, but after executing the final line, the title bar will immediately change.

//F# also has a shortcut for creating mutable data structures in a way that assigns all properties in a single action. This shortcut is somewhat similar to object initializers in C#, except that in F# it works by making properties appear as optional constructor arguments.

//Listing 6.5 Shorthand for creating mutable objects
open System.Windows.Forms
let form = new Form(Text = "Hello from F#!", Width = 300, Height = 300) //Creating and mutating properties of a form in one expression
form.Show()

//If actual constructor arguments are required as well, you can put them in there at the same time.

//Modeling state
//Let's now look at the work needed to model data with state without resorting to mutation.

//Working with mutable data
//Working with mutable data structures in the OO world follows a simple model: you create an object, and then modify its state through operations on that object. What's tricky about this model of working is that it can be hard to reason about your code. Calling a method such as UpdateState() will generally have no return value; the result of calling the method is a side effect that takes place on the object.

//Let's put this into practice with a simple example: driving a car. You want to be able to write code that allows you to drive() a car, tracking the amount of petrol (gas) used. Depending on the distance you drive, you should use up a different amount of petrol.

//Listing 6.6 Managing state with mutable variables
