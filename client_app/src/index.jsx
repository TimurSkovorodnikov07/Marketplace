import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import { store } from "./redux/store";

import "./styles/index.css";
import "./styles/login.css";
import "./styles/image.css";
import "./styles/account-info.css";
import "./styles/input.css";
import "./styles/link.css";
import "./styles/productCategory.css";
import "./styles/topbar.css";
import "./styles/footer.css";
import "./styles/ads-panel.css";
import "./styles/tooltip.css";

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
