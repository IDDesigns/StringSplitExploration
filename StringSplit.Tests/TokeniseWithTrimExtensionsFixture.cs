namespace StringSplit.Tests;

[TestFixture]
public class TokeniseWithTrimExtensionsFixture
{
    [Test]
    public void BeforeMoveNext()
    {
        Assert.That(() => _ = "type".AsSpan().TokeniseWithTrim().Current, Throws.InstanceOf<InvalidOperationException>());
    }

    [Test]
    public void EmptyString()
    {
        var enumerator = "".AsSpan().TokeniseWithTrim();

        Assert.That(enumerator.MoveNext(), Is.False);
        try
        {
            _ = enumerator.Current;
            Assert.Fail();
        }
        catch (InvalidOperationException)
        {
            Assert.Pass();
        }
    }

    [Test]
    public void SingleEntry()
    {
        var enumerator = "value".AsSpan().TokeniseWithTrim();

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value", StringComparison.Ordinal), Is.True);
        Assert.That(enumerator.MoveNext(), Is.False);
    }

    [Test]
    public void MultipleEntries()
    {
        var enumerator = "value1,value2,value3".AsSpan().TokeniseWithTrim();

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value1", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value2", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value3", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.False);
    }

    [Test]
    public void SingleEntryWithTrim()
    {
        var enumerator = " value   ".AsSpan().TokeniseWithTrim();

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value", StringComparison.Ordinal), Is.True);
        Assert.That(enumerator.MoveNext(), Is.False);
    }

    [Test]
    public void MultipleEntriesWithTrim()
    {
        var enumerator = "value1 , value2, value3 ".AsSpan().TokeniseWithTrim();

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value1", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value2", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value3", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.False);
    }

    [Test]
    public void SingleEntryWhiteSpace()
    {
        var enumerator = "   ".AsSpan().TokeniseWithTrim();

        Assert.That(enumerator.MoveNext(), Is.False);
    }

    [Test]
    public void MultipleEntriesWhiteSpace()
    {
        var enumerator = " ,value1 ,, value2, value3 ,   , ".AsSpan().TokeniseWithTrim();

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value1", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value2", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.True);
        Assert.That(enumerator.Current.Equals("value3", StringComparison.Ordinal), Is.True);

        Assert.That(enumerator.MoveNext(), Is.False);
    }
}