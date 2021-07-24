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
} from "./endpoints";

const getUser = async function (id) {
  try {
    let params = `/${id}`;

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

const getUsers = async function (data) {
  try {
    const config = {
      method: "post",
      url: `${getUserEnpoint}`,
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
  paginator
) {
  try {
    const params = `/${id}`;

    const payload = {
      paginator: paginator,
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

const deleteUser = async function (id) {
  try {
    const params = `/${id}`;

    const config = {
      method: "delete",
      url: `${getUserEnpoint}${params}`,
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

export const userService = {
  getUser,
  getUsers,
  updateUser,
  deleteUser,
  postRequestPasswordReset,
  putResendPasswordReset,
  putCancelPasswordReset,
  putCancelEmailConfirmation,
  putCancelAllEmailRequests,
};
