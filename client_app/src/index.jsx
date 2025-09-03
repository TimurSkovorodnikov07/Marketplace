import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import { store } from "./redux/store";

import "./styles/index.css";
import "./styles/login.css";
import "./styles/choose-images.css";
import "./styles/account-info.css";
import "./styles/input.css";
import "./styles/link.css";
import "./styles/productCategory.css";
import "./styles/topbar.css";
import "./styles/footer.css";
import "./styles/ads-panel.css";
import "./styles/tooltip.css";
import "./styles/delivery-company-select.css"
import "./styles/add-tag.css"
import "./styles/product-category-create-or-update.css"
import "./styles/purchased-products.css"

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <BrowserRouter>
      <Provider store={store}>
        <App />
      </Provider>
    </BrowserRouter>
  </React.StrictMode>
);
