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

    return await axios(config);
  } catch (error) {
    return processError(error.response);
  }
};

export const solutionsService = {
  postSolve,
};
