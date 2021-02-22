import store from "../store";

export function requestData(pageListModel) {
  const license = process.env.VUE_APP_LICENSE;
  const requestorId = parseInt(
    store.getters["appSettingsModule/getRequestorId"]
  );
  const appId = parseInt(process.env.VUE_APP_ID);

  return {
    License: license,
    RequestorId: requestorId,
    AppId: appId,
    PageListModel: pageListModel,
  };
}
