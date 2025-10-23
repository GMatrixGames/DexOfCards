# WwiseFnv

Location: `CUE4Parse/CUE4Parse/UE4/Wwise/WwiseFnv.cs`  
Namespace: `CUE4Parse.UE4.Wwise`

Provides **FNV-1a hashing** utilities for Wwise asset identifiers.  
Used to compute consistent 32-bit hash values for sound entry names within Wwise `.bnk` and metadata files.

- Type: `static class`
- Accessibility: `public`

## Highlights

- Implements the **FNV-1a (Fowler–Noll–Vo)** 32-bit hash algorithm.
- Normalizes input to **lowercase UTF-8** for deterministic Wwise compatibility.
- Used by `AkEntry` and related systems to resolve hashed names to readable identifiers.

## Methods

### `uint GetHash(string name)`
Converts the input string to lowercase (using `ToLowerInvariant`) and returns its FNV hash.

**Usage:**
~~~csharp
uint hash = WwiseFnv.GetHash("Explosion_Sound");
~~~

### `uint GetHashLower(string lowerName)`
Computes the hash from a pre-lowercased string.  
Avoids redundant normalization for performance when input is already lowercase.

**Usage:**
~~~csharp
uint hash = WwiseFnv.GetHashLower("explosion_sound");
~~~

### `uint ComputeHash(byte[] nameBytes)` *(private)*
Core implementation of the **FNV-1a 32-bit** hashing algorithm.

**Algorithm details:**
1. Starts with the **offset basis** `2166136261`.
2. For each byte:
   - Multiplies hash by **prime constant** `16777619`.
   - XORs with the current byte.
   - Clamps to 32 bits using `& 0xFFFFFFFF`.

**Returns:**  
Deterministic 32-bit unsigned integer hash.

## Notes

- Wwise uses FNV hashing for asset and event name resolution in `.bnk` soundbank metadata.
- The lowercase transformation ensures consistent hashing across platforms and localization differences.
- The result matches the hashing behavior used internally by the Wwise authoring tools.

## Example Usage

~~~csharp
// Compute FNV hash for a Wwise sound event
string eventName = "Play_Music_Lobby";
uint eventHash = WwiseFnv.GetHash(eventName);

Console.WriteLine($"{eventName} -> 0x{eventHash:X8}");
~~~
