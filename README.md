# MarkovChainSim

## Description

### Source code main dir
[Scripts](_Scripts)

### Markov Chain Model
[Model](_Scripts/MarkovChainModel)

### Model Modules
[Modules](_Scripts/MarkovChainModel/Modules)

The modules are the heart of the whole model system -- the example SEIR-D model(s) are built from these modules (as discussed yesterday) -- but the Markov Chain Model system is not necessarily a SEIR-D model per-se (again, as discussed).

Then once you have a general idea of how the various modules behave -- they are designed to be the atomic elements of any simple or complex system -- you can generally create a model for any system through a graph representation of these processes.

### Control, events, and aggregator systems
[System](_Scripts/MarkovChainModel/System)

To understand the control and flow of the system it is good to know that the Markov processes (I refer to each bounded context as a 'machine') are driven by an event system. The event system is using the single-threaded nature of the Unity runtime to demarcate event boundaries and calling a single event per frame, with the event chain being triggered by a simulation step event from the simulation 'conductor'.

There are also various other elements that  do things like aggregate parameters for easy management, aggregate value histories, and aggregate exporters (to local JSON files).

## Installation

 - Create New Empty Unity Project (Any Template, Recommended Version 2021.X LTS)
 - Delete Any Contents in Assets Folder
 - Download and Unzip MarkovChainSim Repo within Empty Asset Folder
 - Install Unity Mathematics Plugin
 
## Dependencies
 
### Without Any Additional Asset Installations

 - The Simulation Will Run 
 - All Parameters Are Editable
 - Parameters and Simulation Results Are Fully Exportable From the Editor UI
 - New Models Can be Generated at Runtime via APIs
 
### Additional Assets Required for Building and Modifying Models in the Editor 

 - Two Common Unity Assets are Required for Full Editor Functionality 
 - Asset: Odin Serializer
 - Asset: Odin Validator

## Additional Information

Copyright (C) 2022 Matthew W. McKenzie and Kenz LLC

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.



## Odin Serializer & Odin Validator
Copyright (c) Sirenix ApS. All rights reserved.
