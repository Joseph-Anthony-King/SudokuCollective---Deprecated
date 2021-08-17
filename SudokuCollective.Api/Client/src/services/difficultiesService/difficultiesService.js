import * as axios from "axios";
import { requestHeader } from "@/helpers/requestHeader";
import {
  getDifficultiesEndpoint,
} from "./endpoints";

const getDifficulties = async function () {
  try {
    const config = {
      method: "get",
      url: `${getDifficultiesEndpoint}`,
      headers: requestHeader(),
    };

    return await axios(config);
  } catch (error) {
    console.log(error.name, error.message);
    return error.response;
  }
};

export const difficultiesService = {
  getDifficulties,
};