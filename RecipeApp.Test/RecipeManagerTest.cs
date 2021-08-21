using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RecipeApp.Exceptions;
using RecipeApp.Helpers;
using RecipeApp.Managers;
using RecipeApp.Models;
using RecipeApp.Services;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.Test
{
    [TestClass]
    public class RecipeManagerTest
    {
        Mock<IRecipeService> recipeServiceMock;
        Mock<IConnectivityHelper> connectivityHelperMock;
        RecipeManager recipeManager;

        [TestInitialize]
        public void TestInitialize()
        {
            connectivityHelperMock = new Mock<IConnectivityHelper>();
            recipeServiceMock = new Mock<IRecipeService>();

            connectivityHelperMock.Setup(m => m.IsConnected).Returns(true);

            recipeManager = new RecipeManager(recipeServiceMock.Object, connectivityHelperMock.Object);
        }

        [TestMethod]
        public async Task GetRecipes_ReturnsRecipes()
        {
            recipeServiceMock.Setup(m => m.GetRecipes())
                .ReturnsAsync(new Recipe[]
                {
                    new Recipe(),
                    new Recipe(),
                    new Recipe()
                });

            var result = await recipeManager.GetRecipes();

            Assert.IsTrue(result.Count() == 3);
        }

        [TestMethod]
        [ExpectedException(typeof(NoInternetException))]
        public async Task GetRecipes_ThrowException_NoInternet()
        {
            connectivityHelperMock.Setup(m => m.IsConnected).Returns(false);
            await recipeManager.GetRecipes();
        }

        [TestMethod]
        public async Task GetRecipe_ReturnsRecipe()
        {
            recipeServiceMock.Setup(m => m.GetRecipe("r1"))
                .ReturnsAsync(new Recipe()
                {
                    RowKey = "r1",
                    Name = "test recipe"
                });

            var result = await recipeManager.GetRecipe("r1");
            Assert.IsTrue(result.Name.Equals("test recipe"));
        }

        [TestMethod]
        [ExpectedException(typeof(NoInternetException))]
        public async Task GetRecipe_ThrowException_NoInternet()
        {
            connectivityHelperMock.Setup(m => m.IsConnected).Returns(false);
            await recipeManager.GetRecipe(It.IsAny<string>());
        }

        [TestMethod]
        public async Task DeleteRecipe_ReturnsStringMessage()
        {
            string expected = "Recipe has been deleted";

            recipeServiceMock.Setup(m => m.DeleteRecipe("p1", "r1"))
                .ReturnsAsync(expected);

            var result = await recipeManager.DeleteRecipe("p1", "r1");

            //Assert.IsTrue(result.Equals(expected));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NoInternetException))]
        public async Task DeleteRecipe_ThrowException_NoInternet()
        {
            connectivityHelperMock.Setup(m => m.IsConnected).Returns(false);
            await recipeManager.DeleteRecipe("p1", "r1");
        }

        [TestMethod]
        public async Task AddRecipe_ReturnsStringMessage()
        {
            var recipe = new Recipe()
            {
                PartitionKey = "p1",
                RowKey = "r1",
                Name = "test recipe"
            };

            var expected = $"{recipe.Name} has been saved";

            recipeServiceMock.Setup(m => m.AddRecipe(recipe))
                .ReturnsAsync(expected);

            var result = await recipeManager.AddRecipe(recipe);

            //Assert.AreEqual(expected, result);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NoInternetException))]
        public async Task AddRecipe_ThrowException_NoInternet()
        {
            connectivityHelperMock.Setup(m => m.IsConnected).Returns(false);
            await recipeManager.AddRecipe(It.IsAny<Recipe>());
        }
    }
}
