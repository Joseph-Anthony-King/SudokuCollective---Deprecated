import { requestData } from "../requestData";

export function requestDataUpdatePassword(pageListModel, oldPassword, newPassword) {

    let result = requestData(pageListModel);

    result["OldPassword"] = oldPassword;
    result["NewPassword"] = newPassword;

    return result;
}

export function requestDataUpdateUser(
    pageListModel,
    userName,
    firstName,
    lastName,
    nickName,
    email) {

    let result = requestData(pageListModel);

    result["UserName"] = userName;
    result["FirstName"] = firstName;
    result["LastName"] = lastName;
    result["NickName"] = nickName;
    result["Email"] = email;

    return result;
}

export function requestDataUpdatePassword(pageListModel, roleIds) {

    let result = requestData(pageListModel);

    result["RoleIds"] = roleIds;

    return result;
}