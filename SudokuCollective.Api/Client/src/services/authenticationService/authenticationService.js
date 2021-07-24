﻿import * as axios from "axios";
import store from "../../store";
import { requestHeader } from "../../helpers/requestHeader";
import {
  authenticateEndpoint,
  confirmUserNameEndpoint,
} from "./endpoints";

const authenticateUser = async function (username, password) {
  const headers = requestHeader();

  const config = {
    method: "post",
    url: authenticateEndpoint,
    headers: headers,
    data: {
      UserName: `${username}`,
      Password: `${password}`,
      License: `${store.getters["settingsModule/getLicense"]}`
    },
  };

  try {
    const response = await axios(config);

    return response;
  } catch (error) {
    return error.response;
  }
};

const confirmUserName = async function (email) {
  const headers = requestHeader();
  const url = `${confirmUserNameEndpoint}/${email}`;

  const config = {
    method: "get",
    url: url,
    headers: headers,
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
