import * as axios from "axios";
import { requestHeader } from "../../helpers/requestHeader";
import { requestData } from "../../helpers/requestData";
import { getRegisterEndpoint, getResendEmailConfirmationEndpoint } from "./service.endpoints";

const postSignUp = async function (signUpModel) {

  const config = {
    method: "post",
    url: getRegisterEndpoint,
    headers: requestHeader(),
    data: signUpModel,
  };

  try {
    const response = await axios(config);

    return response;
  } catch (error) {
    return error.response;
  }
};

const putResendEmailConfirmation = async function (pageListModel) {

  const config = {
    method: "put",
    url: getResendEmailConfirmationEndpoint,
    headers: requestHeader(),
    data: requestData(pageListModel),
  };

  try {
    const response = await axios(config);

    return response;
  } catch (error) {
    return error.response;
  }
};

export const registerService = {
  postSignUp,
  putResendEmailConfirmation,
};
