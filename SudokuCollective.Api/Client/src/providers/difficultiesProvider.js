import { processFailure } from "@/helpers/commonFunctions/commonFunctions";
import { difficultiesService } from "@/services/difficultiesService/difficultiesService";
import Difficulty from "@/models/difficulty";

const getDifficulties = async function () {
  const response = await difficultiesService.getDifficulties();

  if (response.data.success) {
    let difficulties = [];

    response.data.difficulties.forEach((difficulty) => {
      difficulties.push(new Difficulty(difficulty));
    });
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      difficulties: difficulties,
    };
  } else {
    return processFailure(response);
  }
};

export const difficultiesProvider = {
  getDifficulties,
};
