import store from "@/store";
import { requestData } from "@/helpers/requestData";

export function appUserRequestData(payload) {
  const data = {
    license: store.getters["settingsModule/getLicense"],
    requestorId: store.getters["settingsModule/getRequestorId"],
    appId: store.getters["settingsModule/getAppId"],
    pageListModel: payload.pageListModel
  };

  let result = requestData(data);

  result["targetLicense"] = payload.targetLicense;

  return result;
}