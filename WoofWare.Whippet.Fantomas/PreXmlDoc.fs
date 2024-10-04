namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Xml
open Fantomas.FCS.Text.Range

/// Module for manipulating the PreXmlDoc AST type (which represents XML docstrings).
[<RequireQualifiedAccess>]
module PreXmlDoc =
    /// Pass an arbitrary string in here, with newlines if you like, and we'll make sure it appears correctly.
    let create (s : string) : PreXmlDoc =
        let s = s.Split "\n"

        for i = 0 to s.Length - 1 do
            s.[i] <- " " + s.[i]

        PreXmlDoc.Create (s, range0)

    /// This is a light wrapper around Fantomas's `PreXmlDoc`; you should probably use `create` instead.
    let create' (s : string seq) : PreXmlDoc =
        PreXmlDoc.Create (Array.ofSeq s, range0)
