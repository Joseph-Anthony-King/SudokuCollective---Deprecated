import { userService } from "@/services/userService/user.service";
import { ToastMethods } from "@/models/arrays/toastMethods";
import { showToast, defaultToastOptions } from "@/helpers/toastHelper";

export async function passwordReset(userEmail, component) {
    try {
      const response = await userService.getRequestPasswordReset(
        userEmail
      );

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