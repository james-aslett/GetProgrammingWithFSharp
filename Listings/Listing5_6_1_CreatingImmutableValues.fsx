let name = "isaac" //creating an immutable value
name = "kate" //trying to assign 'kate' to name

//You'll notice when you execute this code, you receive the following output in FSI:
//val name : string = "isaac"
//val it : bool = false

//The false doesn't mean that the assignment has somehow failed. It occurs because in F#, the = operator represents equality, as == does in C#. All you've done is compare 'isaac' with 'kate', which is obviously false. How do you update or mutate a value? You use the assignment operator, <-. Unfortunately, trying to insert that into your code leads to an error, as shown next.

//Trying to mutate an immutable value
name <- "kate" //error FS0027: This value is not mutable

//Oops! This still doesn't work. It turns out that you need to take one final step to make a value mutable, which is to use the mutable keyword.

//Creating a mutable variable
let mutable name = "isaac" //defining a mutable variable
name <- "kate" //assigning a new value to the variable

//You'll notice that the name value is now automatically highlighted in red as a warning that this is a mutable value. You can think of this as the inverse of C# and VB .NET, whereby you use variables by default, and explicitly mark individual items as immutable values by using the readonly keyword. The reason that F# makes this decision is to help guide you down what I refer to as the pit of success; you can use mutation when needed, but you should be explicit about it and should do so in a carefully controlled manner. By default you should go down the route of adopting immutable values and data structures. As it turns out, you can easily develop entire applications (and I have, with web front ends, SQL databases, and so forth) by using only immutable data structures. You’ll be surprised when you realize how little you need mutable data, particularly in request/response-style applications such as web applications, which are inherently stateless.

//Working with mutable objects
//Before we move on to working with immutable data, here’s a quick primer on the syntax for working with mutable objects. I don’t recommend you create your own mutable types, but working with the BCL is a fact of life as a .NET developer, and the BCL is inherently OO-based and filled with mutable structures, so it’s good to know how to interact with them.

open System.Windows.Forms
let form = new Form() //creating the form object
form.Show()
form.Width <- 400 //mutating the form by using the <- operator
form.Height <- 400
form.Text <- "Hello from F#!"

//Mutable bindings and objects
//Most objects in the BCL, such as a Form, are inherently mutable. Notice that the form symbol is immutable, so the binding symbol itself can’t be changed. But the object it refers to is itself mutable, so properties on that object can be changed!

//Notice that you can see the mutation of the form happen through the REPL. If you execute the first three lines, you start with an empty form, but after executing the final line, the title bar will immediately change.

//F# also has a shortcut for creating mutable data structures in a way that assigns all properties in a single action. This shortcut is somewhat similar to object initializers in C#, except that in F# it works by making properties appear as optional constructor arguments.

//Shorthand for creating mutable objects
open System.Windows.Forms
let form = new Form(Text = "Hello from F#!", Width = 300, Height = 300) //Creating and mutating properties of a form in one expression
form.Show()

//If actual constructor arguments are required as well, you can put them in there at the same time.

//Modeling state
//Let’s now look at the work needed to model data with state without resorting to mutation.

//Working with mutable data
//Working with mutable data structures in the OO world follows a simple model: you create an object, and then modify its state through operations on that object. What’s tricky about this model of working is that it can be hard to reason about your code. Calling a method such as UpdateState() will generally have no return value; the result of calling the method is a side effect that takes place on the object.