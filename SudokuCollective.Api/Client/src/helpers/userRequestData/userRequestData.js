import store from "@/store";
import { requestData } from "@/helpers/requestData";

export function requestDataUpdatePassword(
  pageListModel,
  oldPassword,
  newPassword
) {
  const data = {
    license: store.getters["settingsModule/getLicense"],
    requestorId: store.getters["settingsModule/getRequestorId"],
    appId: store.getters["settingsModule/getAppId"],
    pageListModel: pageListModel
  };

  let result = requestData(data);

  result["OldPassword"] = oldPassword;
  result["NewPassword"] = newPassword;

  return result;
}

export function requestDataUpdateUser(
  pageListModel,
  userName,
  firstName,
  lastName,
  nickName,
  email
) {
  const data = {
    license: store.getters["settingsModule/getLicense"],
    requestorId: store.getters["settingsModule/getRequestorId"],
    appId: store.getters["settingsModule/getAppId"],
    pageListModel: pageListModel
  };

  let result = requestData(data);

  result["UserName"] = userName;
  result["FirstName"] = firstName;
  result["LastName"] = lastName;
  result["NickName"] = nickName;
  result["Email"] = email;

  return result;
}

export function requestDataUpdateUserRoles(pageListModel, roleIds) {

  const data = {
    license: store.getters["settingsModule/getLicense"],
    requestorId: store.getters["settingsModule/getRequestorId"],
    appId: store.getters["settingsModule/getAppId"],
    pageListModel: pageListModel
  };

  let result = requestData(data);

  result["RoleIds"] = roleIds;

  return result;
}
