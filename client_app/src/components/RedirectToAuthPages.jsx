import { useContext } from "react";
import { authContext } from "../contexts";
import { Navigate } from "react-router-dom";

export default function RedirectToAuthPages({ children }) {
  const authCon = useContext(authContext);
  return authCon.isAuthenticated ? (
    <>{children}</>
  ) : (
    <Navigate to={"/login"}></Navigate>
  );
}
