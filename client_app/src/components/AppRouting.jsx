import { Route, Routes } from "react-router-dom";
import { getAddresses } from "./Addresses";
import MiddlewareForPages from "../middlware/MiddlewareForPages";

export default function AppRouting({ isAuth }) {
  const addresses = getAddresses(isAuth);

  return (
    <MiddlewareForPages>
      <Routes>
        {addresses.map((value, index) => {
          return (
            <Route
              key={index}
              path={value.getPath()}
              element={value.getElement()}
            />
          );
        })}
      </Routes>
    </MiddlewareForPages>
  );
}
