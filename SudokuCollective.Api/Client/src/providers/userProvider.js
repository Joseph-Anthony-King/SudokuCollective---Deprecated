import { processFailure } from "@/helpers/commonFunctions/commonFunctions";
import { userService } from "@/services/userService/userService";
import User from "@/models/user";

const getUser = async function (id) {
  var response = await userService.getUser(id);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      user: new User(response.data.user),
    };
  } else {
    return processFailure(response);
  }
};

const getUsers = async function () {
  var response = await userService.getUsers();

  if (response.data.success) {
    response.data.users = response.data.users.sort(function (a, b) {
      if (a.id < b.id) {
        return -1;
      }
      if (a.id > b.id) {
        return 1;
      }
      return 0;
    });

    let users = [];

    if (response.data.users.length > 0) {
      response.data.users.forEach((user) => {
        users.push(new User(user));
      });
    }

    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      users: users,
    };
  } else {
    return processFailure(response);
  }
};

const updateUser = async function (data) {
  var response = await userService.updateUser(data);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      user: new User(response.data.user),
    };
  } else {
    return processFailure(response);
  }
};

const deleteUser = async function (id) {
  var response = await userService.deleteUser(id);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const activateUser = async function (id) {
  var response = await userService.putActivateUser(id);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const deactivateUser = async function (id) {
  var response = await userService.putDeactivateUser(id);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const confirmEmail = async function (token) {
  var response = await userService.getConfirmEmail(token);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
      email: response.data.email,
      dateUpdated: response.data.dateUpdated,
      isUpdate: response.data.isUpdate,
      newEmailAddressConfirmed: response.data.newEmailAddressConfirmed,
      confirmationEmailSuccessfullySent:
        response.data.confirmationEmailSuccessfullySent,
    };
  } else {
    return processFailure(response);
  }
};

const resetPassword = async function (data) {
  var response = await userService.putResetPassword(data);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const requestPasswordReset = async function (email) {
  const response = await userService.postRequestPasswordReset(email);

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const resendPasswordReset = async function () {
  const response = await userService.putResendPasswordReset();

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const cancelPasswordReset = async function () {
  const response = await userService.putCancelPasswordReset();

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const cancelEmailConfirmation = async function () {
  const response = await userService.putCancelEmailConfirmation();

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

const cancelAllEmailRequests = async function () {
  const response = await userService.putCancelAllEmailRequests();

  if (response.data.success) {
    return {
      status: response.status,
      success: response.data.success,
      message: response.data.message.substring(17),
    };
  } else {
    return processFailure(response);
  }
};

export const userProvider = {
  getUser,
  getUsers,
  updateUser,
  deleteUser,
  activateUser,
  deactivateUser,
  confirmEmail,
  resetPassword,
  requestPasswordReset,
  resendPasswordReset,
  cancelPasswordReset,
  cancelEmailConfirmation,
  cancelAllEmailRequests,
};
