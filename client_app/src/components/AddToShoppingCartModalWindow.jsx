import { useSelector } from "react-redux";
import {
  addProductFromSession,
  removeFromShoppingCartFromSession,
} from "../services/shopppingCartProductFromSessionService";
import { goToLogin } from "../services/navigateService";
import { useNavigate } from "react-router-dom";

export function AddToShoppingCartModalWindow({
  haveInShoppingCart,
  id,
  numberOfPurchases,
  onAdd,
  onClean,
  className,
}) {
  const isAuth = useSelector((state) => state.auth.isAuth);
  const navigate = useNavigate();

  function add() {
    addProductFromSession({
      PurchasedCategoryId: id,
      NumberOfPurchases: numberOfPurchases,
    });
    onAdd();
  }
  function remove() {
    removeFromShoppingCartFromSession(id);
    onClean();
  }

  return haveInShoppingCart === true ? (
    <button
      className={`remove-to-shopping-cart-button ${className}`}
      onClick={() => remove()}
    >
      <span className="give-attention-text-via-color">
        Remove({numberOfPurchases})
      </span>{" "}
      to cart
    </button>
  ) : (
    <button
      className={className}
      onClick={() => {
        if (isAuth === true) add();
        else goToLogin(navigate);
      }}
    >
      <span className="give-attention-text-via-color">Add</span> to cart
    </button>
  );
}
