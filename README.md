# YoshiStat

YoshiStat is an open-source project for a Smart Thermostat built using C#, the open-source Meadow software stack, and (currently) the YoshiPi hardware with is based on the Raspberry Pi Zero 2W.

Most development is being live-streamed on Twitch and edited recordings are on YouTube.

## Building YoshiStat

YoshoStat, generally, has 2 code branches,  `develop` and `main`.  `main` will be the "Release" branch and `develop` is where ongoing work happens.  The primary difference between the two is that `main` used only NuGet references for libraries, and `develop` uses project references to to open-source meadow libraries.

### Building the `develop` branch

Because `develop` uses project references, in order to build it's important to have all associated repositories pulled, and pulled at the same relative positions as is set up in the repo.

It needs to look like this:

```
repos/
├── wilderness
|   └── Meadow.Contracts/
|   └── Meadow.Core/
|   └── [other meadow repos]
├── dotnetMakers
    └── YoshiStat/
        └── Source/
            .gitignore
            README.md
            ...etc
```

We probably should create a tool that aids in this, but right now the simplest way, if you have the Meadow CLI tools installed would be

1. Create a root `repos` folder, then add two children to it: `wilderness` and `dotnetmakers`
2. Using your tool of choice, clone the YoshiStat repo into the proper location (see the tree above)
3. Using a Terminal, navigate to the `repos/wilderness` folder
4. Run the following:
```
PS F:\repos\wilderness> meadow source clone
...
PS F:\repos\wilderness> meadow source checkout develop
```
This will clone all of the requisite Meadow repositories and change them all to their own respective `develop` branches