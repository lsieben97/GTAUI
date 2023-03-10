# GTAUI
*A batteries included UI framework for GTA V mods via  scripthookVdotnet*

## Features
> **Note:** See below for an overview of what features are currently implemented.
* Fullscreen alerts / input box / progress bar
* Menus (wrapper over LemonUI's `NativeMenu`)
* Styling system
* UIResources system
* Basic Hierarchical UI Components system
* Draggable and responsive window system
* Global menu bar (like MacOS).

## Installation
See [Getting started](https://github.com/lsieben97/GTAUI/wiki/Getting-Started) on the wiki.

## Current state of the library
This library is very much a work in progress.
The following table shows what features are currently implemented.  
:red_circle:: *Not implemented*  
:wrench:: *Currently being implemented*  
:white_check_mark:: *Implemented*

>**Note:** List is subject to change.

| Feature                                        | Status             | Styling supported? |
|------------------------------------------------|--------------------|--------------------|
| Basic UI Components system                     | :white_check_mark: | :heavy_minus_sign: |
| Keyboard and mouse input                       | :white_check_mark: | :heavy_minus_sign: |
| Styling system                                 | :white_check_mark: | :white_check_mark: |
| UI resources system                            | :white_check_mark: | :heavy_minus_sign: |
| **Fullscreen alerts**                          | :white_check_mark: | :white_check_mark: |
| Fullscreen message alert                       | :white_check_mark: | :white_check_mark: |
| Fullscreen input box                           | :white_check_mark: | :white_check_mark: |
| Fullscreen progress bar                        | :white_check_mark: | :white_check_mark: |
| **Menus**                                      | :wrench:           | :wrench:           |
| Basic `NativeMenu` wrapper                     | :wrench:           | :red_circle:*1*    |
| Creating menus defined in json files           | :wrench:           | :heavy_minus_sign: |
| Creating menus from code                       | :wrench:           | :heavy_minus_sign: |
| Creating menus via fluent builder API          | :wrench:           | :heavy_minus_sign: |
| Custom menu items support                      | :wrench:           | :wrench:           |
| Selection menus                                | :wrench:           | :wrench:           |
| Multiple-selection menus                       | :wrench:           | :wrench:           |
| **Draggable and responsive window system**     | :red_circle:       | :red_circle:       |
| Basic window handling                          | :red_circle:       | :red_circle:       |
| WindowUI components system                     | :red_circle:       | :red_circle:       |
| Creating windows with ui defined in json files | :red_circle:       | :red_circle:       |
| Responsive layout components                   | :red_circle:       | :red_circle:       |
| **WindowUI components**                        | :red_circle:       | :red_circle:       |
| Label                                          | :red_circle:       | :red_circle:       |
| Button                                         | :red_circle:       | :red_circle:       |
| Textbox                                        | :red_circle:       | :red_circle:       |
| Scollable components                           | :red_circle:       | :red_circle:       |
| Listbox                                        | :red_circle:       | :red_circle:       |
| DropdownList                                   | :red_circle:       | :red_circle:       |
| Checkbox                                       | :red_circle:       | :red_circle:       |
| Radio button                                   | :red_circle:       | :red_circle:       |
| Table view                                     | :red_circle:       | :red_circle:       |
| Custom WindowUI Components support             | :red_circle:       | :red_circle:       |
| **Advanced WindowUI components**               | :red_circle:       | :red_circle:       |
| Message box                                    | :red_circle:       | :red_circle:       |
| Open file dialog                               | :red_circle:       | :red_circle:       |
| Save file dialog                               | :red_circle:       | :red_circle:       |
| Folder selection dialog                        | :red_circle:       | :red_circle:       |

*1*: Currently only the text and description of `CloseMenuItem` and `BackMenuItem` are stylable. Full menu styling coming soon...

## Screenshots
*Coming soon...*
