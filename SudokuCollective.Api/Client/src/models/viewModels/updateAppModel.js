import Paginator from "@/models/viewModels/paginator";

class UpdateAppModel {
  constructor(
    id,
    name,
    devUrl,
    liveUrl,
    isActive,
    inDevelopment,
    permitSuperUserAccess,
    permitCollectiveLogins,
    disableCustomUrls,
    customEmailConfirmationAction,
    customPasswordResetAction,
    timeFrame,
    accessDuration
  ) {
    this.id = id;
    (this.name = name), (this.devUrl = devUrl);
    this.liveUrl = liveUrl;
    this.isActive = isActive;
    this.inDevelopment = inDevelopment;
    this.permitSuperUserAccess = permitSuperUserAccess;
    this.permitCollectiveLogins = permitCollectiveLogins;
    this.disableCustomUrls = disableCustomUrls;
    this.customEmailConfirmationAction = customEmailConfirmationAction;
    this.customPasswordResetAction = customPasswordResetAction;
    this.timeFrame = timeFrame;
    this.accessDuration = accessDuration;
    this.paginator = new Paginator();
  }
}

export default UpdateAppModel;
