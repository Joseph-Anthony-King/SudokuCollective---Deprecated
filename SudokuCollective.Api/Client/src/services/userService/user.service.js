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

    let user = new User();

    const response = await axios(config);

    user = assignAPIReponseToUser(response.data.user);

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

    const config = {
      method: "put",
      url: `${getUserEnpoint}${params}`,
      headers: requestHeader(),
      data: requestDataUpdateUser(
        pageListModel,
        userName,
        firstName,
        lastName,
        nickName,
        email
      ),
    };

    let user = new User();

    const response = await axios(config);

    user = assignAPIReponseToUser(response.data.user);
    store.dispatch("settingsModule/updateUser", user);

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

const putResendPasswordReset = async function (pageListModel) {
  try {
    const config = {
      method: "put",
      url: `${getResendRequestPasswordResetEndpoint}`,
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

const putCancelPasswordReset = async function (pageListModel) {
  try {
    const config = {
      method: "put",
      url: `${getCancelPasswordResetEndpoint}`,
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

const putCancelEmailConfirmation = async function (pageListModel) {
  try {
    const config = {
      method: "put",
      url: `${getCancelEmailConfirmationEndpoint}`,
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

const putCancelAllEmailRequests = async function (pageListModel) {
  try {
    const config = {
      method: "put",
      url: `${getCancelAllEmailRequestsEndpoint}`,
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

const loginUser = function (user, token) {
  user.login();

  store.dispatch("settingsModule/updateUser", user);
  store.dispatch("settingsModule/updateAuthToken", token);

  return user;
};

const logoutUser = function (user) {
  user.logout();

  user = new User();

  store.dispatch("settingsModule/updateUser", user);
  store.dispatch("settingsModule/updateAuthToken", "");

  return user;
};

const assignAPIReponseToUser = function (data) {
  let user = store.getters["settingsModule/getUser"];

  user.id = data.id;
  user.userName = data.userName;
  user.firstName = data.firstName;
  user.lastName = data.lastName;
  user.nickName = data.nickName;
  user.fullName = data.fullName;
  user.email = data.email;
  user.emailConfirmed = data.emailConfirmed;
  user.receivedRequestToUpdateEmail = data.receivedRequestToUpdateEmail;
  user.receivedRequestToUpdatePassword = data.receivedRequestToUpdatePassword;
  user.isActive = data.isActive;
  user.isAdmin = data.isAdmin;
  user.isSuperUser = data.isSuperUser;
  user.dateCreated = data.dateCreated;
  user.dateUpdated = data.dateUpdated;

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
  assignAPIReponseToUser,
};
