# GuidBase64

[![Build Status](https://travis-ci.com/pyrox18/GuidBase64.svg?branch=master)](https://travis-ci.com/pyrox18/GuidBase64) ![](https://img.shields.io/nuget/v/GuidBase64.svg?style=flat)

A configurable wrapper for `System.Guid` that enables easy conversion of GUIDs to and from base 64 strings.

The base 64 representation uses a URL- and filename-safe character set based on [RFC 4648 Section 5](https://tools.ietf.org/html/rfc4648#section-5) and strips padding by default (see the "Usage: Advanced Configuration" section to learn how to configure this behaviour).

This library borrows implementation concepts from [this blog post by Mads Kristensen](https://madskristensen.net/blog/a-shorter-and-url-friendly-guid/) and [this blog post by Dave Transom](https://www.singular.co.nz/2007/12/shortguid-a-shorter-and-url-friendly-guid-in-c-sharp/).

## Installation

Download and install this package from NuGet using the Package Manager Console, .NET CLI or Visual Studio's NuGet Package Manager.

```bash
PM> Install-Package GuidBase64
# OR
$ dotnet add package GuidBase64
```

## Usage

### Basic Usage

Import the `GuidBase64` namespace. Note that the namespace name is different from the name of the primary type that will be used (`GuidBase64` vs `Base64Guid`).

Use `Base64Guid.NewBase64Guid()` to generate a new base 64 GUID. This method is synonymous with `Guid.NewGuid()`.

```cs
Base64Guid a = Base64Guid.NewBase64Guid();
```

To quickly convert an existing `Guid` object to a `Base64Guid`, use the `ToBase64Guid()` extension method (found in the `GuidBase64.Extensions` namespace):

```cs
Guid guid = Guid.NewGuid();
Base64Guid a = guid.ToBase64Guid();
```

If you just want the base 64 string representation of the `Guid` object, call the `ToBase64String()` extension method instead.

```cs
Guid guid = Guid.NewGuid();
string a = guid.ToBase64String();
```

To convert a base 64 string back to a `Base64Guid` object, parse the string with the `Parse` or `TryParse` methods.

```cs
string str = guid.ToBase64String();

Base64Guid a = Base64Guid.Parse(str);
// OR
bool isParseSuccess = Base64Guid.TryParse(str, out Base64Guid result);
```

You can get the `Guid` equivalent of the `Base64Guid` object by simply using the `Guid` property.

```cs
Base64Guid a = Base64Guid.NewBase64Guid();
Guid guid = a.Guid;
```

String and byte array representations of the `Base64Guid` object can also be obtained with the `ToByteArray` and `ToString` methods.

```cs
Base64Guid a = Base64Guid.NewBase64Guid();
byte[] bytes = a.ToByteArray();
string encoded = a.ToString();
```

The equality of two `Base64Guid` objects can be compared using the `==` and `!=` operators.

```cs
Base64Guid a = new Base64Guid("n0ykxjp2JEWMCwTFmfABpg");
Base64Guid b = new Base64Guid("n0ykxjp2JEWMCwTFmfABpg");

Console.WriteLine(a == b); // true
Console.WriteLine(a != b); // false
```

A `Base64Guid` instance can be implicitly converted to a `Guid` or `string` with equivalent values.

```cs
Base64Guid a = Base64Guid.NewBase64Guid();
Guid b = a;
string c = a;
```

The following constructors can be used to create your own `Base64Guid`.

```cs
Base64Guid a = new Base64Guid(); // synonymous with new Guid()

byte[] buffer = // get buffer from somewhere
Base64Guid b = new Base64Guid(buffer); // synonymous with new Guid(byte[])

Guid guid = Guid.NewGuid();
Base64Guid c = new Base64Guid(guid); // alternative to create from existing GUID

string encoded = "AAAAAAAAAAAAAAAAAAAAAA";
Base64Guid d = new Base64Guid(encoded); // synonymous with new Guid(string), but for base64
```

### ASP.NET Core Model Binding

The `Base64Guid` class has its own implementation of `TypeConverter`, which means that the class can be used for route or query parameter binding in ASP.NET Core.

```cs
[HttpGet("{id}")]
public IActionResult Get(Base64Guid id) // route parameter
{
    // controller logic
}

[HttpGet("values")]
public IActionResult Get(Base64Guid id) // query parameter
{
    // controller logic
}
```

### Advanced Configuration

By default, `Base64Guid` uses the URL-safe base 64 character set specified in [RFC 4648 Section 5](https://tools.ietf.org/html/rfc4648#section-5) and strips any padding when converting to a string representation. However, this behaviour can be customised using a lambda `Action` method that can be provided to all methods that create a `Base64Guid` object. Here are examples of configuration with some of the methods mentioned above:

```cs
Base64Guid a = Base64Guid.NewBase64Guid(options => options.UseStandardBase64Encoding());

string b = Guid.NewGuid().ToBase64String(options => options.UsePadding());

bool isParseSuccess = Base64Guid.TryParse(b, options =>
{
    options.UseStandardBase64Encoding();
    options.UsePadding();
}, out Base64Guid result);
```

Available options:

- `UseStandardBase64Encoding()`: Configures the `Base64Guid` object to use the standard base 64 character set with the `+` and `/` characters instead of `-` and `_`, as specified in [RFC 4648 Section 4](https://tools.ietf.org/html/rfc4648#section-4). This format is not URL- and filename-safe.
- `UsePadding()`: Configures the `Base64Guid` object to retain the padding characters present at the end of the base 64 string representation (which is always two `=` characters, i.e. `==`). This may cause issues if used with URLs.

## Contributing

Refer to the CONTRIBUTING.md file for more information on how to contribute to this project.

## License

This library is licensed under the MIT license.