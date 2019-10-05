# X-Plane Launcher

X-Plane Launcher is a small windows desktop application assisted by a FWL script to give a overview of used planes, their locations and lets you start the sim with a specific plane.

### System Requirements

- Windows
- .net Framework (not dotnet core)
- Fly with LUA
- having x-plane setup to auto start with "default.sit"

### Setup

Have a look at App.config/XPlaneLauncher.exe.config.

### Features Script

- periodically ...
  - generate unique name from the currently flown plane (plane + livery)
  - generate a json file with position, heading, aircraft and livery path
  - save the current situation

### Features Desktop App

- Get json files contents
- list planes by name/livery
  - with thumbnail if available as *_icon11.png file within the livery folder
- show plane positions on openstreetmap
- planes are selectable and synced in list and map view
- define routes for each plane
- show targets/routes for all planes on the map
- start x-plane with the selected plane on the last known position (saved .sit file from the script)

### What its NOT doing (yet)

Take a look at https://github.com/bergziege/XPlaneLauncher/projects/1 or the issues section.

## Development

The app is currently fully functional. As my time permits (about 1h per months ;-) I will slowly integrate some of the points from the "What its not doing" list.

Issues, ideas and pull requests are welcome!

There will be no version for Linux/Mac from my side. But feel free to fork or use the general launcher idea.
