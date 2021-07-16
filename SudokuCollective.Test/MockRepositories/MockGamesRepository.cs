using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Test.MockRepositories
{
    public class MockGamesRepository
    {
        private readonly DatabaseContext context;
        internal Mock<IGamesRepository<Game>> GamesRepositorySuccessfulRequest { get; set; }
        internal Mock<IGamesRepository<Game>> GamesRepositoryFailedRequest { get; set; }
        internal Mock<IGamesRepository<Game>> GamesRepositoryUpdateFailedRequest { get; set; }

        public MockGamesRepository(DatabaseContext ctxt)
        {
            context = ctxt;
            var todaysDate = DateTime.UtcNow;

            GamesRepositorySuccessfulRequest = new Mock<IGamesRepository<Game>>();
            GamesRepositoryFailedRequest = new Mock<IGamesRepository<Game>>();
            GamesRepositoryUpdateFailedRequest = new Mock<IGamesRepository<Game>>();

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.Add(It.IsAny<Game>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Object = new Game(
                            context.Users.FirstOrDefault(u => u.Id == 1),
                            new SudokuMatrix(),
                            context.Difficulties.FirstOrDefault(d => d.DifficultyLevel == DifficultyLevel.TEST),
                            context.Apps.Where(a => a.Id == 1).Select(a => a.Id).FirstOrDefault())
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.Get(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Object = context.Games.FirstOrDefault(g => g.Id == 1)
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.GetAll())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Games.ToList().ConvertAll(g => (IEntityBase)g)
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.Update(It.IsAny<Game>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Games.FirstOrDefault(g => g.Id == 1)
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.UpdateRange(It.IsAny<List<Game>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    { 
                        Success = true,
                        Objects = context.Games.ToList().ConvertAll(g => (IEntityBase)g)
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.Delete(It.IsAny<Game>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.DeleteRange(It.IsAny<List<Game>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.GetAppGame(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Games.FirstOrDefault(g => g.Id == 1)
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.GetAppGames(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Games.ToList().ConvertAll(g => (IEntityBase)g)
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Games.FirstOrDefault(g => g.Id == 1)
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.GetMyGames(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Games.ToList().ConvertAll(g => (IEntityBase)g)
                    } as IRepositoryResponse));

            GamesRepositorySuccessfulRequest.Setup(gamesRepo =>
                gamesRepo.DeleteMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.Add(It.IsAny<Game>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.Get(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.GetAll())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.Update(It.IsAny<Game>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.UpdateRange(It.IsAny<List<Game>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.Delete(It.IsAny<Game>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.DeleteRange(It.IsAny<List<Game>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.GetAppGame(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.GetAppGames(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.GetMyGames(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryFailedRequest.Setup(gamesRepo =>
                gamesRepo.DeleteMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.Add(It.IsAny<Game>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = new Game(
                            context.Users.FirstOrDefault(u => u.Id == 1),
                            new SudokuMatrix(),
                            context.Difficulties.FirstOrDefault(d => d.DifficultyLevel == DifficultyLevel.TEST),
                            context.Apps.Where(a => a.Id == 1).Select(a => a.Id).FirstOrDefault())
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.Get(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Games.FirstOrDefault(g => g.Id == 1)
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.GetAll())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Games.ToList().ConvertAll(g => (IEntityBase)g)
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.Update(It.IsAny<Game>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.UpdateRange(It.IsAny<List<Game>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Games.ToList().ConvertAll(g => (IEntityBase)g)
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.Delete(It.IsAny<Game>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.DeleteRange(It.IsAny<List<Game>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.GetAppGame(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Games.FirstOrDefault(g => g.Id == 1)
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.GetAppGames(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Games.ToList().ConvertAll(g => (IEntityBase)g)
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.GetMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Games.FirstOrDefault(g => g.Id == 1)
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.GetMyGames(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Games.ToList().ConvertAll(g => (IEntityBase)g)
                    } as IRepositoryResponse));

            GamesRepositoryUpdateFailedRequest.Setup(gamesRepo =>
                gamesRepo.DeleteMyGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));
        }
    }
}
