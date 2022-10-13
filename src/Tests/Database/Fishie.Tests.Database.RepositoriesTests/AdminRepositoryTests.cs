namespace Fishie.Tests.Database.RepositoriesTests;

public class AdminRepositoryTests
{
    private readonly AdminRepository _adminRepository;

    public AdminRepositoryTests()
    {
        _adminRepository = new AdminRepository(new NpgSqlContext(new DbContextOptionsBuilder<NpgSqlContext>()
            .UseInMemoryDatabase("FishieDBTest")
            .Options));
    }

    [Fact]
    public async Task ExistsFalseAsync()
    {
        // Arrange & Act
        var result = await _adminRepository.ExistsAsync(9999);
        
        // Assert
        Assert.True(!result);
    }

    [Fact]
    public async Task AddTestsAsync()
    {
        // Arrange
        var admin = new Admin(1, "Gena", "Li", "DeiLux");

        // Act
        await _adminRepository.AddAsync(admin);
        var result = await _adminRepository.ExistsAsync(admin.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUsernameAsync()
    {
        // Arrange
        var admin = new Admin(2, "Gena", "Li", "DeiLux1");
        await _adminRepository.AddAsync(admin);

        // Act
        var resultTrue = await _adminRepository.ExistsAsync(admin.Id);
        await _adminRepository.RemoveAsync(admin.Username!);
        var resultFalse = await _adminRepository.ExistsAsync(admin.Id);

        // Assert
        Assert.True(resultTrue);
        Assert.True(!resultFalse);
    }

    [Fact]
    public async Task FindAsync()
    {
        // Arrange
        var admin = new Admin(3, "Gena", "Li", "DeiLux3");
        await _adminRepository.AddAsync(admin);

        // Act
        var result = await _adminRepository.FindAsync(admin.Username!);

        // Assert
        Assert.Equal(admin.Id, result!.Id);
        Assert.Equal(admin.FirstName, result!.FirstName);
        Assert.Equal(admin.LastName, result!.LastName);
        Assert.Equal(admin.Username, result!.Username);
    }

    [Fact]
    public async Task FindAllAsync()
    {
        // Arrange
        var admin = new Admin(4, "Gena", "Li", "DeiLux4");
        await _adminRepository.AddAsync(admin);

        // Act
        var result = await _adminRepository.FindAllAsync();

        // Assert
        Assert.NotNull(result);
    }
}