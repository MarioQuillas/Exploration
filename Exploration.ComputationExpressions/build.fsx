#I ".."
#load @"../packages/FSharp.Formatting.2.14.4/FSharp.Formatting.fsx"
open FSharp.Literate
open System.IO

let source = "C:\MyGithub\Sniper\Sniper.DeedleTuto"
let template = Path.Combine(source, "template.html")

let script = Path.Combine(source, "deedleScript.fsx")
let toto = Literate.ProcessScriptFile(script, template)

//let doc = Path.Combine(source, "../docs/document.md")
//Literate.ProcessMarkdown(doc, template)