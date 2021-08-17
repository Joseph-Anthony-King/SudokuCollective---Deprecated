import * as axios from "axios";
import { requestHeader } from "@/helpers/requestHeader";
import { requestCheckGame } from "@/helpers/gameRequestData/gameRequestData";
import {
  getGamesEndpoint,
} from "./endpoints";

const getCreateGame = async function (difficultyLevel) {
  try {
    const config = {
      method: "get",
      url: `${getGamesEndpoint}/createAnnonymous?difficultyLevel=${difficultyLevel}`,
      headers: requestHeader(),
    };

    return await axios(config);
  } catch (error) {
    console.log(error.name, error.message);
    return error.response;
  }
};

const getCheckGame = async function (game) {
  try {
    const config = {
      method: "post",
      url: `${getGamesEndpoint}/checkAnnonymous`,
      headers: requestHeader(),
      data: requestCheckGame({"game": game}),
    };

    return await axios(config);
  } catch (error) {
    console.log(error);
    return error.response;
  }
};

export const gamesService = {
  getCreateGame,
  getCheckGame,
};