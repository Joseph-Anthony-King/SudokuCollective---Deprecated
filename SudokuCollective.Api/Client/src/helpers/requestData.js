import store from "@/store";
import PageListModel from "@/models/viewModels/pageListModel"

export function requestData(data) {

  if (data === undefined) {
    data = {
      license: store.getters["settingsModule/getLicense"],
      requestorId: store.getters["settingsModule/getRequestorId"],
      appId: store.getters["settingsModule/getAppId"],
      pageListModel: new PageListModel()
    }
  }

  return {
    License: data.license,
    RequestorId: data.requestorId,
    AppId: data.appId,
    PageListModel: data.pageListModel,
  };
}
