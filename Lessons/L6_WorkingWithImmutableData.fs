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
