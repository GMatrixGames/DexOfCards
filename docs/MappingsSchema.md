# MappingsSchema

Location: `CUE4Parse/CUE4Parse/MappingsProvider/MappingsSchema.cs`  
Namespace: `CUE4Parse.MappingsProvider`

Contains schema classes used to represent Unreal Engine reflection mappings. These define how **UStructs**, **UProperties**, and related types are serialized and introspected in memory or mapping files (e.g., for property reconstruction and blueprint parsing).

## Overview

This file provides the following core mapping types:

- **`Struct`** – Base representation of a reflected Unreal struct/class with properties and inheritance.
- **`SerializedStruct`** – Runtime reflection representation of `UStruct` instances extracted directly from Unreal assets.
- **`PropertyInfo`** – Holds metadata about individual reflected properties.
- **`PropertyType`** – Describes Unreal property types and their inner relationships (arrays, maps, sets, enums, etc.).

---

## `Struct` Class

Represents a logical Unreal Engine struct or class, including its parent type (if any) and property list.

### Fields / Properties
| Name | Type | Description |
|------|------|-------------|
| `Context` | `TypeMappings?` | The parent type mapping context containing this struct. |
| `Name` | `string` | Struct or class name. |
| `SuperType` | `string?` | Optional name of the parent type. |
| `Super` | `Lazy<Struct?>` | Lazily resolved parent struct reference. |
| `Properties` | `Dictionary<int, PropertyInfo>` | Indexed property definitions. |
| `PropertyCount` | `int` | Number of properties in this struct (excluding inherited). |

### Constructors

- `Struct(TypeMappings? context, string name, int propertyCount)`  
  Initializes a struct with its context and property count.

- `Struct(TypeMappings? context, string name, string? superType, Dictionary<int, PropertyInfo> properties, int propertyCount)`  
  Initializes with a defined parent type and property dictionary.  
  The `Super` field resolves its parent from the `Context.Types` dictionary.

### Methods

#### `bool TryGetValue(int i, out PropertyInfo info)`
Attempts to retrieve the property info for the given index.  
If not found locally, recursively searches parent structs (`Super`).

---

## `SerializedStruct` Class

Derived from `Struct`. Represents a live in-memory Unreal `UStruct` object loaded from the asset registry.

### Constructor

`SerializedStruct(TypeMappings? context, UStruct struc)`

- Initializes the mapping from a given `UStruct` object.
- Builds `PropertyInfo` entries for each `FProperty` in `ChildProperties`.
- Recursively initializes inherited structs via `SuperStruct.Load<UStruct>()`.

### Behavior

- Warns via Serilog if a `UScriptClass` has missing property mappings.
- Each Unreal property (`FArrayProperty`, `FMapProperty`, etc.) is wrapped into a `PropertyInfo` entry for indexed lookup.

---

## `PropertyInfo` Class

Represents metadata for an Unreal Engine property mapping.

### Fields / Properties
| Name | Type | Description |
|------|------|-------------|
| `Index` | `int` | Property index in the struct. |
| `Name` | `string` | Property name. |
| `ArraySize` | `int?` | Size if this is an array property. |
| `MappingType` | `PropertyType` | Describes type and structure of the property. |

### Methods
- `ToString()` → Returns `"Index/ArraySize -> Name"`.
- `Clone()` → Creates a shallow copy using `MemberwiseClone()`.

---

## `PropertyType` Class

Encapsulates Unreal Engine property typing metadata, including nested types (arrays, maps, sets, structs, enums).

### Fields / Properties
| Name | Type | Description |
|------|------|-------------|
| `Type` | `string` | Property type name (e.g. `IntProperty`, `StructProperty`). |
| `StructType` | `string?` | Struct name for `StructProperty`. |
| `InnerType` | `PropertyType?` | Inner type for arrays, maps, or sets. |
| `ValueType` | `PropertyType?` | Value type for map properties. |
| `EnumName` | `string?` | Name of associated enum type. |
| `IsEnumAsByte` | `bool?` | Indicates if enum is stored as a byte. |
| `Bool` | `bool?` | Marks boolean flag type. |
| `Struct` | `UStruct?` | Linked struct reference if resolved. |
| `Enum` | `UEnum?` | Linked enum reference if resolved. |

### Constructors

- `PropertyType(string type, string? structType = null, PropertyType? innerType = null, PropertyType? valueType = null, string? enumName = null, bool? isEnumAsByte = null, bool? b = null)`  
  Manual definition for basic property types.

- `PropertyType(FProperty prop)`  
  Auto-constructs a type from an Unreal property instance, recursively resolving sub-types (arrays, maps, sets, enums, structs, optionals).

### Private Methods

#### `ApplyEnum(FProperty prop, FPackageIndex enumIndex)`
Extracts enum metadata from Unreal’s property definition, resolving `UEnum` and setting `InnerType` to `IntProperty` for 4-byte enums.

---

## Notes

- Used by **CUE4Parse**’s Mappings system to dynamically reconstruct UE property layouts from both reflection data and static type maps.
- `Struct` and `SerializedStruct` bridge serialized asset data (`UStruct`) with preloaded type mappings (`TypeMappings`).
- `PropertyType` logic mirrors Unreal’s native property system for accurate data introspection.

---

## Example Usage

~~~csharp
// Load and reflect a UStruct from Unreal data
var typeMappings = new TypeMappings();
var ustruct = asset.Export<UStruct>();

// Build a serialized mapping schema
var schema = new SerializedStruct(typeMappings, ustruct);

// Access property metadata
if (schema.TryGetValue(0, out var prop))
{
    Console.WriteLine($"{prop.Name} -> {prop.MappingType.Type}");
}
~~~
