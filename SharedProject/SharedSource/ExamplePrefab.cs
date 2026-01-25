using HarmonyLib;
using Barotrauma;
using Microsoft.Xna.Framework;

namespace Examples;

// Custom prefab can be found at Content/ExamplePrefab.xml in the content package project
public class ExampleFile : GenericPrefabFile<ExamplePrefab>
{
    public ExampleFile(ContentPackage contentPackage, ContentPath path) : base(contentPackage, path) { }

    protected override bool MatchesSingular(Identifier identifier) => identifier == "Example";
    protected override bool MatchesPlural(Identifier identifier) => identifier == "Examples";
    protected override ExamplePrefab CreatePrefab(ContentXElement element) => new(this, element);
    protected override PrefabCollection<ExamplePrefab> Prefabs => ExamplePrefab.Prefabs;
}

public class ExamplePrefab : Prefab
{
    public static readonly PrefabCollection<ExamplePrefab> Prefabs = new();

    public int SomeProperty { get; set; }

    public ExamplePrefab(ContentFile file, ContentXElement element) : base(file, element.GetAttributeIdentifier("identifier", Identifier.Empty))
    {
        SomeProperty = element.GetAttributeInt(nameof(SomeProperty), 0);
    }

    public override void Dispose()
    {

    }
}