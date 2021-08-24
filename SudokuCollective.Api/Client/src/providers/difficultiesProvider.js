import { processFailure } from "@/helpers/commonFunctions/commonFunctions";
import { difficultiesService } from "@/services/difficultiesService/difficultiesService";
import Difficulty from "@/models/difficulty";

const getDifficulties = async function () {
  const response = await difficultiesService.getDifficulties();

  if (response.data.isSuccess) {
    let difficulties = [];

    response.data.difficulties.forEach((difficulty) => {
      difficulties.push(new Difficulty(difficulty));
    });
    return {
      status: response.status,
      isSuccess: response.data.isSuccess,
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
