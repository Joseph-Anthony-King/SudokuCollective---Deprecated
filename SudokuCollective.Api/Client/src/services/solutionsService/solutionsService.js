import * as axios from "axios";
import { processError } from "@/helpers/commonFunctions/commonFunctions";
import { postSolveEndpoint } from "./endpoints";

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
    return processError(error);
  }
};

export const solutionsService = {
  postSolve,
};