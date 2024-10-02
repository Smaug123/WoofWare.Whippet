# WoofWare.Whippet.Core

This library defines the types you will need to create a plugin that works with the WoofWare.Whippet source generator,
as well as some types which you may find convenient for this purpose.

To the greatest extent possible, WoofWare.Whippet is structured so that if you wish, you do not need to use this library.
However, there are some types you *must* use.

## Mandatory types

When defining any method which performs source generation, you must use the appropriate input type
(such as `RawSourceGenerationArgs`).
We try *very hard* to ensure we never break backward compatibility, so you should safely be able to use old versions of
WoofWare.Whippet.Core even with new versions of the WoofWare.Whippet generator.

## Optional types

* You must decorate your plugin types with an attribute named `[<WhippetGenerator>]`. We supply one you can use.
* We supply interfaces from which you can inherit, to ensure that you are providing members of the right type signature.
