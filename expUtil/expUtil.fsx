module expUtil
open System
open System.IO
open System.Diagnostics

#load "runProc.fsx"
open runProc

// roughly equivalent to posix system()
(*
let system (cmd:string, args:string) =
    eprintfn "Running \"%s\" %s" cmd args
    let p=new ProcessStartInfo()
    p.FileName<-cmd
    p.Arguments<-args
    p.CreateNoWindow<-true
    p.UseShellExecute<-false
    p.RedirectStandardOutput<-true
    p.RedirectStandardError<-true
    use proc = Process.Start(p)
      
    let out=proc.StandardOutput.ReadToEnd()
    proc.WaitForExit();
    (out,err,proc.ExitCode) *)

// take the first argument after 'switch'. If switch isn't present return defaultstr
let argsOrDefault switch defaultStr =
    let ix=Array.IndexOf(fsi.CommandLineArgs, switch)
    if ix>=0 && ix<fsi.CommandLineArgs.Length-1 then 
        fsi.CommandLineArgs.[ix+1]
    else defaultStr

let gitCmd = @"C:\Program Files\Git\cmd\git.exe"
let runGit args = 
    runProc gitCmd args None
    |> fun (err,out) ->
        err|>Seq.iter (eprintfn "%s")
        out|>Seq.iter (eprintfn "%s")
        


// todo: adds and/or tagging/branching.
let gitCommit message = runGit (sprintf "commit -m \"%s\"" message) 
let gitTag tag message = runGit (sprintf "tag -a %s -m \"%s\"" tag message) 
let gitSubmod workingDir = runGit (sprintf "submodule add \"%s\" %s" (Directory.GetCurrentDirectory()) workingDir) 
let gitAdd fn = runGit (sprintf "add %s" fn)
let gitPush () = runGit "push"