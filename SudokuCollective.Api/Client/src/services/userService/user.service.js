import * as axios from "axios";
import store from "../../store";
import User from "../../models/user";
import { requestHeader } from "../../helpers/requestHeader";
import { requestData } from "../../helpers/requestData";
import { requestDataUpdateUser } from "../../helpers/userRequestData/userRequestData";
import {
  getUserEnpoint,
  getRequestPasswordResetEnpoint,
  getResendRequestPasswordResetEndpoint,
  getCancelPasswordResetEndpoint,
  getCancelEmailConfirmationEndpoint,
  getCancelAllEmailRequestsEndpoint,
} from "./service.endpoints";

const getUser = async function (id, fullRecord) {
  try {
    let params = "";

    if (fullRecord) {
      params = `/${id}?fullrecord=${fullRecord}`;
    } else {
      params = `/${id}`;
    }

    const config = {
      method: "post",
      url: `${getUserEnpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    const user = new User(response.data.user);

    return user;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const getUsers = async function (data, fullRecord) {
  try {
    let params = "";

    if (fullRecord) {
      params = `?fullrecord=${fullRecord}`;
    }

    const config = {
      method: "post",
      url: `${getUserEnpoint}${params}`,
      headers: requestHeader(),
      data: requestData(data),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const updateUser = async function (
  id,
  userName,
  firstName,
  lastName,
  nickName,
  email,
  pageListModel
) {
  try {
    const params = `/${id}`;

    const payload = {
      pageListModel: pageListModel,
      userName: userName,
      firstName: firstName,
      lastName: lastName,
      nickName: nickName,
      email: email
    }

    const config = {
      method: "put",
      url: `${getUserEnpoint}${params}`,
      headers: requestHeader(),
      data: requestDataUpdateUser(payload),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    return error.response;
  }
};

const postRequestPasswordReset = async function (email) {
  try {
    const license = store.getters["settingsModule/getLicense"];

    const config = {
      method: "post",
      url: `${getRequestPasswordResetEnpoint}`,
      headers: requestHeader(),
      data: {
        License: license,
        Email: email,
      },
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const putResendPasswordReset = async function () {
  try {
    const config = {
      method: "put",
      url: `${getResendRequestPasswordResetEndpoint}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const putCancelPasswordReset = async function () {
  try {
    const config = {
      method: "put",
      url: `${getCancelPasswordResetEndpoint}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const putCancelEmailConfirmation = async function () {
  try {
    const config = {
      method: "put",
      url: `${getCancelEmailConfirmationEndpoint}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const putCancelAllEmailRequests = async function () {
  try {
    const config = {
      method: "put",
      url: `${getCancelAllEmailRequestsEndpoint}`,
      headers: requestHeader(),
      data: requestData(),
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const loginUser = function (user, token) {
  user.login(token);
  return user;
};

const logoutUser = function (user) {
  user.logout();
  return user;
};

export const userService = {
  getUser,
  getUsers,
  updateUser,
  postRequestPasswordReset,
  putResendPasswordReset,
  putCancelPasswordReset,
  putCancelEmailConfirmation,
  putCancelAllEmailRequests,
  loginUser,
  logoutUser,
};
