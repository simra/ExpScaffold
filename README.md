# ExpScaffold
An Experiment tracking framework.

The basic idea behind this framework is to provide push-button automation for tracking experiments and reproducing results.
  1. Clone this repository, 
  2. add your code, etc
  3. edit runExpt.fsx to include the command that you want to execute to invoke your experiment.
  4. run git add to ensure all the files that are part of the experiment are staged. (I would not recommend adding data files here- the goal is to manage code and parameter changes, not huge data sets).
  4. run runExpt.cmd (for bash users I'll add runExpt.sh)

Now a few things happen:
* your code is committed to the local git repo and tagged with a timestamp.
* A fork is created in Experiments/(timestamp)
* The command specified in runExpt.fsx is invoked in the fork (be sure to add any necessary build steps as part of your command).
* When the command completes, if a file "results.txt" is present in the fork, it's committed to the fork.

Requirements:
* fsi.exe in your path. [http://fsharpforfunandprofit.com/installing-and-using/]
* git in your path. (currently the default windows path is hard-coded in expUtil\expUtil.fsx)


