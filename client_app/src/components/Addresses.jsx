import Address from "../classes/Address";
import PolicyAndPrivacyPage from "../pages/PolicyAndPrivacyPage";
import LoginPage from "../pages/authPages/LoginPage";
import RegistrationPage from "../pages/authPages/RegistrationPage";
import { ProductCategoryPage } from "../pages/ProductCategoryPage";
import NotFoundPage from "../pages/errorPages/NotFoundPage";
import { SellerInfoPage } from "../pages/SellerInfoPage";
import { AccountPage } from "../pages/AccountPage";
import { ProductCategoryCreatePage } from "../pages/ProductCategoryCreatePage";
import { ProductCategoryUpdate } from "../pages/ProductCategoryUpdatePage";
import { ShopppingCartPage } from "../pages/ShoppingCartPage";
import { PurchasedProductsPage } from "../pages/PurchasedProductsPage";
import { HomePage } from "../pages/HomePage";

export function getAddresses(isAuth) {
  return [
    new Address("/", <HomePage />),
    new Address("/products/:id", <ProductCategoryPage />),
    new Address("/products/create", <ProductCategoryCreatePage />),
    new Address("/products/update/:id", <ProductCategoryUpdate />),
    new Address("/account", <AccountPage />),
    new Address("/shopping-cart", <ShopppingCartPage />),
    new Address("/sellers/:sellerId", <SellerInfoPage />),

    new Address(
      "/registration",
      isAuth === true ? <AccountPage /> : <RegistrationPage />
    ),
    new Address("/login", isAuth === true ? <AccountPage /> : <LoginPage />),
    new Address("*", <NotFoundPage />),
  ];
}
