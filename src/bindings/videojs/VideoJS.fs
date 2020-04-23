module VideoJS

open Fable.Core
open Feliz
open Browser.Types
open System

type Source =
    { src: string 
      ``type``: string }

type HlsOptions = 
    { withCredentials: bool }

type Options = 
    { autoplay: bool 
      controls: bool 
      fill: bool 
      sources: Source list
      hls: HlsOptions option }

let defaultOptions =
    { autoplay = false 
      controls = false
      fill = false
      sources = []
      hls = None }

[<Erase>]
type Element =
    | ElementId of string
    | Element of HTMLElement option

type IPlayer =
    abstract dispose: unit -> unit
    abstract src: Source -> unit
    abstract requestFullscreen: unit -> unit

[<ImportDefault("video.js")>]
[<Emit("$0($1, $2, $3)")>]
let videojs (element: Element) (options: Options) (onPlayerReady: unit -> unit): IPlayer = jsNative
