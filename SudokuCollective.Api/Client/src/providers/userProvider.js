import { userService } from "@/services/userService/userService";
import User from "@/models/user"

const getUser = async function (id) {
  var response = await userService.getUser(id);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      user: new User(response.data.user)
    }
  } else {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17)
    }
  }
}

const updateUser = async function (data) {
  var response = await userService.updateUser(data);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      user: new User(response.data.user)
    }
  } else {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17)
    }
  }
}

const deleteUser = async function (id) {
  var response = await userService.deleteUser(id);

  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17)
  }
}

const requestPasswordReset = async function (email) {
  const response = await userService.postRequestPasswordReset(email);

  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17)
  }
}

const resendPasswordReset = async function () {
  const response = await userService.putResendPasswordReset();

  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17)
  }
}

const cancelPasswordReset = async function () {
  const response = await userService.putCancelPasswordReset();

  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17)
  }
}

const cancelEmailConfirmation = async function () {
  const response = await userService.putCancelEmailConfirmation();

  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17)
  }
}

const cancelAllEmailRequests = async function () {
  const response = await userService.putCancelAllEmailRequests();

  return {
    status: response.status,
    success: response.data.success,
    message: response.data.message.substring(17)
  }
}

export const userProvider = {
  getUser,
  updateUser,
  deleteUser,
  requestPasswordReset,
  resendPasswordReset,
  cancelPasswordReset,
  cancelEmailConfirmation,
  cancelAllEmailRequests
}