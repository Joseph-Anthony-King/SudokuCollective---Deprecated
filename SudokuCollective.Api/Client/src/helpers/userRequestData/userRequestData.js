import store from "@/store";
import { requestData } from "@/helpers/requestData";

export function requestDataUpdatePassword(payload) {
  const data = {
    license: store.getters["settingsModule/getLicense"],
    requestorId: store.getters["settingsModule/getRequestorId"],
    appId: store.getters["settingsModule/getAppId"],
    pageListModel: payload.pageListModel
  };

  let result = requestData(data);

  result["OldPassword"] = payload.oldPassword;
  result["NewPassword"] = payload.newPassword;

  return result;
}

export function requestDataUpdateUser(payload) {
  const data = {
    license: store.getters["settingsModule/getLicense"],
    requestorId: store.getters["settingsModule/getRequestorId"],
    appId: store.getters["settingsModule/getAppId"],
    pageListModel: payload.pageListModel
  };

  let result = requestData(data);

  result["UserName"] = payload.userName;
  result["FirstName"] = payload.firstName;
  result["LastName"] = payload.lastName;
  result["NickName"] = payload.nickName;
  result["Email"] = payload.email;

  return result;
}

export function requestDataUpdateUserRoles(payload) {

  const data = {
    license: store.getters["settingsModule/getLicense"],
    requestorId: store.getters["settingsModule/getRequestorId"],
    appId: store.getters["settingsModule/getAppId"],
    pageListModel: payload.pageListModel
  };

  let result = requestData(data);

  result["RoleIds"] = payload.roleIds;

  return result;
}
