namespace WoofWare.Whippet.Plugin.InterfaceMock

/// Attribute indicating an interface type for which the "Interface Mock" Whippet
/// generator should apply during build.
/// This generator creates a record which implements the interface,
/// but where each method is represented as a record field, so you can use
/// record update syntax to easily specify partially-implemented mock objects.
/// You may optionally specify `isInternal = false` to get a mock with the public visibility modifier.
type InterfaceMockAttribute (isInternal : bool) =
    inherit System.Attribute ()
    /// The default value of `isInternal`, the optional argument to the GenerateMockAttribute constructor.
    static member DefaultIsInternal = true

    /// Shorthand for the "isExtensionMethod = false" constructor; see documentation there for details.
    new () = InterfaceMockAttribute InterfaceMockAttribute.DefaultIsInternal
