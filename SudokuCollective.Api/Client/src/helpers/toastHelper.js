import store from "@/store";

export function showToast(component, type, message, options) {
  const timeout = store.getters["settingsModule/getToastDuration"];

  if (type === "show") {
    setTimeout(function () {
      component.$toasted.show(message, options);
    }, timeout);
  } else if (type === "success") {
    setTimeout(function () {
      component.$toasted.success(message, options);
    }, timeout);
  } else if (type === "info") {
    setTimeout(function () {
      component.$toasted.info(message, options);
    }, timeout);
  } else if (type === "error") {
    setTimeout(function () {
      component.$toasted.error(message, options);
    }, timeout);
  } else {
    setTimeout(function () {
      component.$toasted.show(message, options);
    }, timeout);
  }
}

export function defaultToastOptions() {
  return {
    duration: 3000,
    position: "top-center",
  };
}

export function actionToastOptions(action, icon) {
  if (icon === null) {
    return {
      action: action,
      position: "top-center",
    };
  } else {
    return {
      icon: icon,
      action: action,
      position: "top-center",
    };
  }
}
