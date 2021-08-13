﻿import * as axios from "axios";
import store from "../../store";
import Paginator from "@/models/viewModels/paginator";
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
    console.log(config);

    return await axios(config);
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

    return await axios(config);
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const updateUser = async function (data) {
  try {
    const params = `/${data.id}`;

    const payload = {
      userName: data.userName,
      firstName: data.firstName,
      lastName: data.lastName,
      nickName: data.nickName,
      email: data.email,
      paginator: new Paginator(),
    };

    const config = {
      method: "put",
      url: `${getUserEnpoint}${params}`,
      headers: requestHeader(),
      data: requestDataUpdateUser(payload),
    };

    return await axios(config);
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

    return await axios(config);
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const putActivateUser = async function (id) {
  try {
    const params = `/${id}/activate`;

    const config = {
      method: "put",
      url: `${getUserEnpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    return await axios(config);
  } catch (error) {
    console.error(error.name, error.message);
    return error.response;
  }
};

const putDeactivateUser = async function (id) {
  try {
    const params = `/${id}/deactivate`;

    const config = {
      method: "put",
      url: `${getUserEnpoint}${params}`,
      headers: requestHeader(),
      data: requestData(),
    };

    return await axios(config);
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

    return await axios(config);
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

    return await axios(config);
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

    return await axios(config);
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

    return await axios(config);
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

    return await axios(config);
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
  putActivateUser,
  putDeactivateUser,
  postRequestPasswordReset,
  putResendPasswordReset,
  putCancelPasswordReset,
  putCancelEmailConfirmation,
  putCancelAllEmailRequests,
};
