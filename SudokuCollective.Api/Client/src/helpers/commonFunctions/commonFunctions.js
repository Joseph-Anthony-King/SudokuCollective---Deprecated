import _ from "lodash";
import store from "@/store";
import { userProvider } from "@/providers/userProvider";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export async function passwordReset(userEmail, component) {
  try {
    const response = await userProvider.requestPasswordReset(userEmail);

    if (response.status === 200) {
      showToast(
        component,
        ToastMethods["success"],
        `Please review ${userEmail} to reset your password`,
        defaultToastOptions()
      );

      return true;
    } else {
      showToast(
        component,
        ToastMethods["error"],
        response.message,
        defaultToastOptions()
      );

      return false;
    }
  } catch (error) {
    showToast(component, ToastMethods["error"], error, defaultToastOptions());

    return false;
  }
}

export function processError(error) {
  let status = 0;
  let message = "";

  if (error.message === "Request failed with status code 401") {
    status = 401;
    store.dispatch("settingsModule/expireAuthToken");
    message = "Authorization has expired";
  } else {
    message = error.message;
  }

  const result = {
    status: status,
    error: true,
    data: {
      success: false,
      message: message,
    },
  };

  return result;
}

export function processFailure(response) {
  if (response.error) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message,
    };
  } else {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  }
}

export function displayDateUpdated(dateUpdated) {
  const invalidDates = ["12/31/1, 4:07:02 PM", "12/31/2001, 4:07:02 PM"];

  if (!_.includes(invalidDates, dateUpdated)) {
    return true;
  } else {
    return false;
  }
}
