
open System
open System.Net
open System.Windows.Forms

let navigatetoUrl url =
    let webClient = new WebClient()
    webClient.DownloadString(Uri url)
    let browser =
        new WebBrowser(ScriptErrorsSuppressed = true,
            Dock = DockStyle.Fill,
            DocumentText = fsharpOrg)
    let form = new Form(Text = "Hello from F#!")
    form.Controls.Add browser
    form.Show()

navigatetoUrl "http://fsharp.org"
