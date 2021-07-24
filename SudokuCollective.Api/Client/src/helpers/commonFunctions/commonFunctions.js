import { userService } from "@/services/userService/userService";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export async function passwordReset(userEmail, component) {
  try {
    const response = await userService.postRequestPasswordReset(userEmail);

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
        response.data.message.substring(17),
        defaultToastOptions()
      );

      return false;
    }
  } catch (error) {
    showToast(component, ToastMethods["error"], error, defaultToastOptions());

    return false;
  }
}

export function convertStringToDateTime(datetime) {
  return new Date(datetime).toLocaleString();
}
