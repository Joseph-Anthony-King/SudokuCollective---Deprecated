import { processFailure } from "@/helpers/commonFunctions/commonFunctions";
import { solutionsService } from "@/services/solutionsService/solutionsService";

const solve = async function (solveModel) {
  const response = await solutionsService.postSolve(solveModel);

  if (response.data.success) {
    let matrix = [];
    for (var j = 0; j < 81; j++) {
      matrix[j] = response.data.solution.solutionList[j].toString();
    }
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      matrix: matrix,
    };
  } else {
    return processFailure(response);
  }
};

export const solutionsProvider = {
  solve,
};
