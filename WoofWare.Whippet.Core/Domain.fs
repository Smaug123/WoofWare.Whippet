namespace WoofWare.Whippet.Core

(*
These types should take no dependencies and should only change additively; otherwise consumers will break!
*)

/// When decorating a type, indicates that the type contains Whippet generators.
///
/// If you don't want to take a dependency on WoofWare.Whippet.Core, you can define your own attribute with this name,
/// and we'll detect it happily when running the plugin.
type WhippetGeneratorAttribute () =
    inherit System.Attribute ()

/// The arguments we'll give you (a plugin) when we call you to generate some code from raw file input.
type RawSourceGenerationArgs =
    {
        /// Full path to the file, on disk, which you're taking as input.
        FilePath : string
        /// Contents of the file; you might want to `System.Text.Encoding.UTF8.GetString` this.
        FileContents : byte[]
    }

/// We provide this interface as a helper to give you compile-time safety, but you don't have to use it.
/// At runtime, we'll find any member with the right name and signature.
/// You must use `RawSourceGenerationArgs`, though!
type IGenerateRawFromRaw =
    /// Return `null` to indicate "I don't want to do any updates".
    abstract member GenerateRawFromRaw : RawSourceGenerationArgs -> string
