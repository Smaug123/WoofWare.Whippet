namespace WoofWare.Whippet.Plugin.Json

open System

/// Attribute indicating a record type to which the "Add JSON serializer" Whippet
/// generator should apply during build.
/// The purpose of this generator is to create methods (possibly extension methods) of the form
/// `{TypeName}.toJsonNode : {TypeName} -> System.Text.Json.Nodes.JsonNode`.
///
/// If you supply isExtensionMethod = false, you will get a module rather than extension methods.
/// Extension methods can only be consumed from F#, but the benefit is that they don't use up the module name.
/// (If you set this to `false`, we create a module called "{TypeName}").
type JsonSerializeAttribute (isExtensionMethod : bool) =
    inherit Attribute ()

    /// The default value of `isExtensionMethod`, the optional argument to the JsonSerializeAttribute constructor.
    static member DefaultIsExtensionMethod = true

    /// Shorthand for the "isExtensionMethod = false" constructor; see documentation there for details.
    new () = JsonSerializeAttribute JsonSerializeAttribute.DefaultIsExtensionMethod

/// Attribute indicating a record type to which the "Add JSON parse" Whippet
/// generator should apply during build.
/// The purpose of this generator is to create methods (possibly extension methods) of the form
/// `{TypeName}.jsonParse : System.Text.Json.Nodes.JsonNode -> {TypeName}`.
///
/// If you supply isExtensionMethod = false, you will get extension methods.
/// Extension methods can only be consumed from F#, but the benefit is that they don't use up the module name
/// (If you set this to `false`, we create a module called "{TypeName}").
type JsonParseAttribute (isExtensionMethod : bool) =
    inherit Attribute ()

    /// The default value of `isExtensionMethod`, the optional argument to the JsonParseAttribute constructor.
    static member DefaultIsExtensionMethod = true

    /// Shorthand for the "isExtensionMethod = false" constructor; see documentation there for details.
    new () = JsonParseAttribute JsonParseAttribute.DefaultIsExtensionMethod
