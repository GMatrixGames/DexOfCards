# FAesKey

Location: `CUE4Parse/CUE4Parse/Encryption/Aes/FAesKey.cs`  
Namespace: `CUE4Parse.Encryption.Aes`

Represents a 256-bit AES key used for decrypting Unreal Engine VFS archives (`.pak`, `.ucas`, `.utoc`). Provides parsing, validation, and string/byte conversion utilities.

- Type: `class`
- Accessibility: `public`

## Highlights

- Supports AES-256 keys as both **hex strings** (`"0x..."`) and **byte arrays**.
- Ensures strict **32-byte (256-bit)** length validation.
- Provides consistent `0x`-prefixed string representation for keys.
- Detects the “default” all-zero AES key for conditional decryption logic.

## Constructors

### `FAesKey(byte[] key)`
- Initializes an AES key directly from a byte array.
- Validates that `key.Length == 32`, otherwise throws `ArgumentException`.
- Generates a hexadecimal `KeyString` prefixed with `0x`.

### `FAesKey(string keyString)`
- Initializes an AES key from a hexadecimal string representation.
- Automatically prefixes with `0x` if not present.
- Validates that the resulting string length equals `66` (`0x` + 64 hex chars).
- Converts the string into a 32-byte array using `ParseHexBinary()` from `CUE4Parse.Utils`.

## Properties

| Property | Type | Description |
|-----------|------|-------------|
| `Key` | `byte[]` | Raw 32-byte AES key. |
| `KeyString` | `string` | Hexadecimal string form (`0x001122...`). |
| `IsDefault` | `bool` | Returns `true` if all bytes are `0x00`. |

## Methods

### `ToString()`
Returns the hexadecimal string (`KeyString`) representation of the key.

## Exceptions

- `ArgumentException`: Thrown when input key length is invalid (not 32 bytes or 64 hex characters).

## Notes

- The `IsDefault` property is commonly used in decryption pipelines to detect uninitialized or placeholder AES keys.
- `KeyString` format (`0x...`) ensures consistent serialization across the entire CUE4Parse toolset.
- Designed for interop with `AbstractVfsFileProvider`, where AES keys are mapped to `FGuid`s for archive decryption.

## Example Usage

~~~csharp
// Initialize from hex string
var key = new FAesKey("00112233445566778899AABBCCDDEEFF00112233445566778899AABBCCDDEEFF");

// Initialize from byte array
var keyBytes = Enumerable.Range(0, 32).Select(i => (byte)i).ToArray();
var key2 = new FAesKey(keyBytes);

// Check for default (all-zero) key
if (key2.IsDefault)
    Console.WriteLine("Key is not set");

// Use in VFS provider
provider.SubmitKey(guid, key);
~~~
