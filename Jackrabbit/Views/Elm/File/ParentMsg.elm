module Views.Elm.File.ParentMsg exposing (..)

import Views.Elm.File.Model as File


type ParentMsg
    = NoOp
    | RefreshFiles (List File.FileData)
    | SaveError String
    | RemoveFile
    | AddTempFile File.Model
    | CancelTempForm
