import { processFailure } from "@/helpers/commonFunctions/commonFunctions";
import { gamesService } from "@/services/gamesService/gamesService";

const createGame = async function (difficultyLevel) {
  const response = await gamesService.getCreateGame(difficultyLevel);

  if (response.data.success) {
    let game = [];
    response.data.sudokuMatrix.forEach((row) => {
      let result = [];
      row.forEach((cell) => {
        if (cell === 0) {
          result.push("");
        } else {
          result.push(String(cell));
        }
      });
      game = game.concat(result);
    });
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      game: game,
    };
  } else {
    return processFailure(response);
  }
};

const checkGame = async function (game) {
  const response = await gamesService.getCheckGame(game);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

export const gamesProvider = {
  checkGame,
  createGame,
};
