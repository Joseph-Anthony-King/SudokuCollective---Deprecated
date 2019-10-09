using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Domain;
using SudokuCollective.WebApi.Models;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.Enums;

namespace SudokuCollective.WebApi.Services {

    internal static class GamesServiceUtilities {

        internal static async Task<List<Game>> RetrieveGames(
            PageListModel pageListModel, 
            ApplicationDbContext context,
            int userId = 0) {

            var result = new List<Game>();

            if (pageListModel == null) {

                if (pageListModel.IncludeCompletedGames) {

                    if (userId == 0) {

                        result = await context.Games
                            .OrderBy(g => g.Id)
                            .Include(g => g.User).ThenInclude(u => u.Roles)
                            .Include(g => g.SudokuMatrix)
                            .Include(g => g.SudokuSolution)
                            .ToListAsync();

                    } else {

                        result = await context.Games
                            .OrderBy(g => g.Id)
                            .Include(g => g.User).ThenInclude(u => u.Roles)
                            .Include(g => g.SudokuMatrix)
                            .Include(g => g.SudokuSolution)
                            .Where(g => g.User.Id == userId)
                            .ToListAsync();
                    }

                } else {

                    if (userId == 0) {

                        result = await context.Games
                            .OrderBy(g => g.Id)
                            .Include(g => g.User).ThenInclude(u => u.Roles)
                            .Include(g => g.SudokuMatrix)
                            .Include(g => g.SudokuSolution)
                            .Where(g => g.ContinueGame == true)
                            .ToListAsync();

                    } else {

                        result = await context.Games
                            .OrderBy(g => g.Id)
                            .Include(g => g.User).ThenInclude(u => u.Roles)
                            .Include(g => g.SudokuMatrix)
                            .Include(g => g.SudokuSolution)
                            .Where(g => g.ContinueGame == true && g.User.Id == userId)
                            .ToListAsync();
                    }
                }

            } else if (pageListModel.SortBy == SortValue.ID) {

                if (pageListModel.OrderByDescending) {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.Id)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.Id)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.Id)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.Id)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }

                } else {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.Id)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.Id)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.Id)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.Id)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }
                }

            } else if (pageListModel.SortBy == SortValue.USERNAME) {

                if (pageListModel.OrderByDescending) {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.User.UserName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.User.UserName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.User.UserName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.User.UserName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }

                } else {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.User.UserName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.User.UserName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.User.UserName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.User.UserName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }
                }

            } else if (pageListModel.SortBy == SortValue.FIRSTNAME) {

                if (pageListModel.OrderByDescending) {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.User.FirstName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.User.FirstName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.User.FirstName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.User.FirstName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }

                } else {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.User.FirstName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.User.FirstName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.User.FirstName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.User.FirstName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }
                }

            } else if (pageListModel.SortBy == SortValue.LASTNAME) {

                if (pageListModel.OrderByDescending) {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.User.LastName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.User.LastName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.User.LastName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.User.LastName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }

                } else {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.User.LastName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.User.LastName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.User.LastName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.User.LastName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }
                }

            } else if (pageListModel.SortBy == SortValue.FULLNAME) {

                if (pageListModel.OrderByDescending) {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.User.FullName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.User.FullName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.User.FullName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.User.FullName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }

                } else {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.User.FullName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.User.FullName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.User.FullName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.User.FullName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }
                }

            } else if (pageListModel.SortBy == SortValue.NICKNAME) {

                if (pageListModel.OrderByDescending) {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.User.NickName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.User.NickName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.User.NickName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.User.NickName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }

                } else {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.User.NickName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.User.NickName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.User.NickName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.User.NickName)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }
                }

            } else {

                if (pageListModel.OrderByDescending) {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.DateCreated)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.DateCreated)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderByDescending(g => g.DateCreated)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderByDescending(g => g.DateCreated)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }

                } else {

                    if (pageListModel.IncludeCompletedGames) {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.DateCreated)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.DateCreated)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }

                    } else {

                        if (userId == 0) {

                            result = await context.Games
                                .OrderBy(g => g.DateCreated)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();

                        } else {

                            result = await context.Games
                                .OrderBy(g => g.DateCreated)
                                .Include(g => g.User).ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.ContinueGame == true && g.User.Id == userId)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage)
                                .ToListAsync();
                        }
                    }
                }
            }

            return result;
        }
    }
}
