import * as axios from "axios";
import { requestHeader } from "../../helpers/requestHeader";
import { requestData } from "../../helpers/requestData";
import { getRegisterEndpoint, getResendEmailConfirmationEndpoint } from "./service.endpoints";

const postSignUp = async function (signUpModel) {

  try {
    const config = {
      method: "post",
      url: getRegisterEndpoint,
      headers: requestHeader(),
      data: signUpModel,
    };

    const response = await axios(config);

    return response;
  } catch (error) {
    return error.response;
  }
};

const putResendEmailConfirmation = async function () {

  try {
    const config = {
      method: "put",
      url: getResendEmailConfirmationEndpoint,
      headers: requestHeader(),
      data: requestData(),
    };

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
