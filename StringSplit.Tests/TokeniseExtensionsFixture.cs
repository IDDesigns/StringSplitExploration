namespace StringSplit.Tests;

[TestFixture]
public class TokeniseExtensionsFixture
{
    [Test]
    public void BeforeMoveNext()
    {
        Assert.That("type".AsSpan().Tokenise().Current.Length, Is.EqualTo(0));
    }

    [Test]
    public void EmptyString()
    {
        var enumerator = "".AsSpan().Tokenise();

        Assert.That(enumerator.MoveNext(), Is.False);
        Assert.That(enumerator.Current.Length, Is.EqualTo(0));
    }

    [Test]
    public void SingleEntry()
    {
        var enumerator = "value".AsSpan().Tokenise();

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value", StringComparison.Ordinal), Is.True);
        Assert.That(enumerator.MoveNext(), Is.False);
    }

    [Test]
    public void MultipleEntries()
    {
        var enumerator = "value1,value2,value3".AsSpan().Tokenise();

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value1", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value2", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value3", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.False);
    }
}