import * as axios from "axios";
import store from "../../store";
import { processError } from "@/helpers/commonFunctions/commonFunctions";
import { requestHeader } from "../../helpers/requestHeader";
import { authenticateEndpoint, confirmUserNameEndpoint } from "./endpoints";

const authenticateUser = async function (username, password) {
  const headers = requestHeader();

  const config = {
    method: "post",
    url: authenticateEndpoint,
    headers: headers,
    data: {
      UserName: `${username}`,
      Password: `${password}`,
      License: `${store.getters["settingsModule/getLicense"]}`,
    },
  };

  try {
    return await axios(config);
  } catch (error) {
    return processError(error.response);
  }
};

const confirmUserName = async function (email, license) {
  const headers = requestHeader();
  const url = `${confirmUserNameEndpoint}`;

  const config = {
    method: "post",
    url: url,
    headers: headers,
    data: {
      email: email,
      license: license,
    },
  };

  try {
    return await axios(config);
  } catch (error) {
    return processError(error.response);
  }
};

export const authenticationService = {
  authenticateUser,
  confirmUserName,
};
