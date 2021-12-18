![Heroicons.AspNetCore logo](https://raw.githubusercontent.com/tompazourek/Heroicons.AspNetCore/master/assets/logo_32.png) Heroicons.AspNetCore
=====================

*[Heroicons](https://heroicons.com/) that are easy to use in ASP.NET Core MVC as TagHelpers.*

[![Build status](https://img.shields.io/appveyor/ci/tompazourek/heroicons-aspnetcore/master.svg)](https://ci.appveyor.com/project/tompazourek/heroicons-aspnetcore)
[![Tests](https://img.shields.io/appveyor/tests/tompazourek/heroicons-aspnetcore/master.svg)](https://ci.appveyor.com/project/tompazourek/heroicons-aspnetcore/build/tests)
[![codecov](https://codecov.io/gh/tompazourek/Heroicons.AspNetCore/branch/master/graph/badge.svg?token=31JTU6543K)](https://codecov.io/gh/tompazourek/Heroicons.AspNetCore)
[![NuGet version](https://img.shields.io/nuget/v/Heroicons.AspNetCore.Mvc.TagHelpers.svg)](https://www.nuget.org/packages/Heroicons.AspNetCore.Mvc.TagHelpers/)
[![NuGet downloads](https://img.shields.io/nuget/dt/Heroicons.AspNetCore.Mvc.TagHelpers.svg)](https://www.nuget.org/packages/Heroicons.AspNetCore.Mvc.TagHelpers/)


The library is written in C# and released with an [MIT license](https://raw.githubusercontent.com/tompazourek/Heroicons.AspNetCore.Mvc.TagHelpers/master/LICENSE), so feel **free to fork** or **use commercially**.

**Any feedback is appreciated, please visit the [issues](https://github.com/tompazourek/Heroicons.AspNetCore.Mvc.TagHelpers/issues?state=open) page or send me an [e-mail](mailto:tom.pazourek@gmail.com).**

Download
--------

Binaries of the last build can be downloaded on the [AppVeyor CI page of the project](https://ci.appveyor.com/project/tompazourek/heroicons-aspnetcore/build/artifacts).

The library is also [published on NuGet.org](https://www.nuget.org/packages/Heroicons.AspNetCore.Mvc.TagHelpers/), install using:

```
PM> Install-Package Heroicons.AspNetCore.Mvc.TagHelpers
```

<sup>The package is built only for .NET 6 and ASP.NET MVC Core.</sup>

Usage
-----

- Add package to your ASP.NET MVC Core project.
- Find `_ViewImports.cshtml` file and add the following line:

```cshtml
@addTagHelper *, Heroicons.AspNetCore.Mvc.TagHelpers
```

- In your `*.cshtml` views, you can then use the icon as tag helper:

```cshtml
<heroicon kind="Solid" name="Mail" />
```

- The `kind` corresponds to style kind enum, currently, either `Solid` or `Outline`.
- The `name` corresponds to individual icons, currently there are over 200 of them. See https://heroicons.com/ for the complete overview.
- Note that you can easily apply additional attributes (e.g. CSS classes), which makes it great for use with Tailwind CSS:

```cshtml
<heroicon class="ml-2 -mr-0.5 h-4 w-4" kind="Solid" name="Mail" />
```

- You can also try the sample project in the `tests/` folder to see it running.
