import store from "../../store";
import MenuItem from "../viewModels/menuItem";

export const AppMenuItems = [
  new MenuItem(
    store.getters["appSettingsModule/getApiURL"],
    "API Status",
    "mdi-apps"
  ),
  new MenuItem(
    `${store.getters["appSettingsModule/getApiURL"]}/swagger/index.html`,
    "API Documentation",
    "mdi-open-in-new"
  ),
  new MenuItem(
    "https://github.com/Joseph-Anthony-King/SudokuCollective",
    "GitHub Page",
    "mdi-github"
  ),
];
