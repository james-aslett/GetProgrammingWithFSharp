let text = "Hello, world"
text.Length

let greetPerson name age = sprintf "Hello, %s. You are %d years old" name age
let greeting = greetPerson "Fred" 25

let countWords (text:string) =
    text.Split [|' '|] //Split takes an array. Split each word at a space
    |> Seq.length //return amount of words
let myWords = countWords "Apple Pear Plum Peach"

//Spend a little more time in the F# script; write a function, countWords, that can return
//the number of words in a string by using standard .NET string split and array functionality. You'll need to provide a type hint for the input argument, such as
//let countWords (text:string) =...
//Then, save the string and number of words to disk as a plain-text file.