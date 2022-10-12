module repl

//REPL (Read Evaluate Print Loop) - a mechanism for you to enter arbitrary, ad hoc code into a standalone environment and get immediate feedback. The easiest way to think of a REPL is to think of the Immediate window in Visual Studio, but rather than being used when running an application, a REPL is used when developing an application. You send code to the REPL, and it evaluates the code and then prints the result. This feedback loop is a tremendously productive away to develop.

//A REPL can be an affective replacement for all three of the use cases outlined previously. Here are some example uses:
//Writing new business logic that you want to add to your application
//Testing existing production code with a predefined set of data
//Exploring a new NuGet package that you've heard about in a lightweight exploratory mode
//Performing rapid data analysis and exploration - think of tools such as SQL Server Management Studio

//The emphasis is on quick exploration cycles: trying out ideas in a low-cost manner and getting rapid feedback, before pushing that into the application code base. What's nice about F# is that because the language allows you to encode more business rules into the code than in C#, you can often have a great deal of confidence that your code will work. You won’t need to run end-to-ends particularly often. Instead, you’ll find yourself working in Visual Studio and the REPL more and more, focusing on writing code that delivers business value. C# has a basic REPL called C# Interactive.

//F# Interactive (FSI)

//input: printfn "Hello world!";;
//output: Hello world! val it : unit = ()

//input: System.DateTime.UtcNow.ToString();;
//output: val it: string = "12/10/2022 12:18:26"

//You're able to execute arbitrary F# code without needing to run an application; Visual Studio itself is a host to your code. FSI outputs anything that you print out (such as text 'Hello world!') as well as information itself (such as the content of values when they're evaluated). You'll see more in the coming lessons.

//State in FSI
//FSI maintains state across each command, and you can bind the value of a particular expression to a value by using the let keyword that you can then access in subsequent calls:
//input: let currentTime = System.DateTime.UtcNow;;
//input: currentTime.TimeOfDay.ToString();;

//Note the two semicolons(;;) at the end of each command. This tells FSI to execute the text currently in the buffer. Without this, FSI will add that command to the buffer until it encounters ;; and will execute all the commands that are in the buffer.

//If you want to reset all state in FSI, you can either right-click and choose Reset Interactice Session, or press Ctrl-Alt-R. Similarly, you can clear the output of FSI (but retai its state) by using Clear All or Ctrl-Alt-C.

//You probably noticed the 'val it = text' in FSI for commands that you executed. What's this? 'it' is the default value that any expressions are bound to if you don't explicitly supply one by using the 'let' keyword. Executing this command: System.DateTime.UtcNow;; is the same as executing this: let it = System.DateTime.UtcNow;;
