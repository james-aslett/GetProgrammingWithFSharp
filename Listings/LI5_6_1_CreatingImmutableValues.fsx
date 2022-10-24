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

//BOOKMARK p73
