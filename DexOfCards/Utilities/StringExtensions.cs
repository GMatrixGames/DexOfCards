using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DexOfCards.Utilities;

public static class StringExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringBefore(this string s, char delimiter)
    {
        var index = s.IndexOf(delimiter);
        return index == -1 ? s : s[..index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringBefore(this string s, string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
    {
        var index = s.IndexOf(delimiter, comparisonType);
        return index == -1 ? s : s[..index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringAfter(this string s, char delimiter)
    {
        var index = s.IndexOf(delimiter);
        return index == -1 ? s : s.Substring(index + 1, s.Length - index - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringAfter(this string s, string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
    {
        var index = s.IndexOf(delimiter, comparisonType);
        return index == -1 ? s : s.Substring(index + delimiter.Length, s.Length - index - delimiter.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringBeforeLast(this string s, char delimiter)
    {
        var index = s.LastIndexOf(delimiter);
        return index == -1 ? s : s[..index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringBeforeLast(this string s, string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
    {
        var index = s.LastIndexOf(delimiter, comparisonType);
        return index == -1 ? s : s[..index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringBeforeWithLast(this string s, char delimiter)
    {
        var index = s.LastIndexOf(delimiter);
        return index == -1 ? s : s[..(index + 1)];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringBeforeWithLast(this string s, string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
    {
        var index = s.LastIndexOf(delimiter, comparisonType);
        return index == -1 ? s : s[..(index + 1)];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringAfterLast(this string s, char delimiter)
    {
        var index = s.LastIndexOf(delimiter);
        return index == -1 ? s : s.Substring(index + 1, s.Length - index - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringAfterLast(this string s, string delimiter, StringComparison comparisonType = StringComparison.Ordinal)
    {
        var index = s.LastIndexOf(delimiter, comparisonType);
        return index == -1 ? s : s.Substring(index + delimiter.Length, s.Length - index - delimiter.Length);
    }
        
    public static string FirstCharToUpper(this string input)
    {
        switch (input)
        {
            case null: throw new ArgumentNullException(nameof(input));
            case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            default: return input.First().ToString().ToUpper() + input[1..];
        }
    }
        
    public static string FirstCharToLower(this string input)
    {
        switch (input)
        {
            case null: throw new ArgumentNullException(nameof(input));
            case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            default: return input.First().ToString().ToLower() + input[1..];
        }
    }
}