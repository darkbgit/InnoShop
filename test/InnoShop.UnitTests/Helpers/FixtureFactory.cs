using AutoFixture;
using AutoFixture.AutoMoq;

namespace InnoShop.UnitTests.Helpers;

public static class FixtureFactory
{
    public static IFixture GetFixture()
    {
        return new Fixture().Customize(new AutoMoqCustomization());
    }
}
