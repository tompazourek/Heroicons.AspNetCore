using System;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;
using static Heroicons.AspNetCore.Mvc.TagHelpers.Generator.HeroiconSourceGenerator;

namespace Heroicons.AspNetCore.Mvc.TagHelpers.Generator.Tests;

public class HeroiconSourceGeneratorTests
{
    [Fact]
    public void IconGetAll_NotEmpty()
    {
        var iconFiles = Icon.GetAll();
        iconFiles.Should().NotBeEmpty();
    }

    [Fact]
    public void GenerateInternalTagBuilderFactory_Sample()
    {
        var icon = Icon.GetAll().Single(x => x.Kind == "Outline" && x.Name == "AcademicCap");
        var sb = new StringBuilder();
        GenerateSingleTagBuilderFactory(sb, icon);
        var result = sb.ToString();
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateHeroiconTagBuilderFactoryType_Sample()
    {
        var icons = Icon.GetAll();
        var kinds = icons.Select(x => x.Kind).Distinct().OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
        var names = icons.Select(x => x.Name).Distinct().OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
        var sb = new StringBuilder();
        GenerateHeroiconTagBuilderFactoryType(sb, kinds, names);
        var result = sb.ToString();
        result.Should().NotBeNullOrEmpty();
    }
}
