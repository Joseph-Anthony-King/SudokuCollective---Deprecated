import store from "@/store";
import Paginator from "@/models/viewModels/paginator";

export function requestData(data) {
  if (data === undefined) {
    data = {
      license: store.getters["settingsModule/getLicense"],
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      paginator: new Paginator(),
    };
  }

  return {
    License: data.license,
    RequestorId: data.requestorId,
    AppId: data.appId,
    Paginator: data.paginator,
  };
}
