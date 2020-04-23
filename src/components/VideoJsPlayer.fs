module VideoJsPlayer

open Feliz
open System

type VideoJSPlayer = 
    { source: VideoJS.Source 
      playerOptions: VideoJS.Options }

let videoJSPlayer = React.functionComponent("VideoJS", fun (props: VideoJSPlayer) ->
        let playerRef = React.useElementRef()

        let createPlayer () =
            let player = VideoJS.videojs (VideoJS.Element playerRef.current) props.playerOptions (ignore)
            player.src props.source
            player.requestFullscreen()

            { new IDisposable with member this.Dispose() = player.dispose() }

        React.useEffect (createPlayer, [||])

        Html.div [
            prop.style [ 
                // style.position.fixedRelativeToWindow
                // style.custom ("width","100%")
                // style.custom ("height","100%")
            ]
            prop.children [
                Html.div [
                    prop.disabled true
                    prop.custom ("data-vjs-player", "")
                    prop.children [
                        Html.video [
                            prop.className "video-js"
                            prop.ref playerRef
                        ]
                    ]
                ]
            ]
        ]        
    )