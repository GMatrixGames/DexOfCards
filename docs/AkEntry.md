# AkEntry

Location: `CUE4Parse/CUE4Parse/UE4/Wwise/Objects/AkEntry.cs`  
Namespace: `CUE4Parse.UE4.Wwise.Objects`

Represents a single Wwise audio entry within a `.bnk` or related Wwise data structure. Handles parsing of entry metadata and loading of its associated audio data bytes.

- Type: `class`
- Accessibility: `public`
- JSON Serialization: Uses custom `AkEntryConverter` for `Newtonsoft.Json`.

## Highlights

- Reads structured Wwise entry metadata directly from a binary archive (`FArchive`).
- Automatically loads the associated binary data segment (`Data`).
- Detects whether the entry represents a **sound bank** (`IsSoundBank`).
- Supports integration with CUE4Parse’s audio extraction framework.

## Constructor

### `AkEntry(FArchive Ar)`
Parses a Wwise entry from a binary reader.

**Behavior:**
1. Reads core metadata fields in order:
   - `NameHash` (uint)
   - `OffsetMultiplier` (uint)
   - `Size` (int)
   - `Offset` (uint)
   - `FolderId` (uint)
2. Reads raw binary `Data` using `Ar.ReadBytesAt(Offset, Size)`.
   > ⚠️ Comment in code suggests data offset may alternatively be `Offset * OffsetMultiplier` depending on the specific Wwise asset version.

## Properties

| Property | Type | Description |
|-----------|------|-------------|
| `NameHash` | `uint` | Hashed name identifier for the Wwise entry. |
| `OffsetMultiplier` | `uint` | Potential scaling multiplier for offset calculations. |
| `Size` | `int` | Length of the data segment in bytes. |
| `Offset` | `uint` | Byte offset within the source archive. |
| `FolderId` | `uint` | Identifier for the containing folder or structure. |
| `Path` | `string?` | Optional resolved file path (filled externally). |
| `Data` | `byte[]?` | Raw data bytes of the entry, read from the archive. |
| `IsSoundBank` | `bool` | `true` if the data begins with a Wwise `BankHeader` chunk ID. |

## Related Enums / Dependencies

- **`ChunkID`** – Enum representing known Wwise chunk identifiers (e.g., `BankHeader`).
- **`FArchive`** – CUE4Parse binary reader utility for Unreal asset streams.
- **`AkEntryConverter`** – Custom JSON converter for serialization/deserialization.

## Notes

- The `IsSoundBank` check uses `BitConverter.ToUInt32(Data)` to identify `ChunkID.BankHeader`.
- `Path` is not parsed from the archive; it is typically populated later when reconstructing asset hierarchies.
- Used internally by **CUE4Parse’s Wwise parsing layer** to enumerate entries and extract audio or metadata chunks.

## Example Usage

~~~csharp
// Read a Wwise entry from archive
var entry = new AkEntry(archive);

Console.WriteLine($"Entry Hash: {entry.NameHash}");
Console.WriteLine($"Size: {entry.Size} bytes");

if (entry.IsSoundBank)
{
    Console.WriteLine("Detected Wwise SoundBank data");
}

// Optionally save data to file
if (entry.Data != null)
    File.WriteAllBytes($"output/{entry.NameHash}.bnk", entry.Data);
~~~
