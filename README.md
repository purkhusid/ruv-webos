# RÚV LG webOS App

This application brings RÚV to LG smart TVs

It's a Work In Progress so do not expect it work very well at the moment!

## Current state

* Super basic navigation and UI that lists all shows and episodes
* Video playback (Does not start in fullscreen) on TVs
* The video playback does not work in local dev because the RUV streams are HLS and
most browsers don't support HLS

## Developing

The app is basic web application built [F#](https://docs.microsoft.com/en-us/dotnet/fsharp/get-started/) and the [Fable](https://fable.io/), a F#-to-JavaScript compiler.

### Requirements

* [NodeJS/NPM](https://nodejs.org/en/download/package-manager/)
* [Yarn](https://classic.yarnpkg.com/en/docs/install)
* [.Net Core](https://dotnet.microsoft.com/download)
* [LG webOS SDK](http://webostv.developer.lge.com/sdk/installation/)
  * Only required if you want to test the application in the TV emulator

### Running dev server

1. `yarn start`

This will start a Webpack dev server.

### Running in emulator

1. `yarn start-emulator`

This script expects the LG CLI tools to be available on PATH and that the emulator is running.

### Running on your TV

Follow these [instructions](http://webostv.developer.lge.com/develop/app-test/)

## TODO

* Make video plaback start in fullscreen
* Needs performance tuning in menus(Too many things being loaded)
* Additional information about programs/episodes
* Better navigation that works with mouse/magic remote and normal remote
* Beautiful UI
