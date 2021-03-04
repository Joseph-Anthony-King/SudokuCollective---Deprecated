import store from "@/store";
import { requestData } from "@/helpers/requestData";

export function requestDataUpdateApp(payload) {
  const data = {
    license: store.getters["settingsModule/getLicense"],
    requestorId: store.getters["settingsModule/getRequestorId"],
    appId: store.getters["settingsModule/getAppId"],
    pageListModel: payload.pageListModel
  };

  let result = requestData(data);

  result["name"] = payload.name;
  result["devUrl"] = payload.devUrl;
  result["liveUrl"] = payload.liveUrl;
  result["isActive"] = payload.isActive;
  result["inDevelopment"] = payload.inDevelopment;
  result["permitSuperUserAccess"] = payload.permitSuperUserAccess;
  result["permitCollectiveLogins"] = payload.permitCollectiveLogins;
  result["disableCustomUrls"] = payload.disableCustomUrls;
  result["customEmailConfirmationDevUrl"] = payload.customEmailConfirmationDevUrl;
  result["customEmailConfirmationLiveUrl"] = payload.customEmailConfirmationLiveUrl;
  result["customPasswordResetDevUrl"] = payload.customPasswordResetDevUrl;
  result["customPasswordResetLiveUrl"] = payload.customPasswordResetLiveUrl;
  result["timeFrame"] = payload.timeFrame;
  result["accessDuration"] = payload.accessDuration;

  return result;
}