import * as axios from "axios";
import { processError } from "@/helpers/commonFunctions/commonFunctions";
import { requestHeader } from "@/helpers/requestHeader";
import { getDifficultiesEndpoint } from "./endpoints";

const getDifficulties = async function () {
  try {
    const config = {
      method: "get",
      url: `${getDifficultiesEndpoint}`,
      headers: requestHeader(),
    };

    return await axios(config);
  } catch (error) {
    return processError(error);
  }
};

export const difficultiesService = {
  getDifficulties,
};
