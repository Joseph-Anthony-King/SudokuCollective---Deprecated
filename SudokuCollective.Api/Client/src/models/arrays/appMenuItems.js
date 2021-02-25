import store from "../../store";
import MenuItem from "../viewModels/menuItem";

export const AppMenuItems = [
  new MenuItem(
    store.getters["settingsModule/getApiURL"],
    "API Status",
    "mdi-apps",
    "Check the api to see if it's running",
  ),
  new MenuItem(
    `${store.getters["settingsModule/getApiURL"]}/swagger/index.html`,
    "API Documentation",
    "mdi-open-in-new",
    "Review the swagger api documentation"
  ),
  new MenuItem(
    "https://github.com/Joseph-Anthony-King/SudokuCollective",
    "GitHub Page",
    "mdi-github",
    "Review the api code on Github.com"
  ),
];
