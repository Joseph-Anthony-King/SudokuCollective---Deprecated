import store from "@/store";

export function showToast(component, type, message, options) {

    const timeout = store.getters["appSettingsModule/getToastDuration"];

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