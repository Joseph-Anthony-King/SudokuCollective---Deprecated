import * as axios from "axios";
import store from "../../store";
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
    const response = await axios(config);

    return response;
  } catch (error) {
    return error.response;
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
    const response = await axios(config);

    return response;
  } catch (error) {
    return error.response;
  }
};

export const authenticationService = {
  authenticateUser,
  confirmUserName,
};
