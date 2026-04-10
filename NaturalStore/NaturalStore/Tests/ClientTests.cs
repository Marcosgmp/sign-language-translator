using Domain.Entities;
using Xunit;

namespace Tests;

public class ClientTests
{
    [Fact]
    public void CreateWalkInClient_ShouldNotBeOnline()
    {
        var c = new Client("Maria");
        Assert.False(c.IsOnline);
        Assert.Null(c.Login);
    }

    [Fact]
    public void CreateRegisteredClient_ShouldBeOnline()
    {
        var c = new Client("João", "joao@email.com", "joao", "hash123");
        Assert.True(c.IsOnline);
        Assert.Equal("joao", c.Login);
    }

    [Fact]
    public void CheckPassword_CorrectHash_ShouldReturnTrue()
    {
        var c = new Client("João", "joao@email.com", "joao", "myhash");
        Assert.True(c.CheckPassword("myhash"));
    }

    [Fact]
    public void CheckPassword_WrongHash_ShouldReturnFalse()
    {
        var c = new Client("João", "joao@email.com", "joao", "myhash");
        Assert.False(c.CheckPassword("wronghash"));
    }

    [Fact]
    public void CreateClient_EmptyName_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new Client(""));
    }
}
