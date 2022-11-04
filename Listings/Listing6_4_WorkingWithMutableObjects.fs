//Start by creating a good old Windows Form, displaying it, and then setting a few properties of the window.
open System.Windows.Forms
let form = new Form() //creating the form object
form.Show()
form.Width <- 400 //mutating the form by using the <- operator
form.Height <- 400
form.Text <- "Hello from F#!"