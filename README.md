# ExpScaffold
An Experiment tracking framework.

The basic idea behind this framework is to provide push-button automation for tracking experiments and reproducing results.
  1. Clone this repository, 
  2. add your code, etc, and run git add to ensure all the files that are part of the experiment are staged.
  3. edit runExpt.fsx to include the command that you want to execute to invoke your experiment.
  4. run runExpt.cmd (for bash users I'll add runExpt.sh)

Now a few things happen:
* your code is committed to the local git repo and tagged with a timestamp.
* A fork is created in Experiments/(timestamp)
* The command specified in runExpt.fsx is invoked in the fork (be sure to add any necessary build steps).
* When the command completes, if a file "results.txt" is present in the fork, it's committed to the fork.

Requirements:
* fsi.exe in your path.
* git in your path. (currently the default windows path is hard-coded in expUtil\expUtil.fsx)

TODO:
* capture commit descriptions
* provide some facilities to list experiments.
* provide facilities for automating things like parameter sweeps?
