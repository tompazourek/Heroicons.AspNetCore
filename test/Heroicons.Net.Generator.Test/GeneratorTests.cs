using System.IO;
using System.Text;
using FluentAssertions;
using Xunit;
using static System.IO.Path;

namespace Heroicons.Net.Generator.Test;

public class GeneratorTests
{
    private static DirectoryInfo GetHeroiconsRoot()
        => new(GetFullPath(Combine(
            Directory.GetCurrentDirectory(),
            "../../../../../src/Heroicons.Net.Generator/node_modules/heroicons")));

    private static FileInfo GetHeroiconsFile(string relativePath)
        => new(GetFullPath(Combine(
            GetHeroiconsRoot().FullName,
            relativePath)));

    [Fact]
    public void SampleFile_Exists()
    {
        var sampleFile = GetHeroiconsFile("./outline/calculator.svg");
        sampleFile.Exists.Should().BeTrue();
    }

    [Fact]
    public void SampleFile_GenerateSingleMethod()
    {
        var sampleFile = GetHeroiconsFile("./solid/credit-card.svg");
        var generator = new TagBuilderGenerator();
        var sb = new StringBuilder();
        generator.GenerateMethod(sb, 0, sampleFile);
        var result = sb.ToString();
    }

    [Fact]
    public void SampleFile_GenerateMultipleMethods()
    {
        var heroiconsRoot = GetHeroiconsRoot();
        var generator = new TagBuilderGenerator();
        var sb = new StringBuilder();
        var iconFiles = generator.GetIconFiles(heroiconsRoot);
        generator.GenerateMethods(sb, 0, iconFiles);
        var result = sb.ToString();
    }

    [Fact]
    public void SampleFile_GenerateKindsFile()
    {
        var heroiconsRoot = GetHeroiconsRoot();
        var generator = new TagBuilderGenerator();
        var sb = new StringBuilder();
        var iconFiles = generator.GetIconFiles(heroiconsRoot);
        generator.GenerateHeroiconKindFile(sb, 0, iconFiles);
        var result = sb.ToString();
    }

    [Fact]
    public void SampleFile_GenerateNamesFile()
    {
        var heroiconsRoot = GetHeroiconsRoot();
        var generator = new TagBuilderGenerator();
        var sb = new StringBuilder();
        var iconFiles = generator.GetIconFiles(heroiconsRoot);
        generator.GenerateHeroiconNameFile(sb, 0, iconFiles);
        var result = sb.ToString();
    }
}
