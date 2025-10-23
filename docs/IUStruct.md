# IUStruct

Location: `CUE4Parse/CUE4Parse/UE4/IUStruct.cs`  
Namespace: `CUE4Parse.UE4`

Defines a **marker interface** representing Unreal Engine struct-like objects within the CUE4Parse framework.  
Acts as a common contract for any class or data type that models a **UStruct** or similar Unreal reflection type.

- Type: `interface`
- Accessibility: `public`

## Highlights

- Provides a **type identifier** for Unreal Engine struct representations.
- Enables **generic handling** of any `UStruct`-based data structure in parsing or serialization systems.
- Commonly implemented by Unreal property wrappers, reflection types, and runtime-constructed data containers.

## Members

None — this is an **empty (marker) interface** used purely for type distinction and polymorphism.

## Notes

- Serves as a unifying interface for Unreal structs like `FVector`, `FRotator`, or complex reflected `UStruct` classes.
- Often used for type constraints (`where T : IUStruct`) to enforce Unreal data model compatibility.
- Provides no direct behavior but is essential for reflection and asset deserialization within the CUE4Parse architecture.

## Example Usage

~~~csharp
// Example struct implementing IUStruct
public class FVector : IUStruct
{
    public float X;
    public float Y;
    public float Z;
}

// Generic utility constrained to Unreal structs
public void PrintStruct<T>(T value) where T : IUStruct
{
    Console.WriteLine($"Processing Unreal struct of type {typeof(T).Name}");
}
~~~
