
public interface user {
  void createUser();
  void createUsersWithArrayInput();
  void createUsersWithListInput();
  loginUserArgs loginUser();
  void logoutUser();
  void deleteUser();
  getUserByNameArgs getUserByName();
  void updateUser();
}