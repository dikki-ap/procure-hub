using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using ProcureHub.SharedKernel.Caching;

namespace ProcureHub.UnitTests.Caching;

public class MemoryCacheServiceTests : IDisposable
{
    private readonly MemoryCache        _memoryCache = new(new MemoryCacheOptions());
    private readonly MemoryCacheService _sut;

    public MemoryCacheServiceTests() => _sut = new MemoryCacheService(_memoryCache);

    public void Dispose() => _memoryCache.Dispose();

    // ── Get ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Get_ForMissingKey_ShouldReturnDefault()
    {
        _sut.Get<string>("missing").Should().BeNull();
        _sut.Get<int>("missing-int").Should().Be(0);
    }

    // ── Set / Get ─────────────────────────────────────────────────────────────

    [Fact]
    public void Set_ThenGet_ShouldReturnStoredValue()
    {
        _sut.Set("key1", "hello", TimeSpan.FromMinutes(5));

        _sut.Get<string>("key1").Should().Be("hello");
    }

    [Fact]
    public void Set_ThenRemove_ShouldReturnNull()
    {
        _sut.Set("key2", "value", TimeSpan.FromMinutes(5));
        _sut.Remove("key2");

        _sut.Get<string>("key2").Should().BeNull();
    }

    // ── GetOrSetAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetOrSetAsync_OnCacheMiss_ShouldCallFactory()
    {
        var calls = 0;

        var result = await _sut.GetOrSetAsync("miss", () =>
        {
            calls++;
            return Task.FromResult("from-factory");
        }, TimeSpan.FromMinutes(5));

        result.Should().Be("from-factory");
        calls.Should().Be(1);
    }

    [Fact]
    public async Task GetOrSetAsync_OnCacheHit_ShouldNotCallFactory()
    {
        _sut.Set("hit", "cached-value", TimeSpan.FromMinutes(5));
        var calls = 0;

        var result = await _sut.GetOrSetAsync("hit", () =>
        {
            calls++;
            return Task.FromResult("new-value");
        }, TimeSpan.FromMinutes(5));

        result.Should().Be("cached-value");
        calls.Should().Be(0);
    }

    [Fact]
    public async Task GetOrSetAsync_CalledTwiceForSameKey_ShouldCallFactoryOnlyOnce()
    {
        var calls = 0;
        Func<Task<string>> factory = () => { calls++; return Task.FromResult("value"); };

        await _sut.GetOrSetAsync("once", factory, TimeSpan.FromMinutes(5));
        await _sut.GetOrSetAsync("once", factory, TimeSpan.FromMinutes(5));

        calls.Should().Be(1);
    }

    // ── RemoveByPrefix ────────────────────────────────────────────────────────

    [Fact]
    public void RemoveByPrefix_ShouldRemoveOnlyMatchingKeys()
    {
        _sut.Set("vendor:1",   "a", TimeSpan.FromMinutes(5));
        _sut.Set("vendor:2",   "b", TimeSpan.FromMinutes(5));
        _sut.Set("material:1", "c", TimeSpan.FromMinutes(5));

        _sut.RemoveByPrefix("vendor:");

        _sut.Get<string>("vendor:1").Should().BeNull();
        _sut.Get<string>("vendor:2").Should().BeNull();
        _sut.Get<string>("material:1").Should().Be("c");
    }

    [Fact]
    public void RemoveByPrefix_ShouldBeCaseInsensitive()
    {
        _sut.Set("Vendor:1", "v", TimeSpan.FromMinutes(5));

        _sut.RemoveByPrefix("vendor:");

        _sut.Get<string>("Vendor:1").Should().BeNull();
    }

    [Fact]
    public void RemoveByPrefix_WithNoMatchingKeys_ShouldNotThrow()
    {
        var act = () => _sut.RemoveByPrefix("nonexistent:");

        act.Should().NotThrow();
    }
}
