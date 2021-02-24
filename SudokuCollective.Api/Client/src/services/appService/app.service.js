import * as axios from "axios";
import { requestHeader } from "../../helpers/requestHeader";
import { requestData } from "../../helpers/requestData";
import { getObtainAdminPrivilegesEnpoint } from "./service.endpoints";

const postObtainAdminPrivileges = async function (pageListModel) {
  try {
    const config = {
      method: "post",
      url: `${getObtainAdminPrivilegesEnpoint}`,
      headers: requestHeader(),
      data: requestData(pageListModel),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

export const appService = {
  postObtainAdminPrivileges,
};
