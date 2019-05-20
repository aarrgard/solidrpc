
public interface store {
  getInventoryArgs getInventory();
  placeOrderArgs placeOrder();
  void deleteOrder();
  getOrderByIdArgs getOrderById();
}