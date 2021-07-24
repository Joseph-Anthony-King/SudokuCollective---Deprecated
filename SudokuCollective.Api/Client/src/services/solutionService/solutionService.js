import * as axios from "axios";
import { postSolveEndpoint, } from "./endpoints";

const postSolve = async function (solveModel) {

  try {
    const config = {
      method: "post",
      url: postSolveEndpoint,
      data: solveModel,
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    return error.response;
  }
};

export const solutionService = {
  postSolve
};
