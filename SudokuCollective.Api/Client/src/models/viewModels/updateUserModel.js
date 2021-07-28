import Paginator from "@/models/viewModels/paginator";

class UpdateUserModel {
  constructor(
    id,
    userName,
    firstName,
    lastName,
    nickName,
    email
  ) {
    this.id = id;
    this.userName = userName,
    this.firstName = firstName;
    this.lastName = lastName;
    this.nickName = nickName;
    this.email = email;
    this.paginator = new Paginator();
  }
}

export default UpdateUserModel;