# TrainworksModdingToolsExtended
This library contains upgrades to [TrainworksModdingTools](https://github.com/KittenAqua/TrainworksModdingTools) since it has been abandoned. Credit to the contributors of Trainworks Modding Tools for the original code.

This codebase is a fork with improvements to the Existing Builder classes that is to be used in tandem with the original codebase.

## Changes
Unfortunately TMTE is not a 100% drop in replacement and makes a few API breaking changes. This is a complete redesign of the Builders present in the original.
See [changes.txt](https://github.com/brandonandzeus/TrainworksModdingToolsExtended/blob/main/TrainworksModdingToolsExtended/changes.txt)

For quick instructions on starting the migration
1. Replace `using Trainworks.Builders` with `using Trainworks.BuildersV2`
2. Replace `using Trainworks.Constants` with `using Trainworks.ConstantsV2`
3. Add `using Trainworks.ManagersV2`

Again the Builder API has changed a bit, so expect errors, but Visual Studio should make fixing a snap.
