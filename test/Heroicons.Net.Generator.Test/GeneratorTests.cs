using System.IO;
using FluentAssertions;
using Xunit;
using static System.IO.Path;

namespace Heroicons.Net.Generator.Test;

public class GeneratorTests
{
    private static FileInfo GetHeroiconsFile(string relativePath)
        => new(GetFullPath(Combine(
            Directory.GetCurrentDirectory(),
            "../../../../../src/Heroicons.Net.Generator/node_modules/heroicons",
            relativePath)));

    [Fact]
    public void SampleFile_Exists()
    {
        var sampleFile = GetHeroiconsFile("./outline/calculator.svg");
        sampleFile.Exists.Should().BeTrue();
    }
}
